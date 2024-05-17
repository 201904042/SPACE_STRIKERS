using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PartsInvenPref : MonoBehaviour
{
    public int PartsId;
    public int PartsCode;
    public string PartsName;
    public string PartsType;
    public int PartsLevel;
    public string PartsRank;
    public int mainAmount;
    public string Partsability1;
    public int abilityAmount1;
    public string Partsability2;
    public int abilityAmount2;
    public string Partsability3;
    public int abilityAmount3;
    public string Partsability4;
    public int abilityAmount4;
    public string Partsability5;
    public int abilityAmount5;
    public string PartsMainAbility;
    
    public Image itemImage;
    public Image rankAuraImage;

    public Sprite partsSprite1;
    public Sprite partsSprite2;
    public Sprite partsSprite3;
    public Sprite partsSprite4;
    public Sprite partsSprite5;
    public GameObject inventoryScroll;
    public GameObject partsInformPage;
    
    private ItemInformPage partsinformSrt;


    private bool isFirstSet;

    void Awake()
    {
        isFirstSet = false;
        inventoryScroll = transform.parent.parent.parent.gameObject;
        partsInformPage = inventoryScroll.transform.parent.GetChild(4).gameObject;
        partsinformSrt = partsInformPage.GetComponent<ItemInformPage>();
       
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

            mainAbilitySet();
            isFirstSet =true;
        }

        
    }

    void mainAbilitySet()
    {
        switch (PartsCode)
        {
            case 1:
                PartsMainAbility = "Dmg"; break;
            case 2:
                PartsMainAbility = "MSpd"; break;
            case 3:
                PartsMainAbility = "ASpd"; break;
            case 4:
                PartsMainAbility = "ExpUp"; break;
            case 5:
                PartsMainAbility = "ItemUp"; break;
            case 6:
                PartsMainAbility = "Def"; break;
        }
        Debug.Log(PartsCode);
        Debug.Log(PartsMainAbility);
    }

    public void invenBtn()
    {
        if (inventoryScroll.GetComponent<invenScroll>().selectedInvenCode != PartsId)
        {
            inventoryScroll.GetComponent<invenScroll>().selectedInvenCode = PartsId;
        }
        else
        {
            partsinformSrt.itemImage.sprite = itemImage.sprite;
            partsinformSrt.RankImage.color =rankAuraImage.color;
            partsinformSrt.MainAbility.text = PartsMainAbility+" "+mainAmount;

            partsinformSrt.informtext.text = PartsName + "\n" + "LV." + PartsLevel + "\n" + "Rank." + PartsRank;

            if (Partsability1 != "null")
                partsinformSrt.SubAbility1.text = Partsability1 + " +" + abilityAmount1;
            else
                partsinformSrt.SubAbility1.text = "Àá±è";
            if (Partsability2 != "null")
                partsinformSrt.SubAbility2.text = Partsability2 + " +" + abilityAmount2;
            else
                partsinformSrt.SubAbility2.text = "Àá±è";
            if (Partsability3 != "null")
                partsinformSrt.SubAbility3.text = Partsability3 + " +" + abilityAmount3;
            else
                partsinformSrt.SubAbility3.text = "Àá±è";
            if (Partsability4 != "null")
                partsinformSrt.SubAbility4.text = Partsability4 + " +" + abilityAmount4;
            else
                partsinformSrt.SubAbility4.text = "Àá±è";
            if (Partsability5 != "null")
                partsinformSrt.SubAbility5.text = Partsability5 + " +" + abilityAmount5;
            else
                partsinformSrt.SubAbility5.text = "Àá±è";

            partsInformPage.GetComponent<ItemInformPage>().itemLevel = PartsLevel;
            partsInformPage.GetComponent<ItemInformPage>().itemRank = PartsRank;
            partsInformPage.GetComponent<ItemInformPage>().itemCode = PartsCode;
            partsInformPage.SetActive(true);
        }
    }
}
