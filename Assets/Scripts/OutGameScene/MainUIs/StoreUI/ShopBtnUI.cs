using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBtnUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI costText;
    public Button button;

    public int targetItemId;
    public int itemPrice;


    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        costText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

    }

    public bool SetUIValue(int itemMasterId ,Sprite image ,int price= 1000, bool isActive = true)
    {
        targetItemId = itemMasterId;
        itemImage.sprite = image;
        itemPrice = price;


        costText.text = $"��� : {itemPrice}"; //���ϻ������� ���ϵ� �����ϼ� ����

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PurchaseBtnHandler);

        button.interactable = isActive; //�̹� ���Ű� �Ϸ�Ǿ��ٸ� false��

        return true;
    }

    private void PurchaseBtnHandler()
    {
        StartCoroutine(ConfiremPurchase());
        UIManager.purchaseInterface.SetPurchaseInterface(targetItemId, itemPrice);
    }

    private IEnumerator ConfiremPurchase()
    {
        PurchaseInterface purchaseInterface = UIManager.purchaseInterface;

        yield return StartCoroutine(purchaseInterface.GetValue());

        if ((bool)purchaseInterface.result)
        {
            //������ ���� �������� Ȯ�� TFâ ���
            DoubleCheck();
        }
        else
        {
            purchaseInterface.CloseInterface();
        }
    }

    private void DoubleCheck()
    {
        UIManager.tfInterface.SetTFContent("������ �����Ͻðڽ��ϱ�?");
        StartCoroutine(TFCheck());
    }

    private IEnumerator TFCheck()
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //����üũ �Ϸ�� ���� ����
            StoreUI.ItemPurchase(targetItemId, itemPrice);
            UIManager.alterInterface.SetAlert($"���Ű� �Ϸ�Ǿ����ϴ�");
        }
        else
        {
            UIManager.alterInterface.SetAlert($"���Ű� ��ҵǾ����ϴ�");
        }
    }
}
