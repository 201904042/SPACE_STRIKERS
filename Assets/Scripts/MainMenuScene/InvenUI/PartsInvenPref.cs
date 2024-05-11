using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartsInvenPref : MonoBehaviour
{
    public int PartsId;
    public int PartsCode;
    public string PartsName;
    public string PartsType;
    public int PartsLevel;
    public string PartsRank;
    public string Partsability1;
    public string Partsability2;
    public string Partsability3;
    public string Partsability4;
    public string Partsability5;

    public Image itemImage;
    public Image rankAuraImage;

    public Sprite partsSprite1;
    public Sprite partsSprite2;
    public Sprite partsSprite3;
    public Sprite partsSprite4;
    public Sprite partsSprite5;
    public GameObject partsInformPage;

    public invenScroll inventoryScroll;

    private bool isFirstSet;

    void Awake()
    {
        isFirstSet = false;
        inventoryScroll = transform.parent.parent.parent.GetComponent<invenScroll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFirstSet) {
            switch(PartsCode)
            {
                case 1: itemImage.sprite = partsSprite1; break;
                case 2: itemImage.sprite = partsSprite2; break;
                case 3: itemImage.sprite = partsSprite3; break;
                case 4: itemImage.sprite = partsSprite4; break;
                case 5: itemImage.sprite = partsSprite5; break;
            }

            switch (PartsRank)
            {
                case "D":rankAuraImage.color = Color.white; break;
                case "C": rankAuraImage.color = Color.green; break;
                case "B": rankAuraImage.color = Color.blue; break;
                case "A": rankAuraImage.color = Color.magenta; break;
                case "S": rankAuraImage.color = Color.yellow; break;
            }
            isFirstSet=true;
        }

        
    }

    public void invenBtn()
    {
        if(inventoryScroll.selectedInvenCode != PartsId)
        {
            inventoryScroll.selectedInvenCode = PartsId;
        }
        else
        {
            partsInformPage.SetActive(true);
        }
    }
}
