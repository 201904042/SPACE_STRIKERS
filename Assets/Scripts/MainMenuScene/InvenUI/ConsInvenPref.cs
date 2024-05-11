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
                case "�н�AI": itemImage.sprite = itemSprite2; break;
                case "�ڱ����ھ�": itemImage.sprite = itemSprite3; break;
                case "��ȣ�����͸�": itemImage.sprite = itemSprite4; break;
                case "������ձݰ���": itemImage.sprite = itemSprite5; break;
            }

            itemAmountText.text = consAmount.ToString();
            isFirstSet = true;
        }
    }
}
