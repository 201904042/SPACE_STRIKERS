using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance;

    //UIs
    public GameObject HeaderUIObj;

    public GameObject MainUIObj;
    public GameObject PlanetUIObj;
    public GameObject StageUIObj;
    public GameObject ReadyUIObj;
    public GameObject StoreUIObj;
    public GameObject InventoryUIObj;
    public GameObject LabotoryUIObj;

    public GameObject LoadingUI;

    //Interface
    public GameObject OptionInterface;

    private void Awake()
    {
        if (UIInstance == null)
        {
            UIInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UISetting();
        UIInit();
    }

    private void UISetting()
    {
        HeaderUIObj = FindObjectOfType<MainHeaderUI>().gameObject;

        MainUIObj = FindObjectOfType<MainUI>().gameObject;
        PlanetUIObj = FindObjectOfType<PlanetUI>().gameObject;
        StageUIObj = FindObjectOfType<StageUI>().gameObject;
        ReadyUIObj = FindObjectOfType<ReadyUI>().gameObject;
        StoreUIObj = FindObjectOfType<StoreUI>().gameObject;
        InventoryUIObj = FindObjectOfType<InvenUI>().gameObject;
        LabotoryUIObj = FindObjectOfType<LabotoryUI>().gameObject;

        LoadingUI = FindObjectOfType<LoadingUI>().gameObject;

        OptionInterface = FindObjectOfType<OptionPanel>().gameObject;

        
    }

    private void UIInit()
    {
        HeaderUIObj.SetActive(true);
        MainUIObj.SetActive(true);
        PlanetUIObj.SetActive(false);
        StageUIObj.SetActive(false);
        ReadyUIObj.SetActive(false);
        StoreUIObj.SetActive(false);
        InventoryUIObj.SetActive(false);
        LabotoryUIObj.SetActive(false);
        LoadingUI.SetActive(false);
        OptionInterface.SetActive(false);
    }
}
