using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartsInterface : UIInterface
{
    public GameObject partsUI; //파츠 버튼의 UI

    public Transform partsContainer;
    public Transform buttons;

    public Button prevPageBtn;
    public Button nextPageBtn;
    public TextMeshProUGUI pageText;

    public Button backBtn;
    public Button selectBtn;

    public int curPageIndex;
    public int maxPageIndex;

    public PartsData SelectedParts {
        get => clickedParts;
        set
        {
            clickedParts = value;
            selectBtn.interactable = SelectedParts == null ? false : true;
        }
    } //인터페이스에서 선택된 파츠
    [SerializeField] private PartsData clickedParts;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void SetComponent()
    {
        base.SetComponent();
        partsContainer = transform.GetChild(1);
        buttons = transform.GetChild(2);

        prevPageBtn = buttons.GetChild(0).GetChild(0).GetComponent<Button>();
        nextPageBtn = buttons.GetChild(0).GetChild(1).GetComponent<Button>();
        pageText = buttons.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>(); // 1/1 : 현재페이지 / 최대페이지

        backBtn = buttons.GetChild(1).GetChild(0).GetComponent<Button>();
        selectBtn = buttons.GetChild(1).GetChild(1).GetComponent<Button>();
        
    }

    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        resetAll(); //초기화 및 버튼 셋팅

        selectBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(() => OnConfirm(true));
        backBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // 인터페이스 숨기기

        yield return SelectedParts;
    }

    private void resetAll()
    {
        SetBtnsListener();
        SetPartsContainer();

        
        PageTextSet();
        SelectedParts = null;
    }


    private void PageTextSet()
    {
        prevPageBtn.interactable = curPageIndex == 1 ? false : true;
        nextPageBtn.interactable = curPageIndex == maxPageIndex ? false : true;
        pageText.text = $"{curPageIndex} / {maxPageIndex}";
    }

    private void SetBtnsListener()
    {
        //버튼에 리스터 부착
        prevPageBtn.onClick.RemoveAllListeners();
        nextPageBtn.onClick.RemoveAllListeners(); 

        prevPageBtn.onClick.AddListener(PrevPage);
        nextPageBtn.onClick.AddListener(NextPage);
    }

    private void SetPartsContainer()
    {
        if (partsContainer.childCount > 0) //컨테이너를 초기화
        {
            foreach (Transform child in partsContainer.transform)
            {
                Destroy(child.gameObject);  
            }
        }

        //파츠db에서 불러오기
        List<PartsData> isOnPartsList = new List<PartsData>();
        List<PartsData> isOffPartsList = new List<PartsData>();
        foreach (PartsData parts in DataManager.parts.GetDictionary().Values)
        {
            if (parts.isActive)
            {
                isOnPartsList.Add(parts);
            }
            else
            {
                isOffPartsList.Add(parts);
            }
        }

        curPageIndex = 1;
        maxPageIndex = ((isOnPartsList.Count+ isOffPartsList.Count) / 16) + 1;


        isOnPartsList.Sort((part1, part2) => part2.rank.CompareTo(part1.rank));
        isOffPartsList.Sort((part1, part2) => part2.rank.CompareTo(part1.rank));

        //빈 파츠를 담을 프리팹
        ItemUIPref emptyPartsPrefab = Instantiate(partsUI, partsContainer.transform).GetComponent<ItemUIPref>();
        emptyPartsPrefab.SetByInvenId(0);
        emptyPartsPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
        emptyPartsPrefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(emptyPartsPrefab));

        //장착되어 있는 파츠들 먼저 나열
        foreach (PartsData parts in isOnPartsList)
        {
            ItemUIPref prefab = Instantiate(partsUI, partsContainer.transform).GetComponent<ItemUIPref>();
            prefab.SetByInvenId(parts.invenId);
            prefab.GetComponent<Button>().onClick.RemoveAllListeners();
            prefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(prefab));
        }

        //나머지 파츠들 나열
        foreach (PartsData parts in isOffPartsList)
        {
            ItemUIPref prefab = Instantiate(partsUI, partsContainer.transform).GetComponent<ItemUIPref>();
            prefab.SetByInvenId(parts.invenId);
            prefab.GetComponent<Button>().onClick.RemoveAllListeners();
            prefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(prefab));
        }
    }

    public void PartsButtonEvent(ItemUIPref partsBtn)
    {
        SelectedParts = partsBtn.PartsAbilityData;
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
}
