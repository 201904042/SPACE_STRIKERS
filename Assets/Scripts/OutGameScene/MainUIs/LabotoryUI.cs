using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
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

    public Transform Buttons;
    public Button upgradeBtn;
    public Button mainBtn;
    public Button storeBtn;
    

    public int targetInvenCode; //��ȭ�Ϸ��� ĳ���� Ȥ�� ������ �κ��丮 ���̵�
    List<UpgradeIngred> ingredientList = new List<UpgradeIngred>(); //��ȭ�� ��� ��� (key , amount)
    List<Ability> valueList = new List<Ability>(); //��ȭ�ҽ� �߰��� �� (key , value)

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

        ingredientList.Clear();
        valueList.Clear();

        //���ĭ�� ��� �����յ� ��� ����
        if (ingredientSlot.childCount != 0)
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
        Reset();
        StartCoroutine(GetCharIdCoroutine());
    }

    private IEnumerator GetCharIdCoroutine()
    {
        SelectCharInterface selecteCharInterface = UIManager.selectCharInterface.GetComponent<SelectCharInterface>();

        yield return StartCoroutine(selecteCharInterface.GetValue());
        if(selecteCharInterface.result == true)
        {
            targetInvenCode = DataManager.inven.GetDataWithMasterId(selecteCharInterface.SelectedCode + 100).Value.id; // ���� �ΰ����� ĳ���� �ڵ��� �̼������� �ӽ� +100.
            SetUpgradeUIForCharacter(targetInvenCode);
        }
    }


    private void GetPartsId()
    {
        Reset();
        StartCoroutine(GetPartsIdCoroutine());
    }

    private IEnumerator GetPartsIdCoroutine()
    {
        SelectPartsInterface selectPartsInterface = UIManager.selectPartsInterface.GetComponent<SelectPartsInterface>();

        yield return StartCoroutine(selectPartsInterface.GetValue());
        if(selectPartsInterface.result == true)
        {
            targetInvenCode = selectPartsInterface.SelectedParts.invenId;
            SetUpgradeUIForParts(targetInvenCode);
        }
    }

    //todo -> ���� ����. ���ľ���
    private void SetUpgradeUIForCharacter(int invenCode)
    {
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        charSlot.GetComponent<CharacterImageBtn>().SetImageByMasterCode(masterCode);

        CharData charData = DataManager.character.GetData(masterCode);
        int curLevel = charData.level;

        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (upgradeData.upgradeCost[curLevel].ingredients.Length == 0)
        {
            // ��ȭ �Ұ�(�̹� �ִ� ��ȭ ����)
            upgradeInformText.text = "��ȭ�Ұ�";
            return;
        }

        //�Ѽ� ���������� new ����
        ingredientList = new List<UpgradeIngred>(upgradeData.upgradeCost[curLevel].ingredients);
        valueList = new List<Ability>(upgradeData.upgradeCost[curLevel].upgradeValues); // �߰��� ������

        List<Ability> beforeAbility = new List<Ability>(charData.abilityDatas); // ���� ������
        List<Ability> resultAbility = SumAbility(beforeAbility, valueList); // ���ļ� ���� ��� ������

        foreach (UpgradeIngred cost in ingredientList)
        {
            // ��� UI�� ����
            ItemAmountPref itemAmountPref = Instantiate(UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
        }

        upgradeInformText.text = MakeSBText(curLevel, resultAbility, beforeAbility);

        upgradeBtn.interactable = true;
    }

    private void SetUpgradeUIForParts(int invenCode)
    {
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        partsSlot.GetComponent<PartsSlot>().SetParts(DataManager.parts.GetData(invenCode));
        int curLevel = DataManager.parts.GetData(invenCode).level;
        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        foreach (UpgradeCost cost in upgradeData.upgradeCost)
        {
            // �ʿ��� ��� ����Ʈ �߰�
            ItemAmountPref itemAmountPref = Instantiate(UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredients[curLevel - 1].ingredMasterId, cost.ingredients[curLevel - 1].quantity);
        }

        // TODO: ������ ���� ���� UI ������Ʈ
        upgradeBtn.interactable = true;
    }

    public void PartsMode()
    {
        charSlot.gameObject.SetActive(false);
        partsSlot.gameObject.SetActive(true);

        Reset();
    }

    //������ ���� �����Ƽ ����Ʈ�� ���� result�� ������
    private List<Ability> SumAbility(List<Ability> beforeList, List<Ability> addList)
    {
        // ����� ������ ����Ʈ
        List<Ability> result = new List<Ability>(beforeList); // ���� �ɷ�ġ ����

        foreach (Ability ability in addList)
        {
            // beforeList���� �ش� key�� ���� �ɷ��� ã��
            Ability existingAbility = result.Find(a => a.key == ability.key);

            if (existingAbility != null) // �̹� �����ϴ� �ɷ��̶��
            {
                existingAbility.value += ability.value; // �� ������Ʈ
            }
            else // �������� �ʴ� �ɷ��̶��
            {
                result.Add(new Ability { key = ability.key, value = ability.value }); // ���ο� �ɷ� �߰�
            }
        }

        return result;
    }

    //result�� �Ͽ��� ��Ʈ�������� ������ ����
    private string MakeSBText(int level, List<Ability> ResultList, List<Ability> beforeList)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"LEVEL : {level} -> {level + 1}");
        foreach (Ability ability in ResultList)
        {
            Ability before = beforeList.Find(a => a.key == ability.key);
            AbilityData data = DataManager.ability.GetData(ability.key);
            if (before != null)
            {
                sb.AppendLine($"{data.name} : {before.value} -> {ability.value}");
            }
            else
            {
                sb.AppendLine($"{data.name} : 0 -> {ability.value}");
            }
        }

        return sb.ToString();
    }



    public void UpgradeBtnHandler()
    {
        //��ȭ ���� ���� üũ �� ��ȭ����
        if (!CheckAbleToUpgrade(targetInvenCode))
        {
            UIManager.alertInterface.SetAlert("��ᰡ �����մϴ�");
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
            UIManager.alertInterface.SetAlert($"��ȭ�� ��ҵǾ����ϴ�");
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
