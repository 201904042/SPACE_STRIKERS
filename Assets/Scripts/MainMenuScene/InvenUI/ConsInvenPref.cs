using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsInvenPref : MonoBehaviour
{
    public int consId;
    public string consName;
    public int consAmount;

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
            switch (consName)
            {
                case "BST-35": itemImage.sprite = itemSprite1; break;
                case "학습AI": itemImage.sprite = itemSprite2; break;
                case "자기장코어": itemImage.sprite = itemSprite3; break;
                case "보호막배터리": itemImage.sprite = itemSprite4; break;
                case "드라코합금갑판": itemImage.sprite = itemSprite5; break;
            }

            itemAmountText.text = consAmount.ToString();
            isFirstSet = true;
        }
    }
}
