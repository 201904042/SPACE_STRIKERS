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

    protected override void OnEnable()
    {
        base.OnEnable();
        UIInit();
    }

    protected override void UIInit()
    {
        Transform buttons = transform.GetChild(0);

        planetButton = buttons.GetChild(0).GetComponent<Button>();
        storeButton = buttons.GetChild(1).GetComponent<Button>();
        invenButton = buttons.GetChild(2).GetComponent<Button>();

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
        ChangeUI(UIManager.UIInstance.PlanetUIObj);
    }

    public void GotoStoreBtn()
    {
        ChangeUI(UIManager.UIInstance.StoreUIObj);

    }
    public void GotoInvenBtn()
    {
        ChangeUI(UIManager.UIInstance.InventoryUIObj);
    }
}
