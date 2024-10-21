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

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        missileDamage = playerStatDamage * missileDamageRate;
    }


    public void SetParameter()
    {

    }


    private void Update()
    {
        transform.position += transform.up * missileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlreadyHit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //임시로 적을 맞추면 데미지를 합산
            //적을 만들면 적의 hp를 감소하게끔 바꾸기
            isAlreadyHit = true;
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(missileDamage, gameObject);
            }

            MissileSplash splashDamage = GameManager.Instance.Pool.GetSkill(SkillProjType.Skill_Splash, transform.position, transform.rotation).GetComponent<MissileSplash>();
            splashDamage.SetVariable(explosionRange, missileDamage);
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
