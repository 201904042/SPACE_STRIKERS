using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Troop : PlayerProjectile
{
    GameObject player;
    private float troopAtkDelay = 0;
    private Coroutine behaviorCoroutine;
    private bool isSkillEnd;
    private bool isAttack;
    private float normalSpeed;
    private float slowSpeed = 2f;
    private float slowPointY = -2f;
    protected override void ResetProj()
    {
        base.ResetProj();

        player = PlayerMain.Instance.gameObject;
        isSkillEnd = false;
        normalSpeed = speed; // 초기 속도를 저장합니다.

        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("드론 파라미터 세팅");
        isParameterSet = true;
        isShootingObj = true;
        speed = _projSpeed;
        normalSpeed = speed;

        if (_dmgRate == 0)
        {
            Debug.LogError("데미지가 설정되지 않음");
        }
        else
        {
            damageRate = _dmgRate;
        }

        if (_liveTime != 0)
        {
            liveTime = _liveTime;
            StartCoroutine(LiveTimer(liveTime));
        }

        behaviorCoroutine = StartCoroutine(TroopBehavior());
    }

    protected override IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        isSkillEnd = true;
    }

    private IEnumerator TroopBehavior()
    {
        while (true)
        {
            // 일정 위치(y = -2)에 도달하면 속도를 느리게 함
            if (transform.position.y >= slowPointY && !isSkillEnd)
            {
                transform.position += transform.up * slowSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.up * normalSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TroopBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }

        if (collision.CompareTag("Enemy_Projectile"))
        {
            EnemyBullet enemyBullet = collision.GetComponent<EnemyBullet>();
            EnemySplitBullet enemySplitBullet = collision.GetComponent<EnemySplitBullet>();
            if (enemyBullet != null || enemySplitBullet != null)
            {
                GameManager.Instance.Pool.ReleasePool(gameObject);
            }
        }
    }

    

    protected override void Update()
    {
        Debug.Log("자식 Update");
    }

    public int GetDamageRate()
    {
        return damageRate;
    }
}

    //private string rootPath;
    //private string curLevelPath;
    //public float slowYPos = -2f;
    //public float speed = 5f;
    //private int skillLevel;

    //private void Awake()
    //{
    //    rootPath = "Assets/Prefabs/Player/Player_UniqueSkill/player1/shooter_";
    //    palyerSpecialSkill = GameManager.Instance.myPlayer.GetComponent<PlayerSpecialSkill>();
    //}

    //private void OnEnable()
    //{
    //    startTimer = 0;
    //    fireTime = palyerSpecialSkill.specialFireTime;
    //    skillLevel = palyerSpecialSkill.IG_curPowerLevel;
    //    curLevelPath = ChangeShooterLevelPath(skillLevel);
    //    InstantShooter();
    //}

    //private void Update()
    //{

    //    if (transform.position.y > slowYPos) //속도 감소
    //    {
    //        speed = 0.2f;
    //    }
    //    startTimer += Time.deltaTime;
    //    if(startTimer > fireTime)
    //    {
    //        //특수공격 종료
    //        speed = 5f;
    //        palyerSpecialSkill.isSkillActivating = false;
    //    }
    //    transform.Translate(Vector3.up* speed * Time.deltaTime); //이동
    //}
    //private string ChangeShooterLevelPath(int shooter_level)
    //{
    //    switch (shooter_level)
    //    {
    //        case 1:
    //            return "lv1";
    //        case 2:
    //            return "lv2";
    //        case 3:
    //            return "lvMax";
    //        default:
    //            return "lvMax";
    //    }
    //}
    //private void InstantShooter()
    //{
    //    GameObject shooter = AssetDatabase.LoadAssetAtPath<GameObject>(rootPath + curLevelPath + ".prefab");
    //    if (shooter == null)
    //    {
    //        Debug.Log("load fail");
    //    }
    //    else
    //    {
    //        Instantiate(shooter, transform);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag =="Enemy_Projectile")
    //    {
    //        GameManager.Instance.Pool.ReleasePool(collision.gameObject);
    //    }

    //    if (collision.gameObject.tag == "TroopBorder")
    //    {
    //        GameManager.Instance.Pool.ReleasePool(gameObject);
    //    }

    //}
