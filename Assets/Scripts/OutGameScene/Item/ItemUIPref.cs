using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public struct PartsGradeColor
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
    public PartsData partsData; //소유한 파츠의 데이터
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
        itemData = DataManager.master.GetData(masterId);
        
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
        invenData = DataManager.inven.GetData(invenId);

        ItemType itemtype = DataManager.master.GetData(invenData.masterId).type;
        if(itemtype == (ItemType)2) //파츠일경우
        {
            partsData = DataManager.parts.GetData(invenData.id);
        }
        else
        {
            itemData = DataManager.master.GetData(invenData.masterId);
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
                case 5: bgImage.color = PartsGradeColor.S_Color; break;
                case 4: bgImage.color = PartsGradeColor.A_Color; break;
                case 3: bgImage.color = PartsGradeColor.B_Color; break;
                case 2: bgImage.color = PartsGradeColor.C_Color; break;
                case 1: bgImage.color = PartsGradeColor.D_Color; break;
                default: bgImage.color = Color.black; break;
            }

            MasterData master = DataManager.master.GetData(DataManager.inven.GetData(partsData.invenId).masterId);

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
        partsData = new PartsData();

        bgImage.color = Color.white;
        itemImage.sprite = null;
        selectText.SetActive(false);
    }
}
