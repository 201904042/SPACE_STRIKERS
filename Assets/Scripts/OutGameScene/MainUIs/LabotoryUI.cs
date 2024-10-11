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

    public Transform Buttons;
    public Button upgradeBtn;
    public Button mainBtn;
    public Button storeBtn;
    

    public int targetInvenCode; //��ȭ�Ϸ��� ĳ���� Ȥ�� ������ �κ��丮 ���̵�
    public int targetType;
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
        targetType = 0;
        upgradeBtn.interactable = false;
        charSlot.GetComponent<CharacterImageBtn>().ResetData();
        partsSlot.GetComponent<PartsSlot>().ResetData();
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
            targetType = 1;
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
            targetType = 2;
            SetUpgradeUIForParts(targetInvenCode);
        }
    }

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

        List<Ability> beforeAbility = Ability.CopyList(charData.abilityDatas); // ���� ������
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
        PartsData partsData = DataManager.parts.GetData(invenCode);
        partsSlot.GetComponent<PartsSlot>().SetParts(partsData);
        int curLevel = DataManager.parts.GetData(invenCode).level;
        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (upgradeData.upgradeCost[curLevel].ingredients.Length == 0)
        {
            // ��ȭ �Ұ�(�̹� �ִ� ��ȭ ����)
            upgradeInformText.text = "��ȭ�Ұ�";
            return;
        }

        //�Ѽ� ���������� new ����
        ingredientList = new List<UpgradeIngred>(upgradeData.upgradeCost[curLevel].ingredients);

        //���� �����Ƽ ������ ���

        Ability mainAbility = new Ability(partsData.mainAbility); // ���� ������
        List<Ability> subAbility = Ability.CopyList(partsData.subAbilities);
        List<Ability> beforeAbility = new List<Ability>();
        beforeAbility.Add(mainAbility);
        

        Ability changedMainAbility = new Ability(mainAbility);
        changedMainAbility.value += 5;

        List<Ability> resultAbility = new List<Ability>();
        resultAbility.Add(changedMainAbility);


        foreach (UpgradeIngred cost in ingredientList)
        {
            // ��� UI�� ����
            ItemAmountPref itemAmountPref = Instantiate(UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredMasterId, cost.quantity);
        }

        upgradeInformText.text = MakeSBText(curLevel, resultAbility, beforeAbility);
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

        List<Ability> result = Ability.CopyList(beforeList);

        foreach (Ability ability in addList)
        {
            Ability existingAbility = result.Find(a => a.key == ability.key);

            if (existingAbility != null) // �̹� �����ϴ� �ɷ��̶��
            {
                existingAbility.value += ability.value; // �� ������Ʈ
            }
            else // �������� �ʴ� �ɷ��̶��
            {
                result.Add(new Ability(ability)); // ���ο� �ɷ� �߰�
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
        if (!CheckAbleToUpgrade(ingredientList))
        {
            UIManager.alertInterface.SetAlert("��ᰡ �����մϴ�");
            return;
        }

        UIManager.tfInterface.SetTFContent("������ ��ȭ�� �����Ͻðڽ��ϱ�?");
        StartCoroutine(UpgradeCheck());
    }

    private bool CheckAbleToUpgrade(List<UpgradeIngred> ingredients)
    {
        
        foreach(UpgradeIngred ingred in ingredients)
        {
            if(!DataManager.inven.IsEnoughItem(ingred.ingredMasterId, ingred.quantity))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator UpgradeCheck()
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
        InvenData targetData = DataManager.inven.GetData(targetInvenCode);

        if (targetType == 1)
        {
            //ĳ����
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

            DataManager.character.SaveData();
            //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Instance.UserId,nameof(CharData),DataManager.character.GetFilePath());
        }
        else if (targetType == 2) 
        {
            //����
            PartsData targetParts = DataManager.parts.GetData(targetInvenCode);
            targetParts.level += 1;
            targetParts.mainAbility.value += 5;
            DataManager.parts.SaveData();
            //DB_Firebase.UpdateFirebaseNodeFromJson(Auth_Firebase.Instance.UserId,nameof(PartsData),DataManager.parts.GetFilePath());

        }
        else
        {
            Debug.Log("��ȭ�Ҽ� ���� Ÿ��");
        }
        //firebase�� ���� ����

        Reset();
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
