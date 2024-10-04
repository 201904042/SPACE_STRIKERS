using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ReadyUI : MainUIs
{
    public Transform charZone;
    public Button charSlotBtn;
    public CharacterImageBtn charSlotUI;
    public TextMeshProUGUI charInformText;

    public Transform partsZone;
    public Button parts1Btn;
    public Button parts2Btn;
    public Button parts3Btn;
    public Button parts4Btn;

    public Transform itemZone;
    public Button item1;
    public Button item2;
    public Button item3;
    public Button item4;

    public Transform bottomZone;
    public TextMeshProUGUI stageText;
    public Button backBtn;
    public Button gotoIngameBtn;

    [SerializeField]private int curPlayerCode; 
    public int SetPlayerCode
    {
        get => curPlayerCode;
        set
        {
            curPlayerCode = value;
            PlayerPrefs.SetInt("curCharacterCode", value);
            PlayerChange();
        }
    }

    private List<PartsData> equippedPartsList = new List<PartsData>();
    public List<PartsData> EquippedPartsList => equippedPartsList;

    public PartsData SetPartsSlot1
    {
        get => parts1Btn.GetComponent<ItemUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts1Btn, value, "partsSlot1");
        }
    }

    public PartsData SetPartsSlot2
    {
        get => parts2Btn.GetComponent<ItemUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts2Btn, value, "partsSlot2");
        }
    }

    public PartsData SetPartsSlot3
    {
        get => parts3Btn.GetComponent<ItemUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts3Btn, value, "partsSlot3");
        }
    }

    public PartsData SetPartsSlot4
    {
        get => parts4Btn.GetComponent<ItemUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts4Btn, value, "partsSlot4");
        }
    }

    //아이템 파트는 아직 미구현
    private bool isItem1On;
    private bool isItem2On;
    private bool isItem3On;
    private bool isItem4On;

    
    public bool IsItem1On{
        get => isItem1On;
        set
        {
            isItem1On = value;
            ItemBtnChange(item1, value);
        }
    }
    public bool IsItem2On
    {
        get => isItem2On;
        set
        {
            isItem2On = value;
            ItemBtnChange(item2, value);
        }
    }
    public bool IsItem3On
    {
        get => isItem3On;
        set
        {
            isItem3On = value;
            ItemBtnChange(item3, value);
        }
    }
    public bool IsItem4On
    {
        get => isItem4On;
        set
        {
            isItem4On = value;
            ItemBtnChange(item4, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        charZone = transform.GetChild(0);
        charSlotBtn = charZone.GetChild(0).GetComponent<Button>();
        charSlotUI = charSlotBtn.GetComponent<CharacterImageBtn>();
        charInformText = charZone.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        partsZone = transform.GetChild(1);
        parts1Btn = partsZone.GetChild(0).GetComponent<Button>();
        parts2Btn = partsZone.GetChild(1).GetComponent<Button>();
        parts3Btn = partsZone.GetChild(2).GetComponent<Button>();
        parts4Btn = partsZone.GetChild(3).GetComponent<Button>();
        itemZone = transform.GetChild(2).GetChild(1);
        item1 = itemZone.GetChild(0).GetComponent<Button>();
        item2 = itemZone.GetChild(1).GetComponent<Button>();
        item3 = itemZone.GetChild(2).GetComponent<Button>();
        item4 = itemZone.GetChild(3).GetComponent<Button>();
        bottomZone = transform.GetChild(3);
        stageText = bottomZone.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        backBtn = bottomZone.GetChild(1).GetChild(1).GetComponent<Button>();
        gotoIngameBtn = bottomZone.GetChild(1).GetChild(0).GetComponent<Button>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetInit();
    }

    private void SetInit()
    {
        SetPlayerCode = PlayerPrefs.GetInt("curCharacterCode");

        SetPartsSlot1 = GetOwnPartsDataFromSlot("partsSlot1");
        SetPartsSlot2 = GetOwnPartsDataFromSlot("partsSlot2");
        SetPartsSlot3 = GetOwnPartsDataFromSlot("partsSlot3");
        SetPartsSlot4 = GetOwnPartsDataFromSlot("partsSlot4");

        PlayerStatTextSet();

        IsItem1On = false;
        IsItem2On = false;
        IsItem3On = false;
        IsItem4On = false;

        stageText.text = $"스테이지 : {PlayerPrefs.GetInt("ChosenPlanet")}-{PlayerPrefs.GetInt("ChosenStage")}";

        SetBtnListener();
    }

    private void SetBtnListener()
    {
        charSlotBtn.onClick.RemoveAllListeners();
        charSlotBtn.onClick.AddListener(GetCharacterId);
        for (int i = 0; i < partsZone.childCount; i++)
        {
            int index = i + 1;
            partsZone.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            partsZone.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GetPartsId(index));
        }
        for (int i = 0; i < itemZone.childCount; i++)
        {
            int index = i;
            itemZone.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            itemZone.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemOn(index));
        }


        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(GotoStage);
        gotoIngameBtn.onClick.RemoveAllListeners();
        gotoIngameBtn.onClick.AddListener(GameStart);

    }

    private void PlayerChange()
    {
        int playerMasterCode = PlayerPrefs.GetInt("curCharacterCode") + 100;
        charSlotUI.SetImageByMasterCode(playerMasterCode);

        PlayerStatTextSet();
    }

    private void PlayerStatTextSet()
    {
        int masterId = PlayerPrefs.GetInt("curCharacterCode") + 100;
        CharData targetBasicData = DataManager.character.GetData(masterId);

        CharData changedData = CalculateStat(targetBasicData);
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{changedData.name}");
        sb.AppendLine($"LEVEL : {changedData.level}");

        foreach(Ability charAbility in changedData.abilityDatas)
        {
            AbilityData targetAbility = DataManager.ability.GetData(charAbility.key);
            sb.AppendLine($"{targetAbility.name}: {charAbility.value}");
        }
        
        charInformText.text = sb.ToString();
    }

    /// <summary>
    /// 현재 플레이어가 장착한 파츠들로 스텟 증감
    /// </summary>
    private CharData CalculateStat(CharData targetBasicData)
    {
        CharData result = targetBasicData;

        //todo -> 여기서 파츠의 내용을 적용시키지 못하는 문제 발생
        foreach (var part in EquippedPartsList)
        {
            if (!part.isActive) continue;

            PartsDataReader.ApplyAbilityToCharacter(ref result, part.subAbility1);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.subAbility2);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.subAbility3);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.subAbility4);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.subAbility5);
        }
        return result;
    }
    
    //todo -> PartsData Reader에서 static변수로 만들기
    private PartsData GetOwnPartsDataFromSlot(string invenKey)
    {
        int invenId = PlayerPrefs.GetInt(invenKey, -1);

        if (invenId == -1)
        {
            return null;
        }

        PartsData data = DataManager.parts.GetData(DataManager.inven.GetData(invenId).masterId);
        if (data != null)
        {
            return data;
        }
        return null;
    }

    private void CheckDuplicateParts(int partsInvenCode)
    {
        if (parts1Btn.GetComponent<PartsSlot>().partsData?.invenId == partsInvenCode)
        {
            SetPartsSlot1 = null;
        }
        if (parts2Btn.GetComponent<PartsSlot>().partsData?.invenId == partsInvenCode)
        {
            SetPartsSlot2 = null;
        }
        if (parts3Btn.GetComponent<PartsSlot>().partsData?.invenId == partsInvenCode)
        {
            SetPartsSlot3 = null;
        }
        if (parts4Btn.GetComponent<PartsSlot>().partsData?.invenId == partsInvenCode)
        {
            SetPartsSlot4 = null;
        }
    }

    /// <summary>
    /// 장착 슬롯에 해당 파츠를 등록 혹은 해제
    /// </summary>
    private void UpdatePartsSlot(Button partsButton, PartsData value, string slotKey)
    {
        PartsSlot partsUIPref = partsButton.GetComponent<PartsSlot>();

        //해당 장착칸에 이미 파츠가 있는 경우 해당 파츠를 제거
        if(partsUIPref.partsData != null)
        {
            partsUIPref.partsData.isActive = false;
            equippedPartsList.Remove(partsUIPref.partsData);
        }

        PartsData currentParts = partsUIPref.partsData;
        if (value != null) //해당 파츠 슬롯을 주어진 value값으로 채움
        {
            CheckDuplicateParts(value.invenId); //다른 슬롯에 해당 파츠가 설정되어 잇는지 체크. 만약 다른 슬롯에 있다면 그 슬롯의 파츠 해제
            partsUIPref.SetParts(value);
            PlayerPrefs.SetInt(slotKey, value.invenId);
            value.isActive = true;
            if (!equippedPartsList.Contains(value))
            {
                equippedPartsList.Add(value);
            }
        }
        else
        { //파츠데이터가 널일경우 -> 그 파츠슬롯을 빈상태로 만듬
            if (currentParts != null && equippedPartsList.Contains(currentParts))
            {
                currentParts.isActive = false;
                equippedPartsList.Remove(currentParts);
            }

            partsUIPref.ResetData();
            PlayerPrefs.SetInt(slotKey, -1);
        }
    }

    private void ItemBtnChange(Button item, bool value)
    {
        item.transform.GetChild(0).GetComponent<Image>().color = value ? Color.yellow : Color.white;
        //item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = quantity - 1;
    }

    private void ItemOn(int i)
    {
        switch (i)
        {
            case 0: IsItem1On = !IsItem1On ? true : false; break;
            case 1: IsItem2On = !IsItem2On ? true : false; break;
            case 2: IsItem3On = !IsItem3On ? true : false; break;
            case 3: IsItem4On = !IsItem4On ? true : false; break;
        }
    }

    public void GotoStage()
    {
        ChangeUI(UIManager.UIInstance.stageUI);
    }
    public void GameStart()
    {
        SceneManager.LoadScene("InGameTest");
    }
    private void GetCharacterId()
    {
        StartCoroutine(GetCharacterIdCoroutine());
    }

    private IEnumerator GetCharacterIdCoroutine()
    {
        SelectCharInterface selecteCharInterface = UIManager.selectCharInterface.GetComponent<SelectCharInterface>();
        
        yield return StartCoroutine(selecteCharInterface.GetValue());
        
        if(selecteCharInterface.result == true)
        {
            if (selecteCharInterface.SelectedCode != -1)
            {
                SetPlayerCode = selecteCharInterface.SelectedCode;
                Debug.Log(SetPlayerCode);
            }
        }
    }

    

    private void GetPartsId(int partsSlotIndex)
    {
        StartCoroutine(GetPartsIdCoroutine(partsSlotIndex));
    }

    private IEnumerator GetPartsIdCoroutine(int partsSlotIndex)
    {
        SelectPartsInterface selectPartsInterface = UIManager.selectPartsInterface;

        yield return StartCoroutine(selectPartsInterface.GetValue());

        if (selectPartsInterface.result == true)
        {
            PartsData parts = selectPartsInterface.SelectedParts;

            switch (partsSlotIndex)
            {
                case 1: SetPartsSlot1 = parts.invenId != -1 ? parts : null; break;
                case 2: SetPartsSlot2 = parts.invenId != -1 ? parts : null; ; break;
                case 3: SetPartsSlot3 = parts.invenId != -1 ? parts : null; ; break;
                case 4: SetPartsSlot4 = parts.invenId != -1 ? parts : null; ; break;
            }

            PlayerStatTextSet();

        }

    }
}
