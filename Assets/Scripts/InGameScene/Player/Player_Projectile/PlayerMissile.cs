using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerShoot
{
    private bool hashit = false;
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
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hashit)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //�ӽ÷� ���� ���߸� �������� �ջ�
            //���� ����� ���� hp�� �����ϰԲ� �ٲٱ�
            hashit = true;
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(missileDamage, gameObject);
            }
            
            ExplosionSplashDamage splashDamage = ObjectPool.poolInstance.GetProjPool(ProjPoolType.Player_SplashRange, transform.position, transform.rotation).GetComponent<ExplosionSplashDamage>();
            splashDamage.explosionRange = explosionRange;
            splashDamage.missileDamage = missileDamage;
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }
    }
}
