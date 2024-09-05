using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ReadyUI : MainUIs
{
    public Transform charZone;
    public Button charBtn;
    public Image curPlayerImage;
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

    private int curPlayerCode; 
    public int CurPlayerCode
    {
        get => curPlayerCode;
        set
        {
            curPlayerCode = value;
            PlayerPrefs.SetInt("curCharacterCode", value);
            PlayerChange();
        }
    }

    private List<OwnPartsData> equippedPartsList = new List<OwnPartsData>();
    public List<OwnPartsData> EquippedPartsList => equippedPartsList;

    public OwnPartsData SetPartsSlot1
    {
        get => parts1Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts1Btn, value, "partsSlot1");
        }
    }

    public OwnPartsData SetPartsSlot2
    {
        get => parts2Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts2Btn, value, "partsSlot2");
        }
    }

    public OwnPartsData SetPartsSlot3
    {
        get => parts3Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts3Btn, value, "partsSlot3");
        }
    }

    public OwnPartsData SetPartsSlot4
    {
        get => parts4Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            UpdatePartsSlot(parts4Btn, value, "partsSlot4");
        }
    }


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

    

    private void Awake()
    {
        charZone = transform.GetChild(0);
        charBtn = charZone.GetChild(0).GetComponent<Button>();
        curPlayerImage = charBtn.transform.GetChild(0).GetComponent<Image>();
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
        CurPlayerCode = PlayerPrefs.GetInt("curCharacterCode");

        SetPartsSlot1 = GetOwnPartsDataFromSlot("partsSlot1");
        SetPartsSlot2 = GetOwnPartsDataFromSlot("partsSlot2");
        SetPartsSlot3 = GetOwnPartsDataFromSlot("partsSlot3");
        SetPartsSlot4 = GetOwnPartsDataFromSlot("partsSlot4");


        IsItem1On = false;
        IsItem2On = false;
        IsItem3On = false;
        IsItem4On = false;

        stageText.text = $"스테이지 : {PlayerPrefs.GetInt("ChosenPlanet")}-{PlayerPrefs.GetInt("ChosenStage")}";

        SetBtnListener();
    }

    private void SetBtnListener()
    {
        charBtn.onClick.RemoveAllListeners();
        charBtn.onClick.AddListener(SelectCharInterfaceOn);
        for (int i = 0; i < partsZone.childCount; i++)
        {
            int index = i + 1;
            partsZone.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            partsZone.GetChild(i).GetComponent<Button>().onClick.AddListener(() => PartsInterfaceOn(index));
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

        MasterItem masterChar = new MasterItem();
        DataManager.masterData.masterItemDic.TryGetValue(playerMasterCode, out masterChar);

        Character selectedChar = new Character();
        DataManager.characterData.characterDic.TryGetValue(playerMasterCode, out selectedChar);
        curPlayerImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(masterChar.spritePath);

        PlayerStatTextSet();
    }

    private void PlayerStatTextSet()
    {
        int masterId = PlayerPrefs.GetInt("curCharacterCode") + 100;
        Character targetBasicData = new Character();
        bool success = DataManager.characterData.characterDic.TryGetValue(masterId, out targetBasicData);
        if (!success)
        {
            charInformText.text = "error";
            return;
        }

        Character changedData = CalculateStat(targetBasicData);
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{changedData.name}");
        sb.AppendLine($"LEVEL : {changedData.level}");

        sb.AppendLine($"DMG: {changedData.damage}");
        sb.AppendLine($"DEF: {changedData.defense}");
        sb.AppendLine($"ASPD: {changedData.attackSpeed}");
        sb.AppendLine($"MSPD: {changedData.movementSpeed}");
        sb.AppendLine($"HP: {changedData.maxHealth}");

        if (changedData.hpRegen != 0) sb.AppendLine($"초당회복 : {changedData.hpRegen}");
        if (changedData.troopsDamageUp != 0) sb.AppendLine($"잡몹 뎀증: {changedData.troopsDamageUp}");
        if (changedData.bossDamageUp != 0) sb.AppendLine($"보스 뎀증: {changedData.bossDamageUp}");
        if (changedData.stageExpRateUp != 0) sb.AppendLine($"스테이지 경험치 증가: {changedData.stageExpRateUp}");
        if (changedData.stageItemDropRateUp != 0) sb.AppendLine($"스테이지 아이템 증가: {changedData.stageItemDropRateUp}");
        if (changedData.powRegenRateUp != 0) sb.AppendLine($"파워 리젠 증가: {changedData.powRegenRateUp}");
        if (changedData.powAmountUp != 0) sb.AppendLine($"파워 개수 증가 : {changedData.powAmountUp}");
        if (changedData.accountExpUp != 0) sb.AppendLine($"계정 경험치 증가: {changedData.accountExpUp}");
        if (changedData.accountMoneyUp != 0) sb.AppendLine($"계정 획득재화 증가: {changedData.accountMoneyUp}");
        if (changedData.startLevelUp != 0) sb.AppendLine($"시작 레벨 증가: {changedData.startLevelUp}");
        if (changedData.revival != 0) sb.AppendLine($"부활 횟수 : {changedData.revival}");
        if (targetBasicData.startWeaponUp != 0) sb.AppendLine($"시작 무기레벨 증가: {targetBasicData.startWeaponUp}");

        charInformText.text = sb.ToString();
    }

    private Character CalculateStat(Character targetBasicData)
    {
        Character result = targetBasicData;

        //todo -> 여기서 파츠의 내용을 적용시키지 못하는 문제 발생
        foreach (var part in EquippedPartsList)
        {
            if (!part.isOn) continue;

            PartsDataReader.ApplyAbilityToCharacter(ref result, part.ability1);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.ability2);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.ability3);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.ability4);
            PartsDataReader.ApplyAbilityToCharacter(ref result, part.ability5);
        }

        return result;
    }
    
    private OwnPartsData GetOwnPartsDataFromSlot(string invenKey)
    {
        int invenId = PlayerPrefs.GetInt(invenKey, -1);

        if (invenId == -1)
        {
            return null;
        }

        InventoryItem invenData;
        if (DataManager.inventoryData.InvenItemDic.TryGetValue(invenId, out invenData))
        {
            OwnPartsData data;
            if (DataManager.partsData.ownPartsDic.TryGetValue(invenData.masterId, out data))
            {
                return data;
            }
            else
            {
                Debug.LogWarning($"MasterId '{invenData.masterId}'의 파츠를 찾지못함 .");
            }
        }
        else
        {
            Debug.LogWarning($"Slot value '{invenId}'의 인벤토리를 찾지 못함.");
        }
        return null;
    }

    private void CheckDuplicateParts(int partsInvenCode)
    {
        if (parts1Btn.GetComponent<PartsUIPref>().partsData?.inventoryCode == partsInvenCode)
        {
            SetPartsSlot1 = null;
        }
        if (parts2Btn.GetComponent<PartsUIPref>().partsData?.inventoryCode == partsInvenCode)
        {
            SetPartsSlot2 = null;
        }
        if (parts3Btn.GetComponent<PartsUIPref>().partsData?.inventoryCode == partsInvenCode)
        {
            SetPartsSlot3 = null;
        }
        if (parts4Btn.GetComponent<PartsUIPref>().partsData?.inventoryCode == partsInvenCode)
        {
            SetPartsSlot4 = null;
        }
    }

    private void UpdatePartsSlot(Button partsButton, OwnPartsData value, string slotKey)
    {
        var partsUIPref = partsButton.GetComponent<PartsUIPref>();
        var currentParts = partsUIPref.partsData;

        if (value != null)
        {
            CheckDuplicateParts(value.inventoryCode);
            partsUIPref.SetParts(value);
            PlayerPrefs.SetInt(slotKey, value.inventoryCode);

            if (!equippedPartsList.Contains(value))
            {
                equippedPartsList.Add(value);
            }
        }
        else
        {
            if (currentParts != null && equippedPartsList.Contains(currentParts))
            {
                equippedPartsList.Remove(currentParts);
            }

            partsUIPref.ResetData();
            PlayerPrefs.SetInt(slotKey, -1);
        }
    }


    private void ItemBtnChange(Button item, bool value)
    {
        item.transform.GetChild(0).GetComponent<Image>().color = value ? Color.yellow : Color.white;
        //item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amount - 1;
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
        ChangeUI(UIManager.UIInstance.StageUIObj);
    }
    public void GameStart()
    {
        SceneManager.LoadScene("InGameTest");
    }

    public void PartsInterfaceOn(int partsIndex)
    {
        OpenInterface(UIManager.UIInstance.SelectPartsInterface);
        Debug.Log(partsIndex);
        UIManager.UIInstance.SelectPartsInterface.GetComponent<SelectPartsInterface>().curPartsIndex = partsIndex;
    }

    public void PartsInterfaceOff()
    {
        CloseInterface(UIManager.UIInstance.SelectPartsInterface);
    }

    public void GetPartsData(int slotCode, OwnPartsData parts)
    {
        switch (slotCode)
        {
            case 1: SetPartsSlot1 = parts.inventoryCode != -1 ? parts: null; break;
            case 2: SetPartsSlot2 = parts.inventoryCode != -1 ? parts : null; ; break;
            case 3: SetPartsSlot3 = parts.inventoryCode != -1 ? parts : null; ; break;
            case 4: SetPartsSlot4 = parts.inventoryCode != -1 ? parts : null; ; break;
        }
        PlayerStatTextSet();
    }

    public void SelectCharInterfaceOn()
    {
        OpenInterface(UIManager.UIInstance.SelectCharInterface);
    }

    public void SelectCharInterfaceOff()
    {
        CloseInterface(UIManager.UIInstance.SelectCharInterface);
    }
}
