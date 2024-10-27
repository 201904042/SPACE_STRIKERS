using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialSkill : MonoBehaviour
{
    Dictionary<int, UniqueSkill> uniqueSkills;

    private PlayerStat pStat => PlayerMain.pStat;
    [HideInInspector]
    public int playerId => pStat.curPlayerID;
    public int powLv => pStat.IG_curPowerLevel;
  
    public bool isSkillActivating; //현재 스패셜 공격이 실행중인가

    public void Init()
    {
        isSkillActivating = false;
        uniqueSkills = new Dictionary<int, UniqueSkill>();
        SetUniques();
    }

    private void SetUniques()
    {
        Unique_Char1 char1 = new Unique_Char1();
        uniqueSkills.Add(char1.useCharCode, char1);
    }

    public UniqueSkill GetUniqueSkill(int charId)
    {
        UniqueSkill targetSkill;
        uniqueSkills.TryGetValue(charId, out targetSkill);
        if (targetSkill == null) {
            Debug.LogError("해당 캐릭터의 id가 설정된 스킬이 없음");
        }

        return targetSkill;

    }

    public void SpecialFire() //컨트롤러의 키입력함수에 사용
    {
        Debug.Log($"{pStat.curPlayerID}의 스페셜 스킬");

        UniqueSkill targetSkill = GetUniqueSkill(pStat.curPlayerID);
        targetSkill.ActivateSkill();
        //스킬 활성화시 파워레벨 초기화

    }

    //public IEnumerator character1SpecialOn()
    //{

    //}



    //private void BomberSpecial()
    //{
    //    GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Spcial_Player2, transform.position, transform.rotation);
    //}

    //private void TankerSpecial()
    //{
    //    isSkillActivating = true;
    //    PlayerShield shield = GameObject.Find("shield").GetComponent<PlayerShield>();
    //    //shield.shieldCurNum = shield.shieldMaxNum;
    //    //shield.ShieldColorChange();
    //    //shield.shieldIsActive = true;

    //    GameObject field = GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Spcial_Player3, transform.position,transform.rotation);
    //    field.transform.SetParent(transform);

    //    if (IG_curPowerLevel == 1)
    //    {
    //        field.transform.localScale = new Vector3(7f, 7f, 7f);
    //        specialFireTime = 5;
    //    }
    //    else if (IG_curPowerLevel == 2)
    //    {
    //        field.transform.localScale = new Vector3(10, 10f, 10f);
    //        specialFireTime = 7;
    //    }
    //    else if (IG_curPowerLevel == 3)
    //    {
    //        field.transform.localScale = new Vector3(15, 15f, 15f);
    //        specialFireTime = 10;
    //    }

    //    StartCoroutine(Bomber_End(specialFireTime, field));

    //}

    //private IEnumerator Bomber_End(float timer, GameObject field)
    //{
    //    yield return new WaitForSeconds(timer);
    //    GameManager.Instance.Pool.ReleasePool(field);
    //}

    //private void SplashSpecial()
    //{
    //    isSkillActivating = true;
    //    int fire_Num = 0;
    //    if (IG_curPowerLevel == 1)
    //    {
    //        fire_Num = 10;
    //    }
    //    else if (IG_curPowerLevel == 2)
    //    {
    //        fire_Num = 25;
    //    }
    //    else if (IG_curPowerLevel == 3)
    //    {
    //        fire_Num = 30;
    //    }

    //    StartCoroutine(SplashActivate(fire_Num));
    //}

    //private IEnumerator SplashActivate(int num)
    //{
    //    while (num!=0)
    //    {
    //        GameManager.Instance.Pool.GetPlayerProj(PlayerProjType.Spcial_Player4, transform.position, transform.rotation);
    //        num--;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    isSkillActivating = false;
    //}

    //UI 버튼
    public void btn1()
    {
        //SpecialFire(pStat.curPlayerID);
    }

    public void btn2()
    {
        //SpecialFire(pStat.curPlayerID);
    }

    public void btn3()
    {
        //SpecialFire(pStat.curPlayerID);
    }

}
