using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelUP_UI : MonoBehaviour
{
    [Header("스킬 버튼")]
    public GameObject skillBtn;
    [Header("스킬 목록")]
    public GameObject[] ActiveSkill;
    public GameObject[] PassiveSkill;
    public GameObject otherSkill;
    [Header("선택된 버튼 정보")]
    public int chosenSkillId;
    public string chosenSkillType;

    private GameObject player;
    private PlayerInGameExp playerExp;
    private Transform playerSkillSlot; //플레이어의 스킬칸
    private Transform skillSlot; //UI의 스킬버튼이 들어갈 장소
    private bool[] levelMax;


    private int skillSlotNum = 3;
    private int skillNum;

    private string Active = "Active";
    private string Passive = "Passive";
    private string Extra = "Extra";
    private string Clone = "(Clone)";

    private float exp_increase = 5f;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerExp = player.GetComponent<PlayerInGameExp>();
        playerSkillSlot = player.transform.GetChild(1);
        skillSlot = transform.GetChild(1);
        skillNum = ActiveSkill.Length + PassiveSkill.Length;
        levelMax = new bool[skillNum];
        Array.Fill(levelMax, false); //levelMax배열의 모든 배열 false로 초기화
        chosenSkillId = -1;
        for (int i = 0; i < levelMax.Length; i++)
        {
            levelMax[i] = is_skillLevelMax(i);
        }

        InstantSkillSlot();
    }
    
    private void InstantSkillSlot()
    {
        if (CountFalseInLevelMax() < skillSlotNum)
        {
            skillSlotNum = CountFalseInLevelMax();
        }

        if (skillSlotNum == 0) //extra 스킬 지정
        {
            SetSkillSlot(-1);
        }
        else //active, passive 스킬 랜덤 지정
        {
            HashSet<int> setId = new HashSet<int>();
            while (setId.Count < skillSlotNum)
            {
                
                int randomNum = UnityEngine.Random.Range(0, skillNum);
                if (!setId.Contains(randomNum) && !levelMax[randomNum]) //중복과 만렙 제거
                {
                    setId.Add(randomNum);
                }
            }
            int[] skillId = new int[skillSlotNum];
            setId.CopyTo(skillId);

            for (int i = 0; i < skillSlotNum; i++) //UI에 스킬 버튼을 띄우는 과정
            {
                SetSkillSlot(skillId[i]);
            }
        }
    }

    private int CountFalseInLevelMax() //만렙이 아닌 A,P스킬의 개수
    {
        int falseCount = 0;
        foreach (bool element in levelMax)
        {
            if (!element)
            {
                falseCount++;
            }
        }
        return falseCount;
    }

    private bool is_skillLevelMax(int i)
    {
        float playerSkillSlotCount = playerSkillSlot.childCount;
        SkillInterface s_interface = null;
        if (i < ActiveSkill.Length) //Active 스킬일 경우
        {
            return checkSkillLvLoop(s_interface, playerSkillSlotCount, ActiveSkill[i]);
        }
        else if (i >= ActiveSkill.Length) //Passive 스킬일 경우
        {
            i -= ActiveSkill.Length;
            return checkSkillLvLoop(s_interface, playerSkillSlotCount, PassiveSkill[i]);
        }
        return false;
    }

    private bool checkSkillLvLoop(SkillInterface s_interf, float playerSlotNum, GameObject skill)
    {
        int j = 0;
        while (playerSlotNum != j)
        {
            if (skill.name + Clone == playerSkillSlot.GetChild(j).name)
            {
                s_interf = playerSkillSlot.GetChild(j).GetComponent<SkillInterface>();
                if (s_interf.level >= 5)
                { //플레이어의 스킬이 5레벨 이상이라면 true
                    return true;
                }
                break;
            }
            j++;
        }
        return false;
    }

    private void SetSkillSlot(int i)
    {
        int skillSlotCount = playerSkillSlot.childCount;
        if(i == -1)
        {
            SkillInterface s_interface =  otherSkill.GetComponent<SkillInterface>();
            MakeSkillSlot(i, Extra, s_interface);
        }
        else if (i < ActiveSkill.Length) //Active 스킬일 경우
        {
            CheckSkillSlot(skillSlotCount, i, Active, ActiveSkill[i]);
        }
        else if (i >= ActiveSkill.Length) //Passive 스킬일 경우
        {
            i -= ActiveSkill.Length;
            CheckSkillSlot(skillSlotCount, i, Passive, PassiveSkill[i]);
        }
    }

    private void CheckSkillSlot(int skillSlotnum,int s_id, string s_type,GameObject skill)
    {
        SkillInterface s_interface = skill.GetComponent<SkillInterface>();
        int j = 0;
        while (skillSlotnum != j)
        {
            if (skill.name + Clone == playerSkillSlot.GetChild(j).name)
            {
                s_interface = playerSkillSlot.GetChild(j).GetComponent<SkillInterface>();
                break;
            }
            j++;
        }
        MakeSkillSlot(s_id, s_type, s_interface);
    }

    private void MakeSkillSlot(int skillId, string skillType, SkillInterface s_interf)
    {
        SkillBtn skillBtnObj = Instantiate(skillBtn, skillSlot).GetComponent<SkillBtn>();
        skillBtnObj.btnImage = s_interf.icon;
        skillBtnObj.LvText.text = "LV : " + s_interf.level.ToString();
        skillBtnObj.explainText.text = s_interf.skillIntro.ToString();
        skillBtnObj.isImageSet = false;
        skillBtnObj.skillId = skillId;
        skillBtnObj.skillType = skillType;
    }

    private void FindChosenSkillInPlayerSkill(int playerSlotNum,GameObject skill)
    {
        bool isAlreadyHave = false;
        int i = 0;
        while (playerSlotNum != i)
        {
            if (skill.name + Clone == playerSkillSlot.GetChild(i).name)
            {
                playerSkillSlot.GetChild(i).GetComponent<SkillInterface>().level++;
                isAlreadyHave = true;
                break;
            }
            i++;
        }
        if (!isAlreadyHave)
        {
            Instantiate(skill, playerSkillSlot);
        }
    }

    public void AcceptBtn()
    {
        if (chosenSkillId != -1)
        {
            int skillSlotCount = playerSkillSlot.childCount;

            if (chosenSkillType == Active)
            {
                FindChosenSkillInPlayerSkill(skillSlotCount, ActiveSkill[chosenSkillId]);
            }
            else if (chosenSkillType == Passive)
            {
                FindChosenSkillInPlayerSkill(skillSlotCount, PassiveSkill[chosenSkillId]);
            }
        }
        else if (chosenSkillId == -1)
        {
            Instantiate(otherSkill, playerSkillSlot);
        }

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
    public void RewindBtn()
    {
        playerExp.InGameLv--;
        playerExp.maxExp -= exp_increase;
        playerExp.curExp = playerExp.maxExp / 2;
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
