
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUP_UI : MonoBehaviour
{
    [Header("��ų ���")]
    public List<SkillData> ASkillData; //��Ƽ�� ��ų�� ������
    public List<SkillData> PSkillData; //�нú� ��ų�� ������
    public List<SkillData> OSkillData; //�ν���Ʈ �� �� ���� ��ų�� ������ ���� ������

    public List<SkillData> ableAPSkill; //���ü� �ִ� Ap��ų

    public List<int> randomCodeIndexList;

    [Header("���õ� ��ư ����")]
    public SkillData ChosenSkillData;

    //�÷��̾�
    private PlayerInGameExp playerExp;
    private Transform playerSkillSlot; //�÷��̾��� ��ųĭ

    private Transform skillSlot; //UI�� ��ų��ư�� �� ���

    public int skillSlotAmount = 3; //������ ��ų������ ��
    public int APSkillAmount; //��Ƽ�� �нú� ��ų�� ����

    private float exp_increase = 5f;

    public Button ActBtn;

    private void Awake()
    {
        playerExp = GameManager.gameInstance.myPlayer.GetComponent<PlayerInGameExp>();
        playerSkillSlot = GameManager.gameInstance.myPlayer.transform.GetChild(1);
        skillSlot = transform.GetChild(1);
        skillSlotInit();
        ableAPSkill = new List<SkillData>();
        foreach (SkillData Askills in ASkillData)
        {
            ableAPSkill.Add(Askills);
        }
        foreach (SkillData Pskills in PSkillData)
        {
            ableAPSkill.Add(Pskills);
        }

        APSkillAmount = ableAPSkill.Count;
    }

    private void skillSlotInit()
    {
        skillSlot.GetChild(0).gameObject.SetActive(true);
        skillSlot.GetChild(1).gameObject.SetActive(true);
        skillSlot.GetChild(2).gameObject.SetActive(true);
        skillSlot.GetChild(0).gameObject.SetActive(false);
        skillSlot.GetChild(1).gameObject.SetActive(false);
        skillSlot.GetChild(2).gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Init();
    }
    private void OnDisable()
    {
        skillSlot.GetChild(0).gameObject.SetActive(false);
        skillSlot.GetChild(1).gameObject.SetActive(false);
        skillSlot.GetChild(2).gameObject.SetActive(false);
    }

    private void Init()
    {
        ChosenSkillData = null;
        randomCodeIndexList = new List<int>();
        ActBtn.interactable = false;
        InstantSkillSlot();
    }

    private void InstantSkillSlot()
    {
        if(APSkillAmount == 0)
        {
            skillSlot.GetChild(0).gameObject.SetActive(true);
            skillSlot.GetChild(0).GetComponent<SkillBtn>().SkillData = OSkillData[0];
            return;
        }


        if(APSkillAmount < skillSlotAmount)
        {
            skillSlotAmount = APSkillAmount;
        }
        
        for(int i =0; i<skillSlotAmount; i++)
        {
            SkillData randomSkill = RandomSkill();
            skillSlot.GetChild(i).gameObject.SetActive(true);

            Debug.Log("��ų������ ����");
            skillSlot.GetChild(i).GetComponent<SkillBtn>().SkillData = randomSkill;
        }
    }

    private SkillData RandomSkill()
    {
        int randomIndex = Random.Range(0, APSkillAmount);
        while(randomCodeIndexList.Contains(randomIndex) == true)
        {
            randomIndex = Random.Range(0, APSkillAmount);
        }

        randomCodeIndexList.Add(randomIndex);
        return ableAPSkill[randomIndex];
    }


    public void AcceptBtn()
    {
        if(ChosenSkillData == null)
        {
            Debug.Log("���޵� ��ų�����Ͱ� ����");
            return;
        }

        SkillInterface skillInPlayerSlot = FindSkillInPlayerSkill(ChosenSkillData);
        if (skillInPlayerSlot == null)
        {
            Debug.Log("�÷��̾� ���Կ��� ã������");
            return;
        }

        if(!skillInPlayerSlot.gameObject.activeSelf)
        {
            skillInPlayerSlot.gameObject.SetActive(true);
        }
        skillInPlayerSlot.level++;

        if(skillInPlayerSlot.skillData.skilltype == SkillType.Active && skillInPlayerSlot.level == 7)
        {
            ableAPSkill.Remove(ChosenSkillData);
            APSkillAmount -= 1;
        }
        if (skillInPlayerSlot.skillData.skilltype == SkillType.Passive && skillInPlayerSlot.level == 5)
        {
            ableAPSkill.Remove(ChosenSkillData);
            APSkillAmount -= 1;
        }

        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    private SkillInterface FindSkillInPlayerSkill(SkillData chosenSkillData)
    {
        for (int i = 0; i < playerSkillSlot.childCount; i++)
        {
            if (playerSkillSlot.GetChild(i).GetComponent<SkillInterface>().skillId == chosenSkillData.skillID)
            {
                return playerSkillSlot.GetChild(i).GetComponent<SkillInterface>();
            }
        }
        return null;
    }

    public void RewindBtn()
    {
        playerExp.InGameLv--;
        playerExp.maxExp -= exp_increase;
        playerExp.curExp = playerExp.maxExp / 2;
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

}
