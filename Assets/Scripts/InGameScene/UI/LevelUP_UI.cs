
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUP_UI : MonoBehaviour
{
    [Header("스킬 목록")]
    public List<SkillData> ASkillData; //액티브 스킬의 데이터
    public List<SkillData> PSkillData; //패시브 스킬의 데이터
    public List<SkillData> OSkillData; //인스턴트 힐 등 남은 스킬이 없을때 쓰일 데이터

    public List<SkillData> ableAPSkill; //나올수 있는 Ap스킬

    public List<int> randomCodeIndexList;

    [Header("선택된 버튼 정보")]
    public SkillData ChosenSkillData;

    //플레이어
    private PlayerInGameExp playerExp;
    private Transform playerSkillSlot; //플레이어의 스킬칸

    private Transform skillSlot; //UI의 스킬버튼이 들어갈 장소

    public int skillSlotAmount = 3; //생성될 스킬슬롯의 수
    public int APSkillAmount; //액티브 패시브 스킬의 개수

    private float exp_increase = 5f;

    public Button ActBtn;

    private void Awake()
    {
        playerExp = GameManager.Instance.myPlayer.GetComponent<PlayerInGameExp>();
        playerSkillSlot = GameManager.Instance.myPlayer.transform.GetChild(1);
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

            Debug.Log("스킬데이터 전달");
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
        if (ChosenSkillData == null)
        {
            Debug.Log("전달된 스킬데이터가 없음");
            return;
        }

        //선택된 스킬을 스킬매니저를 통해 추가

        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
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
