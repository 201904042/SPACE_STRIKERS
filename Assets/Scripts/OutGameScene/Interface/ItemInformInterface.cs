using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemInformInterface : UIInterface
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Transform Images;
    public Image rankImage;
    public Image itemImage;
    public Transform Buttons;
    public Button cancelBtn;
    public Button sellBtn;

    public int invenItemId; //�κ��丮���� �˻��� ���̵�


    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        Images = transform.GetChild(2);
        Buttons = transform.GetChild(4);

        nameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        descriptionText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

        rankImage = Images.GetChild(0).GetComponentInChildren<Image>();
        itemImage = Images.GetChild(1).GetComponentInChildren<Image>();

        cancelBtn = Buttons.GetChild(0).GetComponentInChildren<Button>();
        sellBtn = Buttons.GetChild(1).GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        SetButtons();
    }

    /// <summary>
    /// �κ��丮 ItemUIpref��ư���� �ҷ�����
    /// </summary>

    public void SetInterface(int targetInvenId)
    {
        invenItemId = targetInvenId;

        MasterData masterData = DataManager.master.GetData(DataManager.inven.GetData(invenItemId).masterId);
        nameText.text = masterData.name;
        descriptionText.text = masterData.description;
        itemImage.sprite = Resources.Load<Sprite>(masterData.spritePath);
        if(masterData.type == MasterType.Parts)
        {
            int rank = DataManager.parts.GetData(invenItemId).rank;
            switch (rank)
            {
                case 5: rankImage.color = PartsGradeColor.S_Color; break;
                case 4: rankImage.color = PartsGradeColor.A_Color; break;
                case 3: rankImage.color = PartsGradeColor.B_Color; break;
                case 2: rankImage.color = PartsGradeColor.C_Color; break;
                case 1: rankImage.color = PartsGradeColor.D_Color; break;
                default: rankImage.color = Color.white; break;
            }
        }
        else
        {
            rankImage.color = Color.white;
        }

        sellBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"�� ��\n({500})";//todo -> ������ �ǸŰ��� �����Ұ�
    }

    public void SetButtons()
    {
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => CancelBtnHandler());
        sellBtn.onClick.RemoveAllListeners();
        sellBtn.onClick.AddListener(() => SellBtnHandler());

    }

    public void CancelBtnHandler()
    {
        gameObject.SetActive(false);
    }

    public void SellBtnHandler()
    {
        UIManager.tfInterface.SetTFContent("������ �Ǹ��Ͻðڽ��ϱ�?");
        StartCoroutine(DoubleCheck());
    }

    private IEnumerator DoubleCheck()
    {
        TFInterface tfInterface = UIManager.tfInterface.GetComponent<TFInterface>();
        // TF �������̽����� ����� ��ٸ�
        yield return StartCoroutine(tfInterface.GetValue());

        // ����� ���� ������ �Ǹ� ó��
        if ((bool)tfInterface.result)
        {
            SellItem(invenItemId);

            //�ش� ������UI�� ���� ������Ʈ
            ItemUIPref itemPref = FindItemUIPref(invenItemId);
            if(itemPref == null)
            {
                yield return null;
            } 
            itemPref.UpdateItemAmount();
        }
        else
        {
            UIManager.alertInterface.SetAlert($"�ǸŰ� ��ҵǾ����ϴ�");
        }
    }

   

    private ItemUIPref FindItemUIPref(int invenId)
    {
        ItemUIPref[] itemUis = FindObjectsOfType<ItemUIPref>();
        foreach (ItemUIPref itemUI in itemUis)
        {
            if (itemUI.invenData.id == invenId)
            {
                return itemUI;
            }
        }
        return null;
    }

    private void SellItem(int invenItemId)
    {
        TradeData sellData = new TradeData()
        {
            tradeCost = TradeType.Item,
            costId = DataManager.inven.GetData(invenItemId).masterId,
            costAmount = 1,
            targetId = 1, //�̳׶�
            tradeAmount = 1000,
            isMultiTrade= false
        };
        
        StoreUI.TradeItem(sellData);
        UIManager.alertInterface.SetAlert($"������ {invenItemId}��(��) �ǸŵǾ����ϴ�.");
    }
}
