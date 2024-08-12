using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class skill_Missile : PlayerShoot
{
    public GameObject splashColliderObject;
    public float missileDamage;
    private float playerStatDamage;
    public float missileDamageRate;
    public float explosionRange;
    private float missileSpeed;

    private Skill_MissileLauncher missileLauncher;

    protected override void Awake()
    {
        base.Awake();
        splashColliderObject = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_missileSplash.prefab");
        missileLauncher = GameObject.Find("skill_MissileLauncher").GetComponent<Skill_MissileLauncher>();
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
        explosionRange = missileLauncher.explosionRange;
        missileDamageRate = missileLauncher.damageRate;

    }

    private void Update()
    {
        transform.position += transform.up * missileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //임시로 적을 맞추면 데미지를 합산
            //적을 만들면 적의 hp를 감소하게끔 바꾸기
            hasHit = true;
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(missileDamage, gameObject);
            }

            ExplosionSplashDamage splashDamage = PoolManager.poolInstance.GetSkill(SkillProjType.Skill_Splash, transform.position, transform.rotation).GetComponent<ExplosionSplashDamage>();
            splashDamage.SetVariable(explosionRange, missileDamage);
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
    }
}
