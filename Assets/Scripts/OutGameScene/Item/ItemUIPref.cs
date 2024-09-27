using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public struct GradeColor
{
    public static Color S_Color = new Color(1, 1, 0, 1);
    public static Color A_Color = new Color(1, 0, 1, 1);
    public static Color B_Color = new Color(0, 0, 1, 1);
    public static Color C_Color = new Color(0, 1, 0, 1);
    public static Color D_Color = new Color(1, 1, 1, 1);
}

public class ItemUIPref : MonoBehaviour
{
    //todo -> 현재 파츠만 해당되게 만들어져 있는데. 모든 아이템을 넣을 수 있도록 수정할것

    //해당 UI에 파츠 혹은 아이템(재료, 소모품) 할당
    //파츠는 지금 있는거에 새롭게 넣은 UI요소 할당, 아이템은 새로 적용

    public InvenData invenData;
    public InvenPartsData partsData; //소유한 파츠의 데이터
    public MasterData itemData; //소유한 파츠 이외의 아이템이나 소유하지 않은 모든 아이템의 데이터

    [SerializeField] private Image bgImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject selectText;
    [SerializeField] private GameObject amountText;

    public Sprite defaultImage;

    public ItemType curItemType; //2면 파츠, 나머지면 다른 아이템

    private void Awake()
    {
        bgImage = transform.GetChild(0).GetComponent<Image>();
        itemImage = transform.GetChild(1).GetComponent<Image>();
        selectText = transform.GetChild(2).gameObject;
        amountText = transform.GetChild(3).gameObject;
        selectText.SetActive(false);
        amountText.SetActive(false);
    }

    /// <summary>
    /// 상점 등 소유하지 않은 아이템을 로드할 경우
    /// </summary>
    public void SetByMasterId(int masterId)
    {
        ResetData();
        bool isSuccess = DataManager.masterData.masterDic.TryGetValue(masterId, out itemData);
        if (!isSuccess)
        {
            Debug.Log("마스터 데이터를 찾지 못함");
        }
        curItemType = itemData.type;
        SetData();
    }

    /// <summary>
    /// 플레이어가 소유한 아이템을 로드 할 경우
    /// </summary>
    public void SetByInvenId(int invenId)
    {
        ResetData();
        if (invenId == -1)
        {
            partsData.invenId = -1;
            curItemType = (ItemType)2;
            return;
        }

        bool isSuccess = DataManager.inventoryData.InvenItemDic.TryGetValue(invenId, out invenData);
        if (!isSuccess)
        {
            Debug.Log("아이템 검색 실패");
        }

        ItemType itemtype = DataManager.masterData.GetData(invenData.masterId).Value.type;
        if(itemtype == (ItemType)2) //파츠일경우
        {
            isSuccess = DataManager.partsData.invenPartsDic.TryGetValue(invenData.id, out partsData);
            if (!isSuccess)
            {
                Debug.Log("파츠 데이터를 찾지 못함");
            }
        }
        else
        {
            isSuccess = DataManager.masterData.masterDic.TryGetValue(invenData.masterId, out itemData);
            if (!isSuccess)
            {
                Debug.Log("마스터 데이터를 찾지 못함");
            }
        }

        curItemType = itemtype;
        SetData();
    }

    private void SetData()
    {
        if (curItemType == (ItemType)2)
        {
            switch (partsData.rank)
            {
                case 5: bgImage.color = GradeColor.S_Color; break;
                case 4: bgImage.color = GradeColor.A_Color; break;
                case 3: bgImage.color = GradeColor.B_Color; break;
                case 2: bgImage.color = GradeColor.C_Color; break;
                case 1: bgImage.color = GradeColor.D_Color; break;
                default: bgImage.color = Color.black; break;
            }

            MasterData master = (MasterData)DataManager.masterData.GetData(DataManager.inventoryData.GetData(partsData.invenId).Value.masterId);

            Sprite image = Resources.Load<Sprite>(master.spritePath);
            if (image == null)
            {
                image = defaultImage;
            }

            itemImage.sprite = image;
            selectText.SetActive(partsData.isActive == true ? true : false);
        }
        else
        {
            Sprite image = Resources.Load<Sprite>(itemData.spritePath);
            if (image == null)
            {
                image = defaultImage;
            }

            itemImage.sprite = image;
            amountText.SetActive(true);
            amountText.GetComponentInChildren<TextMeshProUGUI>().text = invenData.quantity.ToString();
        }
    }

    public void ResetData()
    {
        invenData = new InvenData();
        itemData = new MasterData();
        partsData = new InvenPartsData();

        bgImage.color = Color.white;
        itemImage.sprite = null;
        selectText.SetActive(false);
    }
}
