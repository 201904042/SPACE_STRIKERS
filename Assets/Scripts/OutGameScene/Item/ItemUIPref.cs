using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class ItemUIPref : MonoBehaviour
{
    //todo -> ���� ������ �ش�ǰ� ������� �ִµ�. ��� �������� ���� �� �ֵ��� �����Ұ�

    //�ش� UI�� ���� Ȥ�� ������(���, �Ҹ�ǰ) �Ҵ�
    //������ ���� �ִ°ſ� ���Ӱ� ���� UI��� �Ҵ�, �������� ���� ����

    public InvenData invenData;
    public PartsData PartsAbilityData; //������ ������ ������
    public MasterData itemData; //������ ���� �̿��� �������̳� �������� ���� ��� �������� ������

    [SerializeField] private Image bgImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject selectText;
    [SerializeField] private GameObject amountText;

    public Sprite defaultImage;
    public MasterType curItemType; //3�� ����, �������� �ٸ� ������

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
    /// ���� �� �������� ���� �������� �ε��� ���
    /// </summary>
    public void SetByMasterId(int masterId)
    {
        ResetData();
        itemData = DataManager.master.GetData(masterId);
        
        curItemType = itemData.type;
        SetData();
    }

    /// <summary>
    /// �÷��̾ ������ �������� �ε� �� ���
    /// </summary>
    public void SetByInvenId(int invenId)
    {
        ResetData();
        if (invenId == 0)
        {
            PartsAbilityData.invenId = 0;
            curItemType = MasterType.Parts;
            return;
        }

        invenData = DataManager.inven.GetData(invenId);

        MasterType itemtype = DataManager.master.GetData(invenData.masterId).type;
        if(itemtype == MasterType.Parts) //�����ϰ��
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
        PartsAbilityData = new PartsData();

        bgImage.color = Color.white;
        itemImage.sprite = null;
        selectText.SetActive(false);
    }
}
