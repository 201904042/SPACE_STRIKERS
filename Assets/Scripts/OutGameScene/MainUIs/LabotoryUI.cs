using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine;
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

    public Transform Buttons;
    public Button upgradeBtn;
    public Button mainBtn;
    public Button storeBtn;
    

    public int targetInvenCode; //강화하려는 캐릭터 혹은 파츠의 인벤토리 아이디

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();

        changeModeBtns = transform.GetChild(0);
        charModeBtn = changeModeBtns.GetChild(0).GetComponent<Button>();
        partModeBtn = changeModeBtns.GetChild(1).GetComponent<Button>();

        targetSlots = transform.GetChild(1);
        charSlot = targetSlots.GetChild(0).GetComponent<Button>();
        partsSlot = targetSlots.GetChild(1).GetComponent<Button>();

        informSlots = transform.GetChild(2);
        ingredientSlot = informSlots.GetChild(0).GetComponentInChildren<ScrollRect>().content;
        upgradeInformText = informSlots.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        Buttons = transform.GetChild(3);
        upgradeBtn = Buttons.GetChild(0).GetComponent<Button>();
        mainBtn = Buttons.GetChild(1).GetComponent<Button>();
        storeBtn = Buttons.GetChild(2).GetComponent<Button>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Reset();
        SetButtons();
        CharMode();
    }

    public void Reset()
    {
        targetInvenCode = 0;
        upgradeBtn.interactable = false;

        //재료칸의 재료 프리팹들 모두 삭제
        if(ingredientSlot.childCount != 0)
        {
            for (int i = ingredientSlot.childCount - 1; i >= 0; i--)
            {
                Transform child = ingredientSlot.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        upgradeInformText.text = "";
    }

    private void SetButtons()
    {
        //모드버튼, 슬롯버튼, UI조작 버튼들모두 설정

        //모든 버튼 초기화
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].onClick.RemoveAllListeners();
        }

        charModeBtn.onClick.AddListener(CharMode);
        partModeBtn.onClick.AddListener(PartsMode);

        //슬롯을 클릭하면 캐릭터나 파츠 인터페이스 실행
        charSlot.onClick.AddListener(GetCharId);
        partsSlot.onClick.AddListener(GetPartsId);

        upgradeBtn.onClick.AddListener(UpgradeBtnHandler);
        mainBtn.onClick.AddListener(GotoMain);
        storeBtn.onClick.AddListener(GotoShop);

    }

    public void CharMode()
    {
        charSlot.gameObject.SetActive(true);
        partsSlot.gameObject.SetActive(false);

        Reset();
    }

    private void GetCharId()
    {
        StartCoroutine(GetCharIdCoroutine());
    }

    private IEnumerator GetCharIdCoroutine()
    {
        SelectCharInterface selecteCharInterface = UIManager.selectCharInterface.GetComponent<SelectCharInterface>();

        yield return StartCoroutine(selecteCharInterface.GetValue());
        if(selecteCharInterface.result == true)
        {
            targetInvenCode = DataManager.inven.GetDataWithMasterId(selecteCharInterface.SelectedCode + 100).Value.id; // 현재 인게임의 캐릭터 코드의 미수정으로 임시 +100.
            SetUpgradeUI(targetInvenCode);
        }
    }


    private void GetPartsId()
    {
        StartCoroutine(GetPartsIdCoroutine());
    }

    private IEnumerator GetPartsIdCoroutine()
    {
        SelectPartsInterface selectPartsInterface = UIManager.selectPartsInterface.GetComponent<SelectPartsInterface>();

        yield return StartCoroutine(selectPartsInterface.GetValue());
        if(selectPartsInterface.result == true)
        {
            targetInvenCode = selectPartsInterface.SelectedParts.invenId;
            SetUpgradeUI(targetInvenCode);
        }
    }

    public void SetUpgradeUI(int invenCode)
    {
        Reset();
        //todo -> 타겟의 이미지와 필요한 재료들의 모음과 현재 스텟 -> 강화될스텟으로 UI들을 변경하고 강화시작버튼을 활성화
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (DataManager.master.GetData(masterCode).type == ItemType.Character)
        {
            charSlot.GetComponent<CharacterImageBtn>().SetImageByMasterCode(masterCode);

            int curLevel = DataManager.character.GetData(masterCode).level;
            if (upgradeData.upgradeCost[curLevel-1].ingredients.Length == 0)
            {
                //강화 불가(이미 최대 강화
            }

            foreach(UpgradeIngred cost in upgradeData.upgradeCost[curLevel-1].ingredients)
            {
                ItemAmountPref itemAmountPref = Instantiate(UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
                itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
            }

            CharData changedData = DataManager.character.GetData(masterCode);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{changedData.name}");
            sb.AppendLine($"LEVEL : {changedData.level}");

            sb.AppendLine($"DMG: {changedData.damage}");
            sb.AppendLine($"DEF: {changedData.defense}");
            sb.AppendLine($"ASPD: {changedData.attackSpeed}");
            sb.AppendLine($"MSPD: {changedData.moveSpeed}");
            sb.AppendLine($"HP: {changedData.hp}");

            if (changedData.hpRegen != 0) sb.AppendLine($"regenHP : {changedData.hpRegen}");
            if (changedData.troopsDamageUp != 0) sb.AppendLine($"TroopsDmgUp: {changedData.troopsDamageUp}");
            if (changedData.bossDamageUp != 0) sb.AppendLine($"BossDmgUp: {changedData.bossDamageUp}");
            if (changedData.stageExpRateUp != 0) sb.AppendLine($"StageExpUp: {changedData.stageExpRateUp}");
            if (changedData.stageItemDropRateUp != 0) sb.AppendLine($"ItemRegenUp: {changedData.stageItemDropRateUp}");
            if (changedData.powRegenRateUp != 0) sb.AppendLine($"PowRegenUp: {changedData.powRegenRateUp}");
            if (changedData.powAmountUp != 0) sb.AppendLine($"PowAmountUp : {changedData.powAmountUp}");
            if (changedData.accountExpUp != 0) sb.AppendLine($"AccountExpUp: {changedData.accountExpUp}");
            if (changedData.accountMoneyUp != 0) sb.AppendLine($"AccountMoneyUp: {changedData.accountMoneyUp}");
            if (changedData.startLevelUp != 0) sb.AppendLine($"StartLevelUp: {changedData.startLevelUp}");
            if (changedData.revival != 0) sb.AppendLine($"Revive : {changedData.revival}");
            if (changedData.startWeaponUp != 0) sb.AppendLine($"StartWeaponUp: {changedData.startWeaponUp}");

            upgradeInformText.text = sb.ToString();


            upgradeBtn.interactable = true;
        }
        else if(DataManager.master.GetData(masterCode).type == ItemType.Parts)
        {
            partsSlot.GetComponent<PartsSlot>().SetParts(DataManager.parts.GetData(invenCode));

            int curLevel = DataManager.parts.GetData(invenCode).level;
            foreach (UpgradeCost cost in upgradeData.upgradeCost)
            {
                ItemAmountPref itemAmountPref = Instantiate(UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
                itemAmountPref.SetAmountUI(cost.ingredients[curLevel - 1].ingredMasterId, cost.ingredients[curLevel - 1].quantity);
            }

            //todo -> 스텟

            upgradeBtn.interactable = true;
        }
        else
        {
            UIManager.alterInterface.SetAlert($"{DataManager.inven.GetData(targetInvenCode).name} : 해당 아이템은 강화를 할수 없습니다");
            Reset();
            return;
        }
    }

    

    public void PartsMode()
    {
        charSlot.gameObject.SetActive(false);
        partsSlot.gameObject.SetActive(true);

        Reset();
    }

    public void UpgradeBtnHandler()
    {
        //강화 가능 여부 체크 및 강화실행
        if (!CheckAbleToUpgrade(targetInvenCode))
        {
            UIManager.alterInterface.SetAlert("재료가 부족합니다");
            return;
        }

        UIManager.tfInterface.SetTFContent("정말로 강화를 진행하시겠습니까?");
        StartCoroutine(TFCheck());
    }

    private bool CheckAbleToUpgrade(int targetMasterCode)
    {
        //강화db에서 필요한 재료 검색하여 해당 재료들이 인벤토리에 해당량만큼 존재하는지 검색함

        return true;
    }

    private IEnumerator TFCheck()
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //더블체크 완료시 강화실행
            UpgradeTarget();
        }
        else
        {
            UIManager.alterInterface.SetAlert($"강화가 취소되었습니다");
        }
    }

    private void UpgradeTarget()
    {
        //강화 실행 및 데이터 변경
        //재료 아이템 감소 및 인벤토리에 해당 파츠 혹은 캐릭터의 레벨을 증가시키고 증가함에 따른 스텟을 증가시킴
    }

    public void GotoMain()
    {
        ChangeUI(UIManager.UIInstance.mainUI);
    }

    public void GotoShop()
    {
        ChangeUI(UIManager.UIInstance.stageUI);
    }
}
