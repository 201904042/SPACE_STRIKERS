using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LabotoryUI : MainUIs
{
    public Transform changeModeBtns;
    public Button charModeBtn;
    public Button partModeBtn;

    public Transform targetSlots;
    public Button charSlot;
    public Button partsSlot;

    public Transform informSlots;
    public Transform ingredientSlot;
    public TextMeshProUGUI upgradeInformText;

    public Transform buttons;
    public Button upgradeBtn;
    public Button mainBtn;
    public Button storeBtn;
    

    public int targetInvenCode; //강화하려는 캐릭터 혹은 파츠의 인벤토리 아이디
    public MasterType targetType;
    List<UpgradeIngred> ingredientList = new List<UpgradeIngred>(); //강화에 드는 재료 (key , amount)
    List<Ability> valueList = new List<Ability>(); //강화할시 추가될 값 (key , value)

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();

        InitializeUIComponents();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ResetUI();
        SetupButtons();
        ActivateCharacterMode();
    }

    //@UI 구성

    //컴포넌트 설정
    private void InitializeUIComponents()
    {
        changeModeBtns = transform.GetChild(0);
        charModeBtn = changeModeBtns.GetChild(0).GetComponent<Button>();
        partModeBtn = changeModeBtns.GetChild(1).GetComponent<Button>();

        targetSlots = transform.GetChild(1);
        charSlot = targetSlots.GetChild(0).GetComponent<Button>();
        partsSlot = targetSlots.GetChild(1).GetComponent<Button>();

        informSlots = transform.GetChild(2);
        ingredientSlot = informSlots.GetChild(0).GetComponentInChildren<ScrollRect>().content;
        upgradeInformText = informSlots.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        buttons = transform.GetChild(3);
        upgradeBtn = buttons.GetChild(0).GetComponent<Button>();
        mainBtn = buttons.GetChild(1).GetComponent<Button>();
        storeBtn = buttons.GetChild(2).GetComponent<Button>();
    }

    //UI 초기화
    private void ResetUI()
    {
        targetInvenCode = 0;
        targetType = MasterType.None;
        upgradeBtn.interactable = false;
        charSlot.GetComponent<CharacterImageBtn>().ResetData();
        partsSlot.GetComponent<PartsSlot>().ResetData();
        ingredientList.Clear();
        valueList.Clear();
        ClearChildObjects(ingredientSlot);
        upgradeInformText.text = "";
    }

    //자식객체 삭제
    private void ClearChildObjects(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    //버튼 핸들러 설정
    private void SetupButtons()
    {
        RemoveAllButtonListeners();
        charModeBtn.onClick.AddListener(ActivateCharacterMode);
        partModeBtn.onClick.AddListener(ActivatePartsMode);

        charSlot.onClick.AddListener(GetCharId);
        partsSlot.onClick.AddListener(GetPartsId);

        upgradeBtn.onClick.AddListener(UpgradeBtn);
        mainBtn.onClick.AddListener(() => ChangeUI(OG_UIManager.UIInstance.mainUI));
        storeBtn.onClick.AddListener(() => ChangeUI(OG_UIManager.UIInstance.stageUI));
    }

    //모든 버튼의 리스터 제거
    private void RemoveAllButtonListeners()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.onClick.RemoveAllListeners();
        }
    }

    //캐릭터 강화 모드 버튼 클릭
    private void ActivateCharacterMode()
    {
        ToggleMode(true);
    }

    //파츠 강화 모드 버튼 클릭
    private void ActivatePartsMode()
    {
        ToggleMode(false);
    }

    //현재 모드에 따른 활성화
    private void ToggleMode(bool isCharacterMode)
    {
        charSlot.gameObject.SetActive(isCharacterMode);
        partsSlot.gameObject.SetActive(!isCharacterMode);
        ResetUI();
    }

    //@강화 대상 선택

    //캐릭터 선택 과정
    private void GetCharId()
    {
        ResetUI();
        StartCoroutine(GetCharIdCoroutine());
    }

    private IEnumerator GetCharIdCoroutine()
    {
        SelectCharInterface selecteCharInterface = OG_UIManager.selectCharInterface.GetComponent<SelectCharInterface>();

        yield return StartCoroutine(selecteCharInterface.GetValue());
        if(selecteCharInterface.result == true)
        {
            targetType = MasterType.Character;
            SetUpgradeUIForCharacter(selecteCharInterface.SelectedCode);
        }
    }

    private void SetUpgradeUIForCharacter(int masterId)
    {
        charSlot.GetComponent<CharacterImageBtn>().SetImageByMasterCode(masterId);
        CharData charData = DataManager.character.GetData(masterId);
        int curLevel = charData.level;

        UpgradeData upgradeData = DataManager.upgrade.GetData(masterId);

        if (!CanUpgrade(upgradeData, curLevel))
        {
            upgradeInformText.text = "강화불가";
            return;
        }

        //훼손 방지를위한 new 생성
        ingredientList = new List<UpgradeIngred>(upgradeData.upgradeCost[curLevel].ingredients);
        valueList = new List<Ability>(upgradeData.upgradeCost[curLevel].upgradeValues); // 추가될 데이터

        List<Ability> beforeAbility = Ability.CopyList(charData.abilityDatas); // 원래 데이터
        List<Ability> resultAbility = SumAbility(beforeAbility, valueList); // 합쳐서 나온 결과 데이터

        foreach (UpgradeIngred cost in ingredientList)
        {
            // 재료 UI들 생성
            ItemAmountPref itemAmountPref = Instantiate(OG_UIManager.UIInstance.ItemAmountUI, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
        }
        upgradeInformText.text = MakeSBText(curLevel, resultAbility, beforeAbility);
        upgradeBtn.interactable = true;
    }

    //파츠 선택 과정
    private void GetPartsId()
    {
        ResetUI();
        StartCoroutine(GetPartsIdCoroutine());
    }

    private IEnumerator GetPartsIdCoroutine()
    {
        SelectPartsInterface selectPartsInterface = OG_UIManager.selectPartsInterface.GetComponent<SelectPartsInterface>();

        yield return StartCoroutine(selectPartsInterface.GetValue());
        if(selectPartsInterface.result == true)
        {
            targetInvenCode = selectPartsInterface.SelectedParts.invenId;
            targetType = MasterType.Parts;
            SetUpgradeUIForParts(targetInvenCode);
        }
    }

    private void SetUpgradeUIForParts(int invenCode)
    {
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        PartsData PartsAbilityData = DataManager.parts.GetData(invenCode);
        int curLevel = PartsAbilityData.level;

        partsSlot.GetComponent<PartsSlot>().SetParts(invenCode);
        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (!CanUpgrade(upgradeData, curLevel))
        {
            upgradeInformText.text = "강화불가";
            return;
        }

        SetUpgradeIngredients(upgradeData.upgradeCost[curLevel].ingredients);
        UpdateUpgradeUIForParts(PartsAbilityData, curLevel);
        upgradeBtn.interactable = true;
    }

    private bool CanUpgrade(UpgradeData upgradeData, int curLevel)
    {
        return upgradeData.upgradeCost[curLevel].ingredients.Length > 0;
    }

    private void SetUpgradeIngredients(UpgradeIngred[] ingredients)
    {
        ingredientList = new List<UpgradeIngred>(ingredients);
        ClearChildObjects(ingredientSlot);

        foreach (var cost in ingredientList)
        {
            var itemAmountPref = Instantiate(OG_UIManager.UIInstance.ItemAmountUI, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
        }
    }

    private void UpdateUpgradeUIForParts(PartsData PartsAbilityData, int curLevel)
    {
        var mainAbility = new Ability(PartsAbilityData.mainAbility);
        var beforeAbilities = new List<Ability> { mainAbility };

        var changedMainAbility = new Ability(mainAbility) { value = mainAbility.value + 5 };
        var resultAbilities = new List<Ability> { changedMainAbility };

        upgradeInformText.text = CreateUpgradeInfoText(curLevel, resultAbilities, beforeAbilities);
    }

    private string CreateUpgradeInfoText(int level, List<Ability> newAbilities, List<Ability> beforeAbilities)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"LEVEL : {level} -> {level + 1}");
        foreach (var ability in newAbilities)
        {
            var beforeAbility = beforeAbilities.Find(a => a.key == ability.key);
            var data = DataManager.ability.GetData(ability.key);
            sb.AppendLine($"{data.name} : {beforeAbility?.value ?? 0} -> {ability.value}");
        }
        return sb.ToString();
    }

    //이전과 더할 어벌리티 리스트를 합쳐 result를 리턴함
    private List<Ability> SumAbility(List<Ability> beforeList, List<Ability> addList)
    {
        var result = Ability.CopyList(beforeList);

        foreach (var ability in addList)
        {
            var existingAbility = result.Find(a => a.key == ability.key);
            if (existingAbility != null)
            {
                existingAbility.value += ability.value;
            }
            else
            {
                result.Add(new Ability(ability));
            }
        }

        return result;
    }

    //result로 하여금 스트링빌더로 정보를 구성
    private string MakeSBText(int level, List<Ability> resultList, List<Ability> beforeList)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"LEVEL : {level} -> {level + 1}");

        foreach (Ability ability in resultList)
        {
            Ability before = beforeList.Find(a => a.key == ability.key);
            AbilityData data = DataManager.ability.GetData(ability.key);
            sb.AppendLine($"{data.name} : {before?.value ?? 0} -> {ability.value}");
        }

        return sb.ToString();
    }


    //@업그레이드 진행
    public void UpgradeBtn()
    {
        //강화 가능 여부 체크 및 강화실행
        if (!CheckAbleToUpgrade(ingredientList))
        {
            OG_UIManager.alertInterface.SetAlert("재료가 부족합니다");
            return;
        }

        OG_UIManager.tfInterface.SetTFContent("정말로 강화를 진행하시겠습니까?");
        StartCoroutine(UpgradeCheck());
    }

    //강화 가능 여부 검사
    private bool CheckAbleToUpgrade(List<UpgradeIngred> ingredients)
    {
        
        foreach(UpgradeIngred ingred in ingredients)
        {
            if(!DataManager.inven.IsEnoughItem(DataManager.inven.GetDataWithMasterId(ingred.ingredMasterId).id, ingred.quantity)) //아이템의 양이 충분?
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator UpgradeCheck()
    {
        TFInterface tFInterface = OG_UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //더블체크 완료시 강화실행
            ChangeIngredientData();
            ChangeUpgradeTargetData();
        }
        else
        {
            OG_UIManager.alertInterface.SetAlert($"강화가 취소되었습니다");
        }
    }

    private async void ChangeIngredientData()
    {
        foreach(UpgradeIngred ingred in ingredientList)
        {
            DataManager.inven.DataUpdateOrDelete(DataManager.inven.GetDataWithMasterId(ingred.ingredMasterId).id, ingred.quantity);
        }
        await DataManager.inven.SaveData();
    }

    //실질적인 업그레이드로 인한 데이터 변화
    private void ChangeUpgradeTargetData()
    {
        if (targetType == MasterType.Character)
        {
            //캐릭터
            ChangeCharacterData();
        }
        else if (targetType == MasterType.Parts)
        {
            //파츠
            ChangePartsAbilityData();
        }
        else
        {
            Debug.Log("강화할수 없는 타입");
        }
        ResetUI();
    }


    private async void ChangeCharacterData()
    {
        CharData targetChar = DataManager.character.GetData(DataManager.inven.GetData(targetInvenCode).masterId);
        targetChar.level += 1;
        List<Ability> targetAbility = targetChar.abilityDatas; // 원래 데이터
        foreach (Ability ability in valueList)
        {
            Ability existingAbility = targetAbility.Find(a => a.key == ability.key);

            if (existingAbility != null) // 이미 존재하는 능력이라면
            {
                existingAbility.value += ability.value; // 값 업데이트
            }
            else // 존재하지 않는 능력이라면
            {
                targetAbility.Add(new Ability(ability)); // 새로운 능력 추가
            }
        }
        Debug.Log("캐릭터 강화 완료");
        DataManager.character.UpdateData(DataManager.inven.GetData(targetInvenCode).masterId, targetChar);
        await DataManager.character.SaveData();
        //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Game.UserId,nameof(CharData),DataManager.character.GetFilePath());
    }

    private async void ChangePartsAbilityData()
    {
        PartsData targetParts = DataManager.parts.GetData(targetInvenCode);
        targetParts.level += 1;
        targetParts.mainAbility.value += 5;
        DataManager.parts.UpdateData(targetInvenCode, targetParts);
        await DataManager.parts.SaveData();
        //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Game.UserId,nameof(PartsData),DataManager.parts.GetFilePath());
    }
}
