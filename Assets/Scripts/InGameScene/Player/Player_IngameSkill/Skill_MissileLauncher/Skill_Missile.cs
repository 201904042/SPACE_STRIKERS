using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_Missile : PlayerShoot
{
    private bool hashit = false;
    public GameObject splashColliderObject;
    private float missileDamage;
    private float playerStatDamage;
    public float missileDamageRate;
    public float explosionRange;
    private float missileSpeed;

    protected override void Awake()
    {
        base.Awake();
        splashColliderObject = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_InGameSkill/Proj/skill_missileSplash.prefab");
        missileDamageRate = 1.5f;
        explosionRange = 1.0f;
        playerStatDamage = playerStat.damage;
        missileSpeed = 10f;
    }

    private void Update()
    {
        if (!isFirstSet)
        {
            missileDamage = playerStatDamage * missileDamageRate;
            isFirstSet = true;
        }

        transform.position += transform.up * missileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //임시로 적을 맞추면 데미지를 합산
            //적을 만들면 적의 hp를 감소하게끔 바꾸기
            hashit = true;
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(missileDamage, gameObject);
            }

            ExplosionSplashDamage splashDamage = Instantiate(splashColliderObject, transform.position, transform.rotation).GetComponent<ExplosionSplashDamage>();
            splashDamage.explosionRange = explosionRange;
            splashDamage.missileDamage = missileDamage;
            Destroy(gameObject);
        }
    }
}
