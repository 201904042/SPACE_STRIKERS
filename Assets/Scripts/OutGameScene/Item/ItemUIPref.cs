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
    //todo -> ���� ������ �ش�ǰ� ������� �ִµ�. ��� �������� ���� �� �ֵ��� �����Ұ�

    //�ش� UI�� ���� Ȥ�� ������(���, �Ҹ�ǰ) �Ҵ�
    //������ ���� �ִ°ſ� ���Ӱ� ���� UI��� �Ҵ�, �������� ���� ����

    public InvenData invenData;
    public PartsData partsData; //������ ������ ������
    public MasterData itemData; //������ ���� �̿��� �������̳� �������� ���� ��� �������� ������

    [SerializeField] private Image bgImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject selectText;
    [SerializeField] private GameObject amountText;

    public Sprite defaultImage;

    public ItemType curItemType; //2�� ����, �������� �ٸ� ������

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
        if (invenId == -1)
        {
            partsData.invenId = -1;
            curItemType = (ItemType)2;
            return;
        }
        invenData = DataManager.inven.GetData(invenId);

        ItemType itemtype = DataManager.master.GetData(invenData.masterId).type;
        if(itemtype == (ItemType)2) //�����ϰ��
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
                case 5: bgImage.color = GradeColor.S_Color; break;
                case 4: bgImage.color = GradeColor.A_Color; break;
                case 3: bgImage.color = GradeColor.B_Color; break;
                case 2: bgImage.color = GradeColor.C_Color; break;
                case 1: bgImage.color = GradeColor.D_Color; break;
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
