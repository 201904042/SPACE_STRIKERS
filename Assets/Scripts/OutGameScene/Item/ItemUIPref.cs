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
    public InvenPartsData partsData; //������ ������ ������
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
        bool isSuccess = DataManager.masterData.masterDic.TryGetValue(masterId, out itemData);
        if (!isSuccess)
        {
            Debug.Log("������ �����͸� ã�� ����");
        }
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

        bool isSuccess = DataManager.inventoryData.InvenItemDic.TryGetValue(invenId, out invenData);
        if (!isSuccess)
        {
            Debug.Log("������ �˻� ����");
        }

        ItemType itemtype = DataManager.masterData.GetData(invenData.masterId).Value.type;
        if(itemtype == (ItemType)2) //�����ϰ��
        {
            isSuccess = DataManager.partsData.invenPartsDic.TryGetValue(invenData.id, out partsData);
            if (!isSuccess)
            {
                Debug.Log("���� �����͸� ã�� ����");
            }
        }
        else
        {
            isSuccess = DataManager.masterData.masterDic.TryGetValue(invenData.masterId, out itemData);
            if (!isSuccess)
            {
                Debug.Log("������ �����͸� ã�� ����");
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
