using System.Collections;
using UnityEngine;

public class PlayerSpecialSkill : MonoBehaviour
{
    private PlayerStat pStat => PlayerMain.pStat;
    [HideInInspector]
    public int playerId => pStat.curPlayerID;
    public int powLv => pStat.powerLevel;
  
    public bool isSkillActivating; //현재 스패셜 공격이 실행중인가

    public void Init()
    {
        isSkillActivating = false;
    }

    //private void SpecialFire(int id ) //컨트롤러의 키입력함수에 사용
    //{
    //    Debug.Log($"{id}의 스페셜 스킬");
    //    switch (id) //플레이어의 아이디에 따라 플레이어 별 스페셜스킬 활성
    //    {
    //        case 101:
    //            BalanceSpecial(); break;
    //        case 102:
    //            BomberSpecial(); break;
    //        case 103:
    //            TankerSpecial(); break;
    //        case 104:
    //            SplashSpecial(); break;
    //        default:
    //            Debug.Log("can't find id"); break;
    //    }

    //    //스킬 활성화시 파워레벨 초기화
    //    powerLevel = 0;
    //    powerIncrease = 0;
    //    isSkillActivating = true;
    //}

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

    //    if (powerLevel == 1)
    //    {
    //        field.transform.localScale = new Vector3(7f, 7f, 7f);
    //        specialFireTime = 5;
    //    }
    //    else if (powerLevel == 2)
    //    {
    //        field.transform.localScale = new Vector3(10, 10f, 10f);
    //        specialFireTime = 7;
    //    }
    //    else if (powerLevel == 3)
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
    //    if (powerLevel == 1)
    //    {
    //        fire_Num = 10;
    //    }
    //    else if (powerLevel == 2)
    //    {
    //        fire_Num = 25;
    //    }
    //    else if (powerLevel == 3)
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
