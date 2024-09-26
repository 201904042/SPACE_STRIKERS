using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    int targetInvenCode;

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

        upgradeBtn.onClick.AddListener(Upgrade);
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
        StartCoroutine(GetCharIdCoroutine(targetInvenCode));
    }

    private IEnumerator GetCharIdCoroutine(int target)
    {
        SelectCharInterface selecteCharInterface = UIManager.selectCharInterface.GetComponent<SelectCharInterface>();

        yield return StartCoroutine(selecteCharInterface.GetValue());
        
        targetInvenCode = DataManager.inventoryData.FindByMasterId(selecteCharInterface.SelectedCode + 100).Value.storageId; // ���� �ΰ����� ĳ���� �ڵ��� �̼������� �ӽ� +100.
    }

    private void GetPartsId()
    {
        StartCoroutine(GetPartsIdCoroutine());
    }

    private IEnumerator GetPartsIdCoroutine()
    {
        SelectPartsInterface selectPartsInterface = UIManager.selectPartsInterface.GetComponent<SelectPartsInterface>();

        yield return StartCoroutine(selectPartsInterface.GetValue());

        targetInvenCode = selectPartsInterface.SelectedParts.inventoryCode;
    }

    public void PartsMode()
    {
        charSlot.gameObject.SetActive(false);
        partsSlot.gameObject.SetActive(true);

        Reset();
    }

    public void Upgrade()
    {
        //��ȭ ���� ���� üũ �� ��ȭ����
        if (!CheckAbleToUpgrade(targetInvenCode))
        {
            UIManager.alterInterface.SetAlert("��ᰡ �����մϴ�");
        }

        TargetUpgrade(targetInvenCode);
    }
    private bool CheckAbleToUpgrade(int targetMasterCode)
    {
        //��ȭdb���� �ʿ��� ��� �˻��Ͽ� �ش� ������ �κ��丮�� �ش緮��ŭ �����ϴ��� �˻���

        return true;
    }

    private void TargetUpgrade(int targetMasterCode)
    {
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
