using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseInterface : UIInterface
{
    public Transform Content;
    public Image itemImage;
    public TextMeshProUGUI itemText;
    public Transform Btns;
    public Button cancelBtn;
    public Button purchaseBtn;

    public MasterItemData itemData;
    public int resultPrice;
    public int itemAmount;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        Content = transform.GetChild(2);
        itemImage = Content.GetChild(0).GetComponent<Image>();
        itemText = Content.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        Btns = transform.GetChild(3);
        cancelBtn = Btns.GetChild(0).GetComponent<Button>();
        purchaseBtn = Btns.GetChild(1).GetComponent<Button>();

        itemData = new MasterItemData();
    }

    /// <summary>
    /// ���� ��ũ��Ʈ���� ���.
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //���� �ʱ�ȭ
        result = null;

        //Ȯ�� ���. ��ư �ڵ鷯 ����
        purchaseBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(() => OnConfirm(true));
        cancelBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //Ȯ���� �������� ��ȯ�� ����
        yield return result.Value;
    }

    /// <summary>
    /// ������ �Ű������� �������̽��� ������. 
    /// </summary>
    public bool SetPurchaseInterface(int itemMasterCode, int itemPrice, int itemAmount = 1)
    {
        bool success = DataManager.masterData.masterItemDic.TryGetValue(itemMasterCode, out itemData);
        if (!success) 
        {
            Debug.Log($"�ش� �ڵ带 �˻����� ���� {itemMasterCode}");
            return false;
        }

        itemImage.sprite = Resources.Load<Sprite>(itemData.spritePath);
        itemText.text = itemData.description;
        resultPrice = itemPrice* itemAmount; //todo �̰��� �ذ��ؾ���. ���� ������ ���ϻ������� �� �� ��찡 ����
        this.itemAmount  = itemAmount;
        purchaseBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"�� ��\n{resultPrice}";

        return true;
    }
}
