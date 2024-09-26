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
        
        targetInvenCode = DataManager.inventoryData.FindByMasterId(selecteCharInterface.SelectedCode + 100).Value.storageId; // 현재 인게임의 캐릭터 코드의 미수정으로 임시 +100.
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
        //강화 가능 여부 체크 및 강화실행
        if (!CheckAbleToUpgrade(targetInvenCode))
        {
            UIManager.alterInterface.SetAlert("재료가 부족합니다");
        }

        TargetUpgrade(targetInvenCode);
    }
    private bool CheckAbleToUpgrade(int targetMasterCode)
    {
        //강화db에서 필요한 재료 검색하여 해당 재료들이 인벤토리에 해당량만큼 존재하는지 검색함

        return true;
    }

    private void TargetUpgrade(int targetMasterCode)
    {
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
