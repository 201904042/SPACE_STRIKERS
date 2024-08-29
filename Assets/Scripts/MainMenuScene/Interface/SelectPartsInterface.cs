using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartsInterface : MonoBehaviour
{
    public GameObject partsUI; //���� ��ư�� UI
    public int curPartsIndex; //���� ����� ������ ĭ

    public ReadyUI ParentUI;
    public Transform partsContainer;
    public Transform buttons;

    public Button prevPageBtn;
    public Button nextPageBtn;
    public TextMeshProUGUI pageText;

    public Button backBtn;
    public Button selectBtn;

    public int curPageIndex;
    public int maxPageIndex;

    public OwnPartsData SelectedParts {
        get => selectedParts;
        set
        {
            selectedParts = value;
            selectBtn.interactable = SelectedParts == null ? false : true;
        }
    } //�������̽����� ���õ� ����
    [SerializeField] private OwnPartsData selectedParts;

    public List<OwnPartsData> invenPartsList;

    private void Awake()
    {
        //�ش� �������̽��� ������Ʈ �߰�
        ParentUI = transform.parent.parent.GetComponent<ReadyUI>();

        partsContainer = transform.GetChild(1);
        buttons = transform.GetChild(2);

        prevPageBtn = buttons.GetChild(0).GetChild(0).GetComponent<Button>();
        nextPageBtn = buttons.GetChild(0).GetChild(1).GetComponent<Button>();
        pageText = buttons.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>(); // 1/1 : ���������� / �ִ�������

        backBtn = buttons.GetChild(1).GetChild(0).GetComponent<Button>();
        selectBtn = buttons.GetChild(1).GetChild(1).GetComponent<Button>();
        curPartsIndex = 0;
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        SetButtonListener();
        SetPartsContainer();

        invenPartsList = new List<OwnPartsData>();
        foreach (OwnPartsData parts in DataManager.partsData.ownPartsDic.Values)
        {
            invenPartsList.Add(parts);
        }

        curPageIndex = 1;
        maxPageIndex = (invenPartsList.Count / 16) + 1;
        PageTextSet();
        SelectedParts = null;
    }

    private void PageTextSet()
    {
        prevPageBtn.interactable = curPageIndex == 1 ? false : true;
        nextPageBtn.interactable = curPageIndex == maxPageIndex ? false : true;
        pageText.text = $"{curPageIndex} / {maxPageIndex}";
    }

    private void SetButtonListener()
    {
        //��ư�� ������ ����
        prevPageBtn.onClick.RemoveAllListeners();
        nextPageBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.RemoveAllListeners();

        prevPageBtn.onClick.AddListener(PrevPage);
        nextPageBtn.onClick.AddListener(NextPage);
        backBtn.onClick.AddListener(CloseInterface);
        selectBtn.onClick.AddListener(()=> SelectPartsCloseInterface(curPartsIndex, SelectedParts));
    }

    private void SetPartsContainer()
    {
        if (partsContainer.childCount > 0) //������ ������ ���� ��� �����̳ʸ� ���� ���� ����
        {
            foreach (Transform child in partsContainer.transform)
            {
                Destroy(child.gameObject);  
            }
        }
        

        //�����ͷκ��� �κ��丮�� �ִ� �������� �����͵��� ������
        List<OwnPartsData> isOnPartsList = new List<OwnPartsData>();
        List<OwnPartsData> isOffPartsList = new List<OwnPartsData>();

        foreach (OwnPartsData parts in invenPartsList)
        {
            if (parts.isOn)
            {
                isOnPartsList.Add(parts);
            }
            else 
            {
                isOffPartsList.Add(parts);
            }
        }

        isOnPartsList.Sort((part1, part2) => part2.grade.CompareTo(part1.grade));
        isOffPartsList.Sort((part1, part2) => part2.grade.CompareTo(part1.grade));

        //�� ���� ����
        OwnPartsData emptyParts = new OwnPartsData();
        emptyParts.inventoryCode = -1;
        PartsUIPref emptyPartsPrefab = Instantiate(partsUI, partsContainer.transform).GetComponent<PartsUIPref>();
        emptyPartsPrefab.SetParts(emptyParts);
        emptyPartsPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
        emptyPartsPrefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(emptyPartsPrefab));

        foreach (OwnPartsData parts in isOnPartsList)
        {
            PartsUIPref prefab = Instantiate(partsUI, partsContainer.transform).GetComponent<PartsUIPref>();
            prefab.SetParts(parts);
            prefab.GetComponent<Button>().onClick.RemoveAllListeners();
            prefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(prefab));
        }
        foreach (OwnPartsData parts in isOffPartsList)
        {
            PartsUIPref prefab = Instantiate(partsUI, partsContainer.transform).GetComponent<PartsUIPref>();
            prefab.SetParts(parts);
            prefab.GetComponent<Button>().onClick.RemoveAllListeners();
            prefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(prefab));
        }
    }

    public void PartsButtonEvent(PartsUIPref partsBtn)
    {
        SelectedParts = partsBtn.partsData;
    }

    public void PrevPage()
    {
        //�������� ������������
        if(curPageIndex == 1)
        {
            return;
        }
        curPageIndex = curPageIndex - 1;
        PageTextSet();
    }

    public void NextPage()
    {
        //�������� ���� ��������
        if (curPageIndex == maxPageIndex)
        {
            return;
        }
        curPageIndex = curPageIndex + 1;
        PageTextSet();
    }

    public void CloseInterface()
    {
        //�ش� �������̽� �ݱ�
        ParentUI.PartsInterfaceOff();
    }

    public void SelectPartsCloseInterface(int partsIndex, OwnPartsData selectedParts)
    {
        if(partsIndex == 0 || selectedParts == null)
        {
            return;
        }

        ParentUI.GetPartsData(partsIndex, selectedParts);
        ParentUI.PartsInterfaceOff();
    }

    
}
