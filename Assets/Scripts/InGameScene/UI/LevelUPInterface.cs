using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class LevelUPInterface : UIInterface
{
    private Transform skillSlot; //UI�� ��ų��ư�� �� ���
    public Transform buttons;
    public Button selectBtn;
    public Button returnBtn;

    [Header("��ų ���")]
    
    public List<InGameSkill> activatableSkill; //��ų�Ŵ����� Ingame��ų ����Ʈ ����

    [Header("���õ� ��ư ����")]
    public InGameSkill selectedSkill;

    //�÷��̾�
    private PlayerInGameExp playerExp;
    private PlayerSkillManager psManager;

    


    public int maxSkillSlotAmount; //������ ��ų������ ��

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

        selectBtn.onClick.RemoveAllListeners();
        returnBtn.onClick.RemoveAllListeners();
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
            //O��ų ������� �� ��ų���� ����
            skillSlotCount = SetSkillSlot(OSkills, skillSlotCount);
            return;
        }

        //AP��ų ���� ����
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
            skillSlotBtn.onClick.RemoveAllListeners();
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
        Debug.Log($"{i}��° ��ư ���� , ��ų�� {selectedSkill.SkillCode}");
        selectBtn.interactable = true;
    }

    public void SelectBtn()
    {
        if (selectedSkill == null)
        {
            Debug.Log("���޵� ��ų�����Ͱ� ����");
            return;
        }

        //���õ� ��ų�� ��ų�Ŵ����� ���� �߰�
        GameManager.Instance.psManager.AddSkill(selectedSkill);

        Time.timeScale = 1.0f;
        CloseInterface();
    }


    public void ReturnBtn()
    {
        //todo -> �̺κ��� ������ �Ŵ������� �ű��
        playerExp.InGameLv--;
        playerExp.maxExp -= exp_increase;
        playerExp.curExp = playerExp.maxExp / 2;
        Time.timeScale = 1.0f;
        CloseInterface();
    }

}
