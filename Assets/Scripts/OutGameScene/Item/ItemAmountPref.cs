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
        ownAmountText.color = Color.black;
    }

    public void SetAmountUI(int masterId, float needAmount)
    {
        InvenData invenData = DataManager.inven.GetDataWithMasterId(masterId);
        MasterData masterData = DataManager.master.GetData(masterId);

        itemImage.sprite = Resources.Load<Sprite>(masterData.spritePath);


        needAmountText.text = $"{needAmount}";

        int haveAmount = 0;
        if(invenData != null && invenData.quantity > 0)
        {
            haveAmount = invenData.quantity;
        }

        ownAmountText.text = $"{haveAmount}";
        if (needAmount > haveAmount)
        {
            ownAmountText.color = Color.red;
        }
    }
}
