using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUI : UI_Parent
{
    public GameObject upgradeinterfaceobj;
    public GameObject sellinterfaceobj;

    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackBtn()
    {
        gameObject.SetActive(false);
        Main.SetActive(true);
    }
    public void UpgradeBtn()
    {
        upgradeinterfaceobj.SetActive(true);
    }
    public void SellBtn()
    {
        sellinterfaceobj.SetActive(true);
    }
}
