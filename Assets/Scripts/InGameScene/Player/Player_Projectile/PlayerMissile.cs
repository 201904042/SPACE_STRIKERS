using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    public GameObject splashColliderObject;

    [Header("�⺻�̻��� ����")]
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
            //�ӽ÷� ���� ���߸� �������� �ջ�
            //���� ����� ���� hp�� �����ϰԲ� �ٲٱ�
            hasHit = true;
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(missileDamage, gameObject);
            }
            
            MissileSplash splashDamage = PoolManager.poolInstance.GetProj(ProjType.Player_SplashRange, transform.position, transform.rotation).GetComponent<MissileSplash>();
            splashDamage.explosionRange = explosionRange;
            splashDamage.missileDamage = missileDamage;
            PoolManager.poolInstance.ReleasePool(gameObject);
        }
    }
}
