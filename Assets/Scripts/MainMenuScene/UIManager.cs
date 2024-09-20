using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public static AlertInterface AlertInterface;
    public static TFInterface TFInterface;

    public static OptionInterface OptionInterface;
    public static StageInterface StageInterface;
    public static SelectCharInterface SelectCharInterface;
    public static SelectPartsInterface SelectPartsInterface;
    public static GotchaInterface GotchaInterface;
    public static PurchaseInterface PurchaseInterface;
    public static ItemInformInterface IteminformInterface;
    
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

        AlertInterface = FindObjectOfType<AlertInterface>();
        TFInterface = FindObjectOfType<TFInterface>();
        OptionInterface = FindObjectOfType<OptionInterface>();
        StageInterface = FindObjectOfType<StageInterface>();
        SelectCharInterface = FindObjectOfType<SelectCharInterface>();
        SelectPartsInterface = FindObjectOfType<SelectPartsInterface>();
        GotchaInterface = FindObjectOfType<GotchaInterface>();
        PurchaseInterface = FindObjectOfType<PurchaseInterface>(); 
        IteminformInterface = FindObjectOfType<ItemInformInterface>();
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

        AlertInterface.gameObject.SetActive(false);
        TFInterface.gameObject.SetActive(false);
        OptionInterface.gameObject.SetActive(false);
        StageInterface.gameObject.SetActive(false);
        SelectCharInterface.gameObject.SetActive(false);
        SelectPartsInterface.gameObject.SetActive(false);
        GotchaInterface.gameObject.SetActive(false);
        PurchaseInterface.gameObject.SetActive(false);
        IteminformInterface.gameObject.SetActive(false);
    }
}
