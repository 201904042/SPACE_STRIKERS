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
    

    public int targetInvenCode; //강화하려는 캐릭터 혹은 파츠의 인벤토리 아이디
    List<UpgradeIngred> ingredientList = new List<UpgradeIngred>(); //강화에 드는 재료 (key , amount)
    List<Ability> valueList = new List<Ability>(); //강화할시 추가될 값 (key , value)

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

        //재료칸의 재료 프리팹들 모두 삭제
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
        Reset();
        StartCoroutine(GetCharIdCoroutine());
    }

    private IEnumerator GetCharIdCoroutine()
    {
        SelectCharInterface selecteCharInterface = UIManager.selectCharInterface.GetComponent<SelectCharInterface>();

        yield return StartCoroutine(selecteCharInterface.GetValue());
        if(selecteCharInterface.result == true)
        {
            targetInvenCode = DataManager.inven.GetDataWithMasterId(selecteCharInterface.SelectedCode + 100).Value.id; // 현재 인게임의 캐릭터 코드의 미수정으로 임시 +100.
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

    //todo -> 오류 있음. 고쳐야함
    private void SetUpgradeUIForCharacter(int invenCode)
    {
        int masterCode = DataManager.inven.GetData(invenCode).masterId;
        charSlot.GetComponent<CharacterImageBtn>().SetImageByMasterCode(masterCode);

        CharData charData = DataManager.character.GetData(masterCode);
        int curLevel = charData.level;

        UpgradeData upgradeData = DataManager.upgrade.GetData(masterCode);

        if (upgradeData.upgradeCost[curLevel].ingredients.Length == 0)
        {
            // 강화 불가(이미 최대 강화 상태)
            upgradeInformText.text = "강화불가";
            return;
        }

        //훼손 방지를위한 new 생성
        ingredientList = new List<UpgradeIngred>(upgradeData.upgradeCost[curLevel].ingredients);
        valueList = new List<Ability>(upgradeData.upgradeCost[curLevel].upgradeValues); // 추가될 데이터

        List<Ability> beforeAbility = new List<Ability>(charData.abilityDatas); // 원래 데이터
        List<Ability> resultAbility = SumAbility(beforeAbility, valueList); // 합쳐서 나온 결과 데이터

        foreach (UpgradeIngred cost in ingredientList)
        {
            // 재료 UI들 생성
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
            // 필요한 재료 리스트 추가
            ItemAmountPref itemAmountPref = Instantiate(UIManager.UIInstance.itemAmountPref, ingredientSlot).GetComponent<ItemAmountPref>();
            itemAmountPref.SetAmountUI(cost.ingredients[curLevel - 1].ingredMasterId, cost.ingredients[curLevel - 1].quantity);
        }

        // TODO: 파츠의 스탯 관련 UI 업데이트
        upgradeBtn.interactable = true;
    }

    public void PartsMode()
    {
        charSlot.gameObject.SetActive(false);
        partsSlot.gameObject.SetActive(true);

        Reset();
    }

    //이전과 더할 어벌리티 리스트를 합쳐 result를 리턴함
    private List<Ability> SumAbility(List<Ability> beforeList, List<Ability> addList)
    {
        // 결과를 저장할 리스트
        List<Ability> result = new List<Ability>(beforeList); // 기존 능력치 복사

        foreach (Ability ability in addList)
        {
            // beforeList에서 해당 key를 가진 능력을 찾음
            Ability existingAbility = result.Find(a => a.key == ability.key);

            if (existingAbility != null) // 이미 존재하는 능력이라면
            {
                existingAbility.value += ability.value; // 값 업데이트
            }
            else // 존재하지 않는 능력이라면
            {
                result.Add(new Ability { key = ability.key, value = ability.value }); // 새로운 능력 추가
            }
        }

        return result;
    }

    //result로 하여금 스트링빌더로 정보를 구성
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
        //강화 가능 여부 체크 및 강화실행
        if (!CheckAbleToUpgrade(targetInvenCode))
        {
            UIManager.alertInterface.SetAlert("재료가 부족합니다");
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
            UIManager.alertInterface.SetAlert($"강화가 취소되었습니다");
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
