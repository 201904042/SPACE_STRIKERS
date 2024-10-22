using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class skill_Missile : PlayerProjectile
{
    public float missileDamage;
    private float playerStatDamage;
    public float missileDamageRate;
    public float explosionRange;
    private float missileSpeed;


    protected override void Awake()
    {
        base.Awake();
        
        missileDamageRate = 1.5f;
        explosionRange = 1.0f;
        playerStatDamage = playerStat.damage;
        missileSpeed = 10f;
    }

    //protected override void OnEnable()
    //{
    //    Init();
    //}

    //protected override void Init()
    //{
    //    base.Init();
    //    missileDamage = playerStatDamage * missileDamageRate;
    //}


    public void SetParameter()
    {

    }


    private void Update()
    {
        transform.position += transform.up * missileSpeed * Time.deltaTime;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (isHitOnce)
    //    {
    //        return;
    //    }
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        //�ӽ÷� ���� ���߸� �������� �ջ�
    //        //���� ����� ���� hp�� �����ϰԲ� �ٲٱ�
    //        isHitOnce = true;
    //        if (collision.GetComponent<EnemyObject>() != null)
    //        {
    //            collision.GetComponent<EnemyObject>().EnemyDamaged(missileDamage, gameObject);
    //        }

    //        MissileSplash splashDamage = GameManager.Instance.Pool.GetSkill(SkillProjType.Skill_Splash, transform.position, transform.rotation).GetComponent<MissileSplash>();
    //        splashDamage.SetVariable(explosionRange, missileDamage);
    //        GameManager.Instance.Pool.ReleasePool(gameObject);
    //    }
    //}
}
