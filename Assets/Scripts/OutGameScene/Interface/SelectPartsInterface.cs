using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartsInterface : UIInterface
{
    public GameObject partsUI; //���� ��ư�� UI

    public Transform partsContainer;
    public Transform buttons;

    public Button prevPageBtn;
    public Button nextPageBtn;
    public TextMeshProUGUI pageText;

    public Button backBtn;
    public Button selectBtn;

    public int curPageIndex;
    public int maxPageIndex;

    public PartsAbilityData SelectedParts {
        get => clickedParts;
        set
        {
            clickedParts = value;
            selectBtn.interactable = SelectedParts == null ? false : true;
        }
    } //�������̽����� ���õ� ����
    [SerializeField] private PartsAbilityData clickedParts;
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
        pageText = buttons.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>(); // 1/1 : ���������� / �ִ�������

        backBtn = buttons.GetChild(1).GetChild(0).GetComponent<Button>();
        selectBtn = buttons.GetChild(1).GetChild(1).GetComponent<Button>();
        
    }

    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        resetAll(); //�ʱ�ȭ �� ��ư ����

        selectBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(() => OnConfirm(true));
        backBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // �������̽� �����

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
        //��ư�� ������ ����
        prevPageBtn.onClick.RemoveAllListeners();
        nextPageBtn.onClick.RemoveAllListeners(); 

        prevPageBtn.onClick.AddListener(PrevPage);
        nextPageBtn.onClick.AddListener(NextPage);
    }

    private void SetPartsContainer()
    {
        if (partsContainer.childCount > 0) //�����̳ʸ� �ʱ�ȭ
        {
            foreach (Transform child in partsContainer.transform)
            {
                Destroy(child.gameObject);  
            }
        }

        //����db���� �ҷ�����
        List<PartsAbilityData> isOnPartsList = new List<PartsAbilityData>();
        List<PartsAbilityData> isOffPartsList = new List<PartsAbilityData>();
        foreach (PartsAbilityData parts in DataManager.parts.GetDictionary().Values)
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

        //�� ������ ���� ������
        ItemUIPref emptyPartsPrefab = Instantiate(partsUI, partsContainer.transform).GetComponent<ItemUIPref>();
        emptyPartsPrefab.SetByInvenId(-1);
        emptyPartsPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
        emptyPartsPrefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(emptyPartsPrefab));

        //�����Ǿ� �ִ� ������ ���� ����
        foreach (PartsAbilityData parts in isOnPartsList)
        {
            ItemUIPref prefab = Instantiate(partsUI, partsContainer.transform).GetComponent<ItemUIPref>();
            prefab.SetByInvenId(parts.invenId);
            prefab.GetComponent<Button>().onClick.RemoveAllListeners();
            prefab.GetComponent<Button>().onClick.AddListener(() => PartsButtonEvent(prefab));
        }

        //������ ������ ����
        foreach (PartsAbilityData parts in isOffPartsList)
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
}
