using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class ItemUIPref : MonoBehaviour
{
    //todo -> 현재 파츠만 해당되게 만들어져 있는데. 모든 아이템을 넣을 수 있도록 수정할것

    //해당 UI에 파츠 혹은 아이템(재료, 소모품) 할당
    //파츠는 지금 있는거에 새롭게 넣은 UI요소 할당, 아이템은 새로 적용

    public InvenData invenData;
    public PartsAbilityData PartsAbilityData; //소유한 파츠의 데이터
    public MasterData itemData; //소유한 파츠 이외의 아이템이나 소유하지 않은 모든 아이템의 데이터

    [SerializeField] private Image bgImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject selectText;
    [SerializeField] private GameObject amountText;

    public Sprite defaultImage;
    public MasterType curItemType; //2면 파츠, 나머지면 다른 아이템

    //public int ItemAmount 
    //{ 
    //    get => invenData.quantity; 
    //}

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
            PartsAbilityData.invenId = -1;
            curItemType = MasterType.Parts;
            return;
        }
        invenData = DataManager.inven.GetData(invenId);

        MasterType itemtype = DataManager.master.GetData(invenData.masterId).type;
        if(itemtype == MasterType.Parts) //파츠일경우
        {
            PartsAbilityData = DataManager.parts.GetData(invenData.id);
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
        if (curItemType == MasterType.Parts)
        {
            switch (PartsAbilityData.rank)
            {
                case 5: bgImage.color = PartsGradeColor.S_Color; break;
                case 4: bgImage.color = PartsGradeColor.A_Color; break;
                case 3: bgImage.color = PartsGradeColor.B_Color; break;
                case 2: bgImage.color = PartsGradeColor.C_Color; break;
                case 1: bgImage.color = PartsGradeColor.D_Color; break;
                default: bgImage.color = Color.black; break;
            }

            MasterData master = DataManager.master.GetData(DataManager.inven.GetData(PartsAbilityData.invenId).masterId);

            Sprite image = Resources.Load<Sprite>(master.spritePath);
            if (image == null)
            {
                image = defaultImage;
            }

            itemImage.sprite = image;
            selectText.SetActive(PartsAbilityData.isActive == true ? true : false);
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
            UpdateItemAmount();
        }
    }

    public void UpdateItemAmount()
    {
        amountText.GetComponentInChildren<TextMeshProUGUI>().text = invenData.quantity.ToString();
    }

    public void ResetData()
    {
        invenData = new InvenData();
        itemData = new MasterData();
        PartsAbilityData = new PartsAbilityData();

        bgImage.color = Color.white;
        itemImage.sprite = null;
        selectText.SetActive(false);
    }
}
