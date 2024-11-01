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
    

    public int targetInvenCode; //��ȭ�Ϸ��� ĳ���� Ȥ�� ������ �κ��丮 ���̵�
    public MasterType targetType;
    List<UpgradeIngred> ingredientList = new List<UpgradeIngred>(); //��ȭ�� ��� ��� (key , amount)
    List<Ability> valueList = new List<Ability>(); //��ȭ�ҽ� �߰��� �� (key , value)

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

    //@UI ����

    //������Ʈ ����
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

    //UI �ʱ�ȭ
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

    //�ڽİ�ü ����
    private void ClearChildObjects(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    //��ư �ڵ鷯 ����
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

    //��� ��ư�� ������ ����
    private void RemoveAllButtonListeners()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.onClick.RemoveAllListeners();
        }
    }

    //ĳ���� ��ȭ ��� ��ư Ŭ��
    private void ActivateCharacterMode()
    {
        ToggleMode(true);
    }

    //���� ��ȭ ��� ��ư Ŭ��
    private void ActivatePartsMode()
    {
        ToggleMode(false);
    }

    //���� ��忡 ���� Ȱ��ȭ
    private void ToggleMode(bool isCharacterMode)
    {
        charSlot.gameObject.SetActive(isCharacterMode);
        partsSlot.gameObject.SetActive(!isCharacterMode);
        ResetUI();
    }

    //@��ȭ ��� ����

    //ĳ���� ���� ����
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
            targetInvenCode = DataManager.inven.GetDataWithMasterId(selecteCharInterface.SelectedCode).id;
            targetType = MasterType.Character;
            SetUpgradeUIForCharacter(targetInvenCode);
        }
    }

    private void SetUpgradeUIForCharacter(int invenCode)
    {
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        charSlot.GetComponent<CharacterImageBtn>().SetImageByMasterCode(masterCode);
        CharData charData = DataManager.character.GetData(masterCode);
        int curLevel = charData.level;

        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (!CanUpgrade(upgradeData, curLevel))
        {
            upgradeInformText.text = "��ȭ�Ұ�";
            return;
        }

        //�Ѽ� ���������� new ����
        ingredientList = new List<UpgradeIngred>(upgradeData.upgradeCost[curLevel].ingredients);
        valueList = new List<Ability>(upgradeData.upgradeCost[curLevel].upgradeValues); // �߰��� ������

        List<Ability> beforeAbility = Ability.CopyList(charData.abilityDatas); // ���� ������
        List<Ability> resultAbility = SumAbility(beforeAbility, valueList); // ���ļ� ���� ��� ������

        foreach (UpgradeIngred cost in ingredientList)
        {
            // ��� UI�� ����
            ItemAmountPref itemAmountPref = Instantiate(OG_UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
        }
        upgradeInformText.text = MakeSBText(curLevel, resultAbility, beforeAbility);
        upgradeBtn.interactable = true;
    }

    //���� ���� ����
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
        PartsAbilityData PartsAbilityData = DataManager.parts.GetData(invenCode);
        int curLevel = PartsAbilityData.level;

        partsSlot.GetComponent<PartsSlot>().SetParts(invenCode);
        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (!CanUpgrade(upgradeData, curLevel))
        {
            upgradeInformText.text = "��ȭ�Ұ�";
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
            var itemAmountPref = Instantiate(OG_UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
        }
    }

    private void UpdateUpgradeUIForParts(PartsAbilityData PartsAbilityData, int curLevel)
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

    //������ ���� �����Ƽ ����Ʈ�� ���� result�� ������
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

    //result�� �Ͽ��� ��Ʈ�������� ������ ����
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


    //@���׷��̵� ����
    public void UpgradeBtn()
    {
        //��ȭ ���� ���� üũ �� ��ȭ����
        if (!CheckAbleToUpgrade(ingredientList))
        {
            OG_UIManager.alertInterface.SetAlert("��ᰡ �����մϴ�");
            return;
        }

        OG_UIManager.tfInterface.SetTFContent("������ ��ȭ�� �����Ͻðڽ��ϱ�?");
        StartCoroutine(UpgradeCheck());
    }

    //��ȭ ���� ���� �˻�
    private bool CheckAbleToUpgrade(List<UpgradeIngred> ingredients)
    {
        
        foreach(UpgradeIngred ingred in ingredients)
        {
            if(!DataManager.inven.IsEnoughItem(DataManager.inven.GetDataWithMasterId(ingred.ingredMasterId).id, ingred.quantity)) //�������� ���� ���?
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
            //����üũ �Ϸ�� ��ȭ����
            ChangeIngredientData();
            ChangeUpgradeTargetData();
        }
        else
        {
            OG_UIManager.alertInterface.SetAlert($"��ȭ�� ��ҵǾ����ϴ�");
        }
    }

    private void ChangeIngredientData()
    {
        foreach(UpgradeIngred ingred in ingredientList)
        {
            DataManager.inven.DataUpdateOrDelete(DataManager.inven.GetDataWithMasterId(ingred.ingredMasterId).id, ingred.quantity);
        }
        DataManager.inven.SaveData();
    }

    //�������� ���׷��̵�� ���� ������ ��ȭ
    private void ChangeUpgradeTargetData()
    {
        if (targetType == MasterType.Character)
        {
            //ĳ����
            ChangeCharacterData();
        }
        else if (targetType == MasterType.Parts)
        {
            //����
            ChangePartsAbilityData();
        }
        else
        {
            Debug.Log("��ȭ�Ҽ� ���� Ÿ��");
        }
        ResetUI();
    }


    private void ChangeCharacterData()
    {
        CharData targetChar = DataManager.character.GetData(DataManager.inven.GetData(targetInvenCode).masterId);
        targetChar.level += 1;
        List<Ability> targetAbility = targetChar.abilityDatas; // ���� ������
        foreach (Ability ability in valueList)
        {
            Ability existingAbility = targetAbility.Find(a => a.key == ability.key);

            if (existingAbility != null) // �̹� �����ϴ� �ɷ��̶��
            {
                existingAbility.value += ability.value; // �� ������Ʈ
            }
            else // �������� �ʴ� �ɷ��̶��
            {
                targetAbility.Add(new Ability(ability)); // ���ο� �ɷ� �߰�
            }
        }
        Debug.Log("ĳ���� ��ȭ �Ϸ�");
        DataManager.character.UpdateData(DataManager.inven.GetData(targetInvenCode).masterId, targetChar);
        DataManager.character.SaveData();
        //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Game.UserId,nameof(CharData),DataManager.character.GetFilePath());
    }

    private void ChangePartsAbilityData()
    {
        PartsAbilityData targetParts = DataManager.parts.GetData(targetInvenCode);
        targetParts.level += 1;
        targetParts.mainAbility.value += 5;
        DataManager.parts.UpdateData(targetInvenCode, targetParts);
        DataManager.parts.SaveData();
        //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Game.UserId,nameof(PartsAbilityData),DataManager.parts.GetFilePath());
    }
}
