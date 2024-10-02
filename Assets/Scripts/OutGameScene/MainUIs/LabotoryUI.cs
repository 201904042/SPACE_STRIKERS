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
    

    public int targetInvenCode; //��ȭ�Ϸ��� ĳ���� Ȥ�� ������ �κ��丮 ���̵�

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

        //���ĭ�� ��� �����յ� ��� ����
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
        //����ư, ���Թ�ư, UI���� ��ư���� ����

        //��� ��ư �ʱ�ȭ
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].onClick.RemoveAllListeners();
        }

        charModeBtn.onClick.AddListener(CharMode);
        partModeBtn.onClick.AddListener(PartsMode);

        //������ Ŭ���ϸ� ĳ���ͳ� ���� �������̽� ����
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
            targetInvenCode = DataManager.inven.GetDataWithMasterId(selecteCharInterface.SelectedCode + 100).Value.id; // ���� �ΰ����� ĳ���� �ڵ��� �̼������� �ӽ� +100.
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
        //todo -> Ÿ���� �̹����� �ʿ��� ������ ������ ���� ���� -> ��ȭ�ɽ������� UI���� �����ϰ� ��ȭ���۹�ư�� Ȱ��ȭ
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (DataManager.master.GetData(masterCode).type == ItemType.Character)
        {
            charSlot.GetComponent<CharacterImageBtn>().SetImageByMasterCode(masterCode);

            int curLevel = DataManager.character.GetData(masterCode).level;
            if (upgradeData.upgradeCost[curLevel-1].ingredients.Length == 0)
            {
                //��ȭ �Ұ�(�̹� �ִ� ��ȭ
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

            //todo -> ����

            upgradeBtn.interactable = true;
        }
        else
        {
            UIManager.alterInterface.SetAlert($"{DataManager.inven.GetData(targetInvenCode).name} : �ش� �������� ��ȭ�� �Ҽ� �����ϴ�");
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
        //��ȭ ���� ���� üũ �� ��ȭ����
        if (!CheckAbleToUpgrade(targetInvenCode))
        {
            UIManager.alterInterface.SetAlert("��ᰡ �����մϴ�");
            return;
        }

        UIManager.tfInterface.SetTFContent("������ ��ȭ�� �����Ͻðڽ��ϱ�?");
        StartCoroutine(TFCheck());
    }

    private bool CheckAbleToUpgrade(int targetMasterCode)
    {
        //��ȭdb���� �ʿ��� ��� �˻��Ͽ� �ش� ������ �κ��丮�� �ش緮��ŭ �����ϴ��� �˻���

        return true;
    }

    private IEnumerator TFCheck()
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //����üũ �Ϸ�� ��ȭ����
            UpgradeTarget();
        }
        else
        {
            UIManager.alterInterface.SetAlert($"��ȭ�� ��ҵǾ����ϴ�");
        }
    }

    private void UpgradeTarget()
    {
        //��ȭ ���� �� ������ ����
        //��� ������ ���� �� �κ��丮�� �ش� ���� Ȥ�� ĳ������ ������ ������Ű�� �����Կ� ���� ������ ������Ŵ
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
