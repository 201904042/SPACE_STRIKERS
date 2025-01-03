using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    public GameObject splashColliderObject;

    [Header("기본미사일 스텟")]
    [SerializeField]
    private float missileDamage;
    [SerializeField]
    private float playerStatDamage;
    [SerializeField]
    private float missileDamageRate;
    [SerializeField]
    private float explosionRange;

    protected override void Awake()
    {
        base.Awake();
        splashColliderObject = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player/Player_Bullets/splashRange.prefab");
        missileDamageRate = 1.5f;
        explosionRange = 1.0f;
        playerStatDamage = playerStat.damage;

        missileDamage = playerStatDamage * missileDamageRate;
    }

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasHit)
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
            
            MissileSplash splashDamage = Managers.Instance.Pool.GetProj(ProjType.Player_SplashRange, transform.position, transform.rotation).GetComponent<MissileSplash>();
            splashDamage.explosionRange = explosionRange;
            splashDamage.missileDamage = missileDamage;
            Managers.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
