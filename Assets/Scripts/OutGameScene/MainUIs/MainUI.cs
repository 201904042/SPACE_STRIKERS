using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MainUIs
{
    private Button planetButton;
    private Button storeButton;
    private Button invenButton;

    //public GameObject EventBannerZone; º¸·ù

    
    public override void SetComponent()
    {
        base.SetComponent();

        Transform buttons = transform.GetChild(0);

        planetButton = buttons.GetChild(0).GetComponent<Button>();
        storeButton = buttons.GetChild(1).GetComponent<Button>();
        invenButton = buttons.GetChild(2).GetComponent<Button>();
    }


    public override IEnumerator SetUI()
    {
        yield return base.SetUI();
        Init();
    }

    private void Init()
    {
        if (planetButton != null)
        {
            planetButton.onClick.RemoveAllListeners();
            planetButton.onClick.AddListener(GotoPlanetBtn);
        }

        if (storeButton != null)
        {
            storeButton.onClick.RemoveAllListeners();
            storeButton.onClick.AddListener(GotoStoreBtn);
        }

        if (invenButton != null)
        {
            invenButton.onClick.RemoveAllListeners();
            invenButton.onClick.AddListener(GotoInvenBtn);
        }
    }

    

    public void GotoPlanetBtn()
    {
        ChangeUI(OG_UIManager.UIInstance.planetUI);
    }

    public void GotoStoreBtn()
    {
        ChangeUI(OG_UIManager.UIInstance.storeUI);

    }
    public void GotoInvenBtn()
    {
        ChangeUI(OG_UIManager.UIInstance.invenUI);
    }
}
