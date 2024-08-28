using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartsInterface : MonoBehaviour
{
    public GameObject partsUI;
    public int curPartsIndex; //현재 적용될 파츠의 칸

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
    } //인터페이스에서 선택된 파츠
    private OwnPartsData selectedParts;

    public List<OwnPartsData> invenPartsList;

    private void Awake()
    {
        //해당 인터페이스의 컴포넌트 추가
        ParentUI = transform.parent.parent.GetComponent<ReadyUI>();

        partsContainer = transform.GetChild(1);
        buttons = transform.GetChild(2);

        prevPageBtn = buttons.GetChild(0).GetChild(0).GetComponent<Button>();
        nextPageBtn = buttons.GetChild(0).GetChild(1).GetComponent<Button>();
        pageText = buttons.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>(); // 1/1 : 현재페이지 / 최대페이지

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
        //버튼에 리스터 부착
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
            Destroy(child.gameObject);  //이전에 연적이 있을경우 컨테이너를 비우고 새로 장착
        }

        //데이터로부터 인벤토리에 있는 파츠들의 데이터들을 가져옴
        invenPartsList = new List<OwnPartsData>();

        //인벤토리 데이터에서 최소한 파츠의 랭크와 종류를 알아내기


        //새로운 파츠UI를 만들고 인벤토리에서 보유한 파츠의 데이터를 삽입
    }

    public void PrevPage()
    {
        //페이지를 이전페이지로
        if(curPageIndex == 1)
        {
            return;
        }
        curPageIndex = curPageIndex - 1;
        PageTextSet();
    }

    public void NextPage()
    {
        //페이지를 다음 페이지로
        if (curPageIndex == maxPageIndex)
        {
            return;
        }
        curPageIndex = curPageIndex + 1;
        PageTextSet();
    }

    public void CloseInterface()
    {
        //해당 인터페이스 닫기
        ParentUI.PartsInterfaceOff();
    }

    public void SelectPartsCloseInterface(int partsIndex, OwnPartsData selectedParts)
    {
        if(partsIndex == 0 || selectedParts == null)
        {
            return;
        }
        //지정된 파츠를 readyUI에 적용
        ParentUI.PartsInterfaceOff(partsIndex, this.selectedParts);
    }

    
}
