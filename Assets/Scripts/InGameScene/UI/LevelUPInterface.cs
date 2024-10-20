using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class LevelUPInterface : UIInterface
{
    private Transform skillSlot; //UI의 스킬버튼이 들어갈 장소
    public Transform buttons;
    public Button selectBtn;
    public Button returnBtn;

    [Header("스킬 목록")]
    
    public List<InGameSkill> activatableSkill; //스킬매니저의 Ingame스킬 리스트 복사

    [Header("선택된 버튼 정보")]
    public InGameSkill selectedSkill;

    //플레이어
    private PlayerInGameExp playerExp;
    private PlayerSkillManager psManager;

    


    public int maxSkillSlotAmount; //생성될 스킬슬롯의 수

    private float exp_increase = 5f;

    
    

    protected override void Awake()
    {
        base.Awake();
    }


    public override void SetComponent()
    {
        base.SetComponent();
        skillSlot = transform.GetChild(2);
        buttons = transform.GetChild(3);
        selectBtn = buttons.GetChild(0).GetComponent<Button>();
        returnBtn = buttons.GetChild(1).GetComponent<Button>();
    }
    protected override void OnConfirm(bool isConfirmed)
    {
        base.OnConfirm(isConfirmed);
    }

    
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        playerExp = GameManager.Instance.myPlayer.GetComponent<PlayerInGameExp>();
        activatableSkill = GameManager.Instance.psManager.ingameSkillList;
        maxSkillSlotAmount = skillSlot.childCount;
        ResetInterface();
        InstantSkillSlot();

        selectBtn.onClick.AddListener(SelectBtn);
        returnBtn.onClick.AddListener(ReturnBtn);
    }

    private void ResetInterface()
    {
        selectedSkill = null;
        selectBtn.interactable = false;

        for (int i = 0; i < skillSlot.childCount; i++)
        {
            skillSlot.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void InstantSkillSlot()
    {
        List<InGameSkill> APSkills = new List<InGameSkill>();
        List<InGameSkill> OSkills = new List<InGameSkill>();
        int skillSlotCount = maxSkillSlotAmount;

        foreach (InGameSkill skill in activatableSkill)
        {
            if (skill.type == SkillType.Other)
            {
                OSkills.Add(skill);
            }
            else
            {
                APSkills.Add(skill);
            }
        }

        if (APSkills.Count == 0)
        {
            //O스킬 랜덤출력 및 스킬슬롯 생성
            skillSlotCount = SetSkillSlot(OSkills, skillSlotCount);
            return;
        }

        //AP스킬 랜덤 생성
        skillSlotCount = SetSkillSlot(APSkills, skillSlotCount);
    }

    private int SetSkillSlot(List<InGameSkill> skillList, int slotNum)
    {
        if (skillList.Count <= maxSkillSlotAmount)
        {
            slotNum = skillList.Count;
        }

        List<int> randomIndexList = new List<int>();

        for (int i = 0; i < slotNum; i++)
        {
            int index = i;
            int randomIndex = RandomSkill(skillList.Count, randomIndexList);
            Button skillSlotBtn = skillSlot.GetChild(i).GetComponent<Button>();
            skillSlotBtn.gameObject.SetActive(true);
            skillSlotBtn.GetComponent<SkillBtn>().SetSkillData(skillList[randomIndex]);
            skillSlotBtn.onClick.AddListener(() => SlotBtnListener(index));
        }

        return slotNum;
    }

    private int RandomSkill(int apSkillNum, List<int> randomList)
    {
        int randomIndex = Random.Range(0, apSkillNum);
        while (randomList.Contains(randomIndex) == true)
        {
            randomIndex = Random.Range(0, apSkillNum);
        }

        randomList.Add(randomIndex);
        return randomIndex;
    }

    private void SlotBtnListener(int i)
    {
        
        selectedSkill = skillSlot.GetChild(i).GetComponent<SkillBtn>().GetSkillData();
        Debug.Log($"{i}번째 버튼 선택 , 스킬은 {selectedSkill.SkillCode}");
        selectBtn.interactable = true;
    }

    public void SelectBtn()
    {
        if (selectedSkill == null)
        {
            Debug.Log("전달된 스킬데이터가 없음");
            return;
        }

        //선택된 스킬을 스킬매니저를 통해 추가
        GameManager.Instance.psManager.AddSkill(selectedSkill);

        Time.timeScale = 1.0f;
        CloseInterface();
    }


    public void ReturnBtn()
    {
        //todo -> 이부분은 레벨업 매니저에서 옮길것
        playerExp.InGameLv--;
        playerExp.maxExp -= exp_increase;
        playerExp.curExp = playerExp.maxExp / 2;
        Time.timeScale = 1.0f;
        CloseInterface();
    }

}
