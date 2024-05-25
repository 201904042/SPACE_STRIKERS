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
    public GameObject[] activeSkill;
    public GameObject[] passiveSkill;
    public GameObject otherSkill;
    [Header("���õ� ��ư ����")]
    public int chosenSkillId;
    public string chosenSkillType;

    private GameObject player;
    private PlayerInGameExp playerExp;
    private Transform playerSkillSlot; //�÷��̾��� ��ųĭ
    private Transform skillSlot; //UI�� ��ų��ư�� �� ���
    private bool[] levelMax;


    private int skillSlotNum = 3;
    private int skillNum;

    private string active = "active";
    private string passive = "passive";
    private string extra = "extra";
    private string clone = "(Clone)";

    private float exp_increase = 5f;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerExp = player.GetComponent<PlayerInGameExp>();
        playerSkillSlot = player.transform.GetChild(1);
        skillSlot = transform.GetChild(1);
        skillNum = activeSkill.Length + passiveSkill.Length;
        levelMax = new bool[skillNum];
        Array.Fill(levelMax, false); //levelMax�迭�� ��� �迭 false�� �ʱ�ȭ
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

        if (skillSlotNum == 0) //extra ��ų ����
        {
            SetSkillSlot(-1);
        }
        else //active, passive ��ų ���� ����
        {
            HashSet<int> setId = new HashSet<int>();
            while (setId.Count < skillSlotNum)
            {
                
                int randomNum = UnityEngine.Random.Range(0, skillNum);
                if (!setId.Contains(randomNum) && !levelMax[randomNum]) //�ߺ��� ���� ����
                {
                    setId.Add(randomNum);
                }
            }
            int[] skillId = new int[skillSlotNum];
            setId.CopyTo(skillId);

            for (int i = 0; i < skillSlotNum; i++) //UI�� ��ų ��ư�� ���� ����
            {
                SetSkillSlot(skillId[i]);
            }
        }
    }

    private int CountFalseInLevelMax() //������ �ƴ� A,P��ų�� ����
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
        if (i < activeSkill.Length) //active ��ų�� ���
        {
            return checkSkillLvLoop(s_interface, playerSkillSlotCount, activeSkill[i]);
        }
        else if (i >= activeSkill.Length) //passive ��ų�� ���
        {
            i -= activeSkill.Length;
            return checkSkillLvLoop(s_interface, playerSkillSlotCount, passiveSkill[i]);
        }
        return false;
    }

    private bool checkSkillLvLoop(SkillInterface s_interf, float playerSlotNum, GameObject skill)
    {
        int j = 0;
        while (playerSlotNum != j)
        {
            if (skill.name + clone == playerSkillSlot.GetChild(j).name)
            {
                s_interf = playerSkillSlot.GetChild(j).GetComponent<SkillInterface>();
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
        int skillSlotCount = playerSkillSlot.childCount;
        if(i == -1)
        {
            SkillInterface s_interface =  otherSkill.GetComponent<SkillInterface>();
            MakeSkillSlot(i, extra, s_interface);
        }
        else if (i < activeSkill.Length) //active ��ų�� ���
        {
            CheckSkillSlot(skillSlotCount, i, active, activeSkill[i]);
        }
        else if (i >= activeSkill.Length) //passive ��ų�� ���
        {
            i -= activeSkill.Length;
            CheckSkillSlot(skillSlotCount, i, passive, passiveSkill[i]);
        }
    }

    private void CheckSkillSlot(int skillSlotnum,int s_id, string s_type,GameObject skill)
    {
        SkillInterface s_interface = skill.GetComponent<SkillInterface>();
        int j = 0;
        while (skillSlotnum != j)
        {
            if (skill.name + clone == playerSkillSlot.GetChild(j).name)
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
            if (skill.name + clone == playerSkillSlot.GetChild(i).name)
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

            if (chosenSkillType == active)
            {
                FindChosenSkillInPlayerSkill(skillSlotCount, activeSkill[chosenSkillId]);
            }
            else if (chosenSkillType == passive)
            {
                FindChosenSkillInPlayerSkill(skillSlotCount, passiveSkill[chosenSkillId]);
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
