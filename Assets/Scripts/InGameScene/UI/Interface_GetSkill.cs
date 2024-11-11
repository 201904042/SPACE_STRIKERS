using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// todo -> ��ų Ÿ�Ժ� ���ٲٱ�
// ��ų�� ������ �� ���������� 0 -> 1�̷������� �ٲٱ�
public class Interface_GetSkill : UIInterface
{
    private Transform skillSlot; //UI�� ��ų��ư�� �� ���

    public Transform buttons;
    public Button selectBtn;
    public Button returnBtn;

    public List<InGameSkill> activatableSkill; //��ų�Ŵ����� Ingame��ų ����Ʈ ����

    [Header("���õ� ��ư ����")]
    public InGameSkill selectedSkill;

    public int maxSkillSlotAmount; //������ ��ų������ ��

    protected override void Awake()
    {
        base.Awake();
    }


    public override void SetComponent()
    {
        base.SetComponent();
        skillSlot = transform.GetChild(3);
        buttons = transform.GetChild(4);
        selectBtn = buttons.GetChild(0).GetComponent<Button>();
        returnBtn = buttons.GetChild(1).GetComponent<Button>();

        selectBtn.onClick.RemoveAllListeners();
        returnBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(SelectBtn);
        returnBtn.onClick.AddListener(ReturnBtn);
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
        if (PlayerMain.Instance == null)
        {
            return;
        }
        maxSkillSlotAmount = skillSlot.childCount;
        ResetInterface();
        InstantSkillSlot();
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
        activatableSkill = PlayerMain.pSkill.ingameSkillList;
        List<InGameSkill> apSkillList = new List<InGameSkill>();
        List<InGameSkill> extraSkillList = new List<InGameSkill>();
        int skillSlotCount = maxSkillSlotAmount;

        foreach (InGameSkill skill in activatableSkill)
        {
            if (skill.type == SkillType.Extra)
            {
                extraSkillList.Add(skill);
            }
            else
            {
                apSkillList.Add(skill);
            }
        }

       // apSkillList.Clear(); //extra test��
        if (apSkillList.Count == 0)
        {
            //extra��ų ������� �� ��ų���� ����
            skillSlotCount = SetSkillSlot(extraSkillList, skillSlotCount);
            return;
        }

        //AP��ų ���� ����
        skillSlotCount = SetSkillSlot(apSkillList, skillSlotCount);
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
            skillSlotBtn.GetComponent<SelectSkillBtn>().SetSkillData(skillList[randomIndex]);
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
        
        selectedSkill = skillSlot.GetChild(i).GetComponent<SelectSkillBtn>().GetSkillData();
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
        PlayerMain.pSkill.AddSkill(selectedSkill);
        GameManager.Game.Restart();
        CloseInterface();
    }


    public void ReturnBtn()
    {
        PlayerStat pStat = PlayerMain.pStat;
        pStat.IG_Level -= 1;
        pStat.IG_MaxExp = pStat.MaxExpInstance;
        pStat.CurExp = pStat.IG_MaxExp / 2;
        GameManager.Game.Restart();
        CloseInterface();
    }

}