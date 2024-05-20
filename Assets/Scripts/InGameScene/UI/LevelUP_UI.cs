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
    private Player_InGame_Exp p_exp;
    private Transform p_skillSlot; //플레이어의 스킬칸
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
        p_exp = player.GetComponent<Player_InGame_Exp>();
        p_skillSlot = player.transform.GetChild(1);
        skillSlot = transform.GetChild(1);
        skillNum = ActiveSkill.Length + PassiveSkill.Length;
        levelMax = new bool[skillNum];
        Array.Fill(levelMax, false); //levelMax배열의 모든 배열 false로 초기화
        chosenSkillId = -1;
        for (int i = 0; i < levelMax.Length; i++)
        {
            levelMax[i] = is_skillLevelMax(i);
        }

        instantSkillSlot();
    }
    
    private void instantSkillSlot()
    {
        if (countFalseInLevelMax() < skillSlotNum)
        {
            skillSlotNum = countFalseInLevelMax();
        }

        if (skillSlotNum == 0) //extra 스킬 지정
        {
            SetSkillSlot(-1);
        }
        else //active, passive 스킬 랜덤 지정
        {
            HashSet<int> setid = new HashSet<int>();
            while (setid.Count < skillSlotNum)
            {
                
                int randomNum = UnityEngine.Random.Range(0, skillNum);
                if (!setid.Contains(randomNum) && !levelMax[randomNum]) //중복과 만렙 제거
                {
                    setid.Add(randomNum);
                }
            }
            int[] skillId = new int[skillSlotNum];
            setid.CopyTo(skillId);

            for (int i = 0; i < skillSlotNum; i++) //UI에 스킬 버튼을 띄우는 과정
            {
                SetSkillSlot(skillId[i]);
            }
        }
    }

    private int countFalseInLevelMax() //만렙이 아닌 A,P스킬의 개수
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
        float p_skillSlotCount = p_skillSlot.childCount;
        skill_interface s_interface = null;
        if (i < ActiveSkill.Length) //Active 스킬일 경우
        {
            return checkSkillLvLoop(s_interface, p_skillSlotCount, ActiveSkill[i]);
        }
        else if (i >= ActiveSkill.Length) //Passive 스킬일 경우
        {
            i -= ActiveSkill.Length;
            return checkSkillLvLoop(s_interface, p_skillSlotCount, PassiveSkill[i]);
        }
        return false;
    }

    private bool checkSkillLvLoop(skill_interface s_interf, float p_slotNum, GameObject skill)
    {
        int j = 0;
        while (p_slotNum != j)
        {
            if (skill.name + Clone == p_skillSlot.GetChild(j).name)
            {
                s_interf = p_skillSlot.GetChild(j).GetComponent<skill_interface>();
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
        int skillSlotCount = p_skillSlot.childCount;
        if(i == -1)
        {
            skill_interface s_interface =  otherSkill.GetComponent<skill_interface>();
            makeSkillSlot(i, Extra, s_interface);
        }
        else if (i < ActiveSkill.Length) //Active 스킬일 경우
        {
            checkSkillSlot(skillSlotCount, i, Active, ActiveSkill[i]);
        }
        else if (i >= ActiveSkill.Length) //Passive 스킬일 경우
        {
            i -= ActiveSkill.Length;
            checkSkillSlot(skillSlotCount, i, Passive, PassiveSkill[i]);
        }
    }

    private void checkSkillSlot(int skillSlotnum,int s_id, string s_type,GameObject skill)
    {
        skill_interface s_interface = skill.GetComponent<skill_interface>();
        int j = 0;
        while (skillSlotnum != j)
        {
            if (skill.name + Clone == p_skillSlot.GetChild(j).name)
            {
                s_interface = p_skillSlot.GetChild(j).GetComponent<skill_interface>();
                break;
            }
            j++;
        }
        makeSkillSlot(s_id, s_type, s_interface);
    }

    private void makeSkillSlot(int s_id, string s_type,skill_interface s_interf)
    {
        SkillBtn skillBtnObj = Instantiate(skillBtn, skillSlot).GetComponent<SkillBtn>();
        skillBtnObj.btnImage = s_interf.icon;
        skillBtnObj.LvText.text = "LV : " + s_interf.level.ToString();
        skillBtnObj.explainText.text = s_interf.skill_intro.ToString();
        skillBtnObj.is_imageSet = false;
        skillBtnObj.skillId = s_id;
        skillBtnObj.skillType = s_type;
    }

    public void AcceptBtn()
    {
        if (chosenSkillId != -1)
        {
            int skillSlotCount = p_skillSlot.childCount;

            if (chosenSkillType == Active)
            {
                compareChosenToPSkill(skillSlotCount, ActiveSkill[chosenSkillId]);
            }
            else if (chosenSkillType == Passive)
            {
                compareChosenToPSkill(skillSlotCount, PassiveSkill[chosenSkillId]);
            }
        }
        else if(chosenSkillId == -1)
        {
            Instantiate(otherSkill, p_skillSlot);
        }

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    void compareChosenToPSkill(int p_slotNum,GameObject skill)
    {
        bool already_has_skill = false;
        int i = 0;
        while (p_slotNum != i)
        {
            if (skill.name + Clone == p_skillSlot.GetChild(i).name)
            {
                p_skillSlot.GetChild(i).GetComponent<skill_interface>().level++;
                already_has_skill = true;
                break;
            }
            i++;
        }
        if (!already_has_skill)
        {
            Instantiate(skill, p_skillSlot);
        }
    }

    public void RewindBtn()
    {
        p_exp.InGame_Lv--;
        p_exp.max_exp -= exp_increase;
        p_exp.cur_exp = p_exp.max_exp / 2;
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
