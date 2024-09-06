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


    //Interface
    public static GameObject OptionInterface;
    public static GameObject StageInterface;
    public static GameObject SelectCharInterface;
    public static GameObject SelectPartsInterface;
    public static GameObject GotchaInterface;

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


        OptionInterface = FindObjectOfType<OptionPanel>().gameObject;
        StageInterface = FindObjectOfType<StageInterface>().gameObject;
        SelectCharInterface = FindObjectOfType<SelectCharInterface>().gameObject;
        SelectPartsInterface = FindObjectOfType<SelectPartsInterface>().gameObject;
        GotchaInterface = FindObjectOfType<GotchaInterface>().gameObject;
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

        OptionInterface.SetActive(false);
        StageInterface.SetActive(false);
        SelectCharInterface.SetActive(false);
        SelectPartsInterface.SetActive(false);
        GotchaInterface.SetActive(false);
    }
}
