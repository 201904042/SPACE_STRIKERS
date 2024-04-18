using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelUP_UI : MonoBehaviour
{
    [Header("��ų ��ư")]
    public GameObject skillBtn;
    [Header("��ų ���")]
    public GameObject[] ActiveSkill;
    public GameObject[] PassiveSkill;
    public GameObject otherSkill;
    [Header("���õ� ��ư ����")]
    public int chosenSkillId;
    public string chosenSkillType;

    private GameObject player;
    private Player_InGame_Exp p_exp;
    private Transform p_skillSlot; //�÷��̾��� ��ųĭ
    private Transform skillSlot; //UI�� ��ų��ư�� �� ���
    private bool[] levelMax;
    private Button[] buttons;

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
        Array.Fill(levelMax, false); //levelMax�迭�� ��� �迭 false�� �ʱ�ȭ
        chosenSkillId = -1;
        for (int i = 0; i < levelMax.Length; i++)
        {
            levelMax[i] = is_skillLevelMax(i);
        }
        instantSkillSlot();
        for(int i = 0; i<skillSlotNum; i++)
        {
            buttons[i] = skillSlot.GetChild(i).GetComponent<Button>();
        }
    }
    private void Update()
    {
        
    }
    private void instantSkillSlot()
    {
        if (countFalseInLevelMax() < skillSlotNum)
        {
            skillSlotNum = countFalseInLevelMax();
        }

        if (skillSlotNum == 0) //extra ��ų ����
        {
            SetSkillSlot(-1);
        }
        else //active, passive ��ų ���� ����
        {
            HashSet<int> setid = new HashSet<int>();
            while (setid.Count < skillSlotNum)
            {
                
                int randomNum = UnityEngine.Random.Range(0, skillNum);
                if (!setid.Contains(randomNum) && !levelMax[randomNum]) //�ߺ��� ���� ����
                {
                    setid.Add(randomNum);
                }
            }
            int[] skillId = new int[skillSlotNum];
            setid.CopyTo(skillId);

            for (int i = 0; i < skillSlotNum; i++) //UI�� ��ų ��ư�� ���� ����
            {
                SetSkillSlot(skillId[i]);
            }
        }
    }

    private int countFalseInLevelMax() //������ �ƴ� A,P��ų�� ����
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
        if (i < ActiveSkill.Length) //Active ��ų�� ���
        {
            return checkSkillLvLoop(s_interface, p_skillSlotCount, ActiveSkill[i]);
        }
        else if (i >= ActiveSkill.Length) //Passive ��ų�� ���
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
                { //�÷��̾��� ��ų�� 5���� �̻��̶�� true
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
        else if (i < ActiveSkill.Length) //Active ��ų�� ���
        {
            checkSkillSlot(skillSlotCount, i, Active, ActiveSkill[i]);
        }
        else if (i >= ActiveSkill.Length) //Passive ��ų�� ���
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
