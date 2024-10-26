using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ReadyUI : MainUIs
{
    public Transform charZone;
    public Button charSlotBtn;
    public CharacterImageBtn charSlotUI;
    public TextMeshProUGUI charInformText;

    public Transform partsZone;

    public Transform bottomZone;
    public TextMeshProUGUI stageText;
    public Button backBtn;
    public Button gotoIngameBtn;

    public int CurPlayerCode
    {
        get => DataManager.account.GetChar();
    }
    public int[] PartsCode
    {
        get => DataManager.account.GetPartsArray();
    }

    //아이템 파트는 아직 미구현

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        charZone = transform.GetChild(0);
        charSlotBtn = charZone.GetChild(0).GetComponent<Button>();
        charSlotUI = charSlotBtn.GetComponent<CharacterImageBtn>();
        charInformText = charZone.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        partsZone = transform.GetChild(1);
        //itemZone = transform.GetChild(2).GetChild(1);
        //item1 = itemZone.GetChild(0).GetComponent<Button>();
        //item2 = itemZone.GetChild(1).GetComponent<Button>();
        //item3 = itemZone.GetChild(2).GetComponent<Button>();
        //item4 = itemZone.GetChild(3).GetComponent<Button>();
        bottomZone = transform.GetChild(3);
        stageText = bottomZone.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        backBtn = bottomZone.GetChild(1).GetChild(1).GetComponent<Button>();
        gotoIngameBtn = bottomZone.GetChild(1).GetChild(0).GetComponent<Button>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetInitUIs();
    }

    private void SetInitUIs()
    {
        PlayerImageSet();

        for (int i = 0; i< PartsCode.Length; i++)
        {
            UpdatePartsSlot(i+1, PartsCode[i]);
        }

        PlayerStatTextSet();

        stageText.text = $"스테이지 : {DataManager.account.GetPlanet()}-{DataManager.account.GetStage()}";

        SetBtnListener();
    }

    private void SetBtnListener()
    {
        charSlotBtn.onClick.RemoveAllListeners();
        charSlotBtn.onClick.AddListener(GetCharacterId);
        for (int i = 0; i < partsZone.childCount; i++)
        {
            int index = i + 1;
            partsZone.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            partsZone.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GetPartsId(index));
        }
        
        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(GotoStage);
        gotoIngameBtn.onClick.RemoveAllListeners();
        gotoIngameBtn.onClick.AddListener(GameStart);

    }

    private void UpdateCharacter(int characterMasterId)
    {
        DataManager.account.SetCharValue(characterMasterId);
        PlayerImageSet();
        PlayerStatTextSet();
    }

    private void PlayerImageSet()
    {
        charSlotUI.SetImageByMasterCode(CurPlayerCode);
    }

    private void PlayerStatTextSet()
    {
        CharData targetBasicData = DataManager.character.GetData(CurPlayerCode);
        Dictionary<int, float> changedData = CalculateTotalAbilities();
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{targetBasicData.name}");
        sb.AppendLine($"LEVEL : {targetBasicData.level}");

        foreach (var ability in changedData)
        {
            AbilityData targetAbility = DataManager.ability.GetData(ability.Key);
            sb.AppendLine($"{targetAbility.name}: {ability.Value}");
        }

        charInformText.text = sb.ToString();
    }

    /// <summary>
    /// 현재 플레이어가 장착한 파츠들로 스텟 증감
    /// </summary>
    public Dictionary<int, float> CalculateTotalAbilities()
    {
        CharData characterData = DataManager.character.GetData(CurPlayerCode);
        // 캐릭터의 어빌리티 값을 딕셔너리로 변환
        Dictionary<int, float> totalAbilities = new Dictionary<int, float>();
        foreach (var ability in characterData.abilityDatas)
        {
            if (!totalAbilities.ContainsKey(ability.key))
            {
                totalAbilities[ability.key] = ability.value;
            }
            else
            {
                totalAbilities[ability.key] += ability.value;
            }
        }

        foreach(int partsInvenId in PartsCode)
        {
            PartsAbilityData partData = DataManager.parts.GetData(partsInvenId);
            if(partData == null)
            {
                continue;
            }
            if (partData.mainAbility != null)
            {
                int mainKey = partData.mainAbility.key;
                float mainValue = partData.mainAbility.value;

                if (!totalAbilities.ContainsKey(mainKey))
                {
                    totalAbilities[mainKey] = mainValue;
                }
                else
                {
                    totalAbilities[mainKey] += mainValue;
                }
            }

            // 파츠의 서브 어빌리티 추가
            foreach (var subAbility in partData.subAbilities)
            {
                int subKey = subAbility.key;
                float subValue = subAbility.value;

                if (!totalAbilities.ContainsKey(subKey))
                {
                    totalAbilities[subKey] = subValue;
                }
                else
                {
                    totalAbilities[subKey] += subValue;
                }
            }
        }
        // 파츠의 메인 어빌리티 추가

        return totalAbilities;
    }

    /// <summary>
    /// 장착 슬롯에 해당 파츠를 등록 혹은 해제
    /// </summary>
    private void UpdatePartsSlot(int slotKey, int changedPartsId)
    {
        CheckDuplicateParts(changedPartsId);
        DataManager.account.SetParts(slotKey, changedPartsId);
        partsZone.GetChild(slotKey-1).GetComponent<PartsSlot>().SetParts(changedPartsId);
        
    }

    private void CheckDuplicateParts(int partsInvenCode)
    {
        for (int i = 0; i < PartsCode.Length; i++)
        {
            if (PartsCode[i] == partsInvenCode)
            {
                //배열을 순회하며 바꿔야하는 아이디가 같다면 해당 칸은 비움
                DataManager.account.SetParts(i + 1, 0);
                partsZone.GetChild(i).GetComponent<PartsSlot>().SetParts(0);
            }
        }
    }

    

    //인터페이스에 의한 캐릭터 선택
    private void GetCharacterId()
    {
        StartCoroutine(GetCharacterIdCoroutine());
    }

    private IEnumerator GetCharacterIdCoroutine()
    {
        SelectCharInterface selecteCharInterface = UIManager.selectCharInterface.GetComponent<SelectCharInterface>();

        yield return StartCoroutine(selecteCharInterface.GetValue());

        if (selecteCharInterface.result == true)
        {
            UpdateCharacter(selecteCharInterface.SelectedCode);
            PlayerStatTextSet();
        }
    }

    //인터페이스에 의한 파츠의 선택
    private void GetPartsId(int partsSlotId)
    {
        StartCoroutine(GetPartsIdCoroutine(partsSlotId));
    }

    private IEnumerator GetPartsIdCoroutine(int partsSlotId)
    {
        SelectPartsInterface selectPartsInterface = UIManager.selectPartsInterface;

        yield return StartCoroutine(selectPartsInterface.GetValue());

        if (selectPartsInterface.result == true)
        {
            UpdatePartsSlot(partsSlotId, selectPartsInterface.SelectedParts.invenId);
            PlayerStatTextSet();
        }

    }


    /* 아이템은 보류
    private void ItemBtnChange(Button item, bool value)
    {
        item.transform.GetChild(0).GetComponent<Image>().currentColor = value ? Color.yellow : Color.white;
        //item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = quantity - 1;
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
    */

    public void GotoStage()
    {
        ChangeUI(UIManager.UIInstance.stageUI);
    }
    public void GameStart()
    {
        //게임 시작
        SceneManager.LoadScene("InGameTest");
    }

    
}
