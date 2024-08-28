using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartsInterface : MonoBehaviour
{
    public GameObject partsUI;
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

    public OwnPartsData SelectedParts{
        get => selectedParts;
        set
        {
            selectedParts = value;
            selectBtn.interactable = SelectedParts == null ? false : true;
        }
    } //�������̽����� ���õ� ����
    private OwnPartsData selectedParts;

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
   
        foreach (Transform child in partsContainer.transform)
        {
            Destroy(child.gameObject);  //������ ������ ������� �����̳ʸ� ���� ���� ����
        }

        //�����ͷκ��� �κ��丮�� �ִ� �������� �����͵��� ������
        invenPartsList = new List<OwnPartsData>();

        //�κ��丮 �����Ϳ��� �ּ��� ������ ��ũ�� ������ �˾Ƴ���


        //���ο� ����UI�� ����� �κ��丮���� ������ ������ �����͸� ����
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
        //������ ������ readyUI�� ����
        ParentUI.PartsInterfaceOff(partsIndex, this.selectedParts);
    }

    
}
