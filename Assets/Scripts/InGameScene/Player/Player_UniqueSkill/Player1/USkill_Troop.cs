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
    private float slowSpeed = 0.1f;
    private float slowPointY = -2f;
    protected override void ResetProj()
    {
        base.ResetProj();

        player = PlayerMain.Instance.gameObject;
        isSkillEnd = false;
        normalSpeed = speed; // �ʱ� �ӵ��� �����մϴ�.

        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //���Ϸ� ���� ����
        Transform instantTransform = transform.GetChild(0);
        for (int i = instantTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(instantTransform.GetChild(i).gameObject);
        }

    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        ResetProj();

        Debug.Log("��� �Ķ���� ����");
        isParameterSet = true;
        isShootingObj = true;
        speed = _projSpeed;
        normalSpeed = speed;

        if (_dmgRate == 0)
        {
            Debug.LogError("�������� �������� ����");
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
            // ���� ��ġ(y = -2)�� �����ϸ� �ӵ��� ������ ��
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
                GameManager.Instance.Pool.ReleasePool(collision.gameObject);
            }
        }
    }

    

    protected override void Update()
    {
        //������Ʈ�� ������� ����
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

    //    if (transform.position.y > slowYPos) //�ӵ� ����
    //    {
    //        speed = 0.2f;
    //    }
    //    startTimer += Time.deltaTime;
    //    if(startTimer > fireTime)
    //    {
    //        //Ư������ ����
    //        speed = 5f;
    //        palyerSpecialSkill.isSkillActivating = false;
    //    }
    //    transform.Translate(Vector3.up* speed * Time.deltaTime); //�̵�
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
