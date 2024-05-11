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
                case "고철": itemImage.sprite = itemSprite1; break;
                case "전선": itemImage.sprite = itemSprite2; break;
                case "기어": itemImage.sprite = itemSprite3; break;
                case "칩셋": itemImage.sprite = itemSprite4; break;
                case "배터리": itemImage.sprite = itemSprite5; break;
            }

            itemAmountText.text = ingredAmount.ToString();
            isFirstSet = true;
        }
    }
}
