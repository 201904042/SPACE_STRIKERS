using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredInvenPref : MonoBehaviour
{
    public int ingredId;
    public string ingredName;
    public int ingredAmount;

    public Image itemImage;
    public TextMeshProUGUI itemAmountText;

    public Sprite itemSprite1;
    public Sprite itemSprite2;
    public Sprite itemSprite3;
    public Sprite itemSprite4;
    public Sprite itemSprite5;
    private bool isFirstSet;
    void Awake()
    {
        isFirstSet = false;
    }

    void Update()
    {
        if (!isFirstSet)
        {
            switch (ingredName)
            {
                case "��ö": itemImage.sprite = itemSprite1; break;
                case "����": itemImage.sprite = itemSprite2; break;
                case "���": itemImage.sprite = itemSprite3; break;
                case "Ĩ��": itemImage.sprite = itemSprite4; break;
                case "���͸�": itemImage.sprite = itemSprite5; break;
            }

            itemAmountText.text = ingredAmount.ToString();
            isFirstSet = true;
        }
    }
}
