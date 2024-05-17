using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformPage : MonoBehaviour
{

    public GameObject upgradePageobj;
    public GameObject sellPageobj;
    private UpgradePage upgradePage;

    public Image itemImage;
    public Image RankImage;
    public TextMeshProUGUI informtext;
    public TextMeshProUGUI MainAbility;
    public TextMeshProUGUI SubAbility1;
    public TextMeshProUGUI SubAbility2;
    public TextMeshProUGUI SubAbility3;
    public TextMeshProUGUI SubAbility4;
    public TextMeshProUGUI SubAbility5;

    public string itemRank;
    public int itemLevel;
    public int itemCode;

    private void Awake()
    {
        upgradePage = upgradePageobj.GetComponent<UpgradePage>();
    }

    public void BackBtn()
    {
        gameObject.SetActive(false);
    }
    public void UpgradeBtn()
    {
        upgradePage.upgradImage.sprite = itemImage.sprite;
        //upgradePage.upgradeIngred.text 
       // upgradePage.upgradeExplain.text
        upgradePageobj.SetActive(true);
    }
    public void SellBtn()
    {
        sellPageobj.SetActive(true);
    }

    
}
