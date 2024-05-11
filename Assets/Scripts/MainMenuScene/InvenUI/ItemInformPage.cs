using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformPage : MonoBehaviour
{

    public GameObject upgradePageobj;
    public GameObject sellPageobj;

    public Image itemImage;
    public Image RankImage;
    public TextMeshProUGUI informtext;
    public TextMeshProUGUI MainAbility;
    public TextMeshProUGUI SubAbility1;
    public TextMeshProUGUI SubAbility2;
    public TextMeshProUGUI SubAbility3;
    public TextMeshProUGUI SubAbility4;
    public TextMeshProUGUI SubAbility5;

    public bool isSet;
    
    void Awake()
    {
        isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSet)
        {

        }
    }

    public void BackBtn()
    {
        gameObject.SetActive(false);
    }
    public void UpgradeBtn()
    {
        upgradePageobj.SetActive(true);
    }
    public void SellBtn()
    {
        sellPageobj.SetActive(true);
    }

}
