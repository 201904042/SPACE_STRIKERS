using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance;

    //UIs
    private Transform mainUIs;

    public GameObject HeaderUIObj;
    public MainUI mainUI;
    public PlanetUI planetUI;
    public StageUI stageUI;
    public ReadyUI readyUI;
    public StoreUI storeUI;
    public InvenUI invenUI;
    public LabotoryUI labotoryUI;


    //Interface
    private Transform interfaceUIs;
    public static AlertInterface alertInterface;
    public static TFInterface tfInterface;

    public static OptionInterface optionInteface;
    public static StageInterface stageInteface;
    public static SelectCharInterface selectCharInterface;
    public static SelectPartsInterface selectPartsInterface;
    public static GotchaInterface gotchaInterface;
    public static PurchaseInterface purchaseInterface;
    public static ItemInformInterface itemInformInterface;

    //Prefab
    public GameObject itemAmountPref;

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

        mainUIs = GameObject.Find("MainUIs").transform;
        mainUI = mainUIs.GetComponentInChildren<MainUI>();
        planetUI = mainUIs.GetComponentInChildren<PlanetUI>();
        stageUI = mainUIs.GetComponentInChildren<StageUI>();
        readyUI = mainUIs.GetComponentInChildren<ReadyUI>();
        storeUI = mainUIs.GetComponentInChildren<StoreUI>();
        invenUI = mainUIs.GetComponentInChildren<InvenUI>();
        labotoryUI = mainUIs.GetComponentInChildren<LabotoryUI>();

        interfaceUIs = GameObject.Find("InterfaceUIs").transform;
        alertInterface = interfaceUIs.GetComponentInChildren<AlertInterface>();
        tfInterface = interfaceUIs.GetComponentInChildren<TFInterface>();
        optionInteface = interfaceUIs.GetComponentInChildren<OptionInterface>();
        stageInteface = interfaceUIs.GetComponentInChildren<StageInterface>();
        selectCharInterface = interfaceUIs.GetComponentInChildren<SelectCharInterface>();
        selectPartsInterface = interfaceUIs.GetComponentInChildren<SelectPartsInterface>();
        gotchaInterface = interfaceUIs.GetComponentInChildren<GotchaInterface>();
        purchaseInterface = interfaceUIs.GetComponentInChildren<PurchaseInterface>();
        itemInformInterface = interfaceUIs.GetComponentInChildren<ItemInformInterface>();
    }

    private void UIInit()
    {
        HeaderUIObj.SetActive(true);

        for (int i = 0; i < mainUIs.childCount; i++)
        {
            GameObject targetUI = mainUIs.GetChild(i).gameObject;
            if (!targetUI.GetComponent<MainUIs>().isComponentSet)
            {
                targetUI.GetComponent<MainUIs>().SetComponent();
            }
            targetUI.SetActive(false);
        }
        mainUI.gameObject.SetActive(true);
        
        for (int i = 0; i < interfaceUIs.childCount; i++) {
            GameObject targetInterface = interfaceUIs.GetChild(i).gameObject;
            if (!targetInterface.GetComponent<UIInterface>().isComponentSet)
            {
                targetInterface.GetComponent<UIInterface>().SetComponent();
            }
            
            targetInterface.SetActive(false);
        }
    }
}
