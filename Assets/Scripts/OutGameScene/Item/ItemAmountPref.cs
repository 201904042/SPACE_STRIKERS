using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemAmountPref : MonoBehaviour
{
    public Image itemImage;
    public Transform textGroup;
    public TextMeshProUGUI ownAmountText;
    public TextMeshProUGUI needAmountText;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        textGroup = transform.GetChild(1);
        ownAmountText = textGroup.GetChild(0).GetComponent<TextMeshProUGUI>();
        needAmountText = textGroup.GetChild(1).GetComponent<TextMeshProUGUI>();
        
    }

    private void OnEnable()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        itemImage.sprite = null;
        needAmountText.text = "";
        ownAmountText.text = "";
    }

    public void SetAmountUI(int masterId, int needAmount)
    {
        MasterData masterData = new MasterData();

        InvenData invenData = new InvenData();
        bool success = DataManager.inventoryData.InvenItemDic.TryGetValue(masterId, out invenData);
        if (!success)
        {
            ownAmountText.text = $"0";
        }
        ownAmountText.text = $"{invenData.quantity}";

        success = DataManager.masterData.masterDic.TryGetValue(masterId, out masterData);
        if (!success)
        {
            Debug.Log("아이디가 마스터 데이터에 없음");
            return;
        }

        itemImage.sprite = Resources.Load<Sprite>(masterData.spritePath);

        needAmountText.text = $"{needAmount}";

        if(needAmount > invenData.quantity)
        {
            ownAmountText.color = Color.red;
        }
        
    }
}
