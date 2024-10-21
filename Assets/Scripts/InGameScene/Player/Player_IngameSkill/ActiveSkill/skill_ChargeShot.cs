using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Android;

public class Skill_ChargeShot : PlayerProjectile
{
    private List<GameObject> hittedList;

    public bool isPenetrate;
    public int penetrateCount;

    protected override void Awake()
    {
        base.Awake();
        isAlreadyHit = false;
    }

    protected override void ResetProj()
    {
        base.ResetProj();
        isPenetrate = false;
        penetrateCount = 0;
    }


    public override void SetProjParameter(int projSpeed, int dmgRate, float liveTime, float range)
    {
        base.SetProjParameter(projSpeed, dmgRate,  liveTime, range);
        isPenetrate = false;
        penetrateCount = 0;

        speed = projSpeed;
        damageRate = dmgRate;
        this.range = range;

        finalDamage = (int)playerStat.damage * damageRate / 100;
    }

    public override void SetAddParameter(float value1, float value2 = 0, float value3 = 0)
    {
        base.SetAddParameter(value1, value2, value3);
        isPenetrate = true;
        penetrateCount = (int)value1;
    }


    private void Update()
    {
        //todo -> 코루틴으로 작동?
        MoveUp();
    }

    //todo -> 트리거부분도 개선해볼것
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyObject>() != null)
            {
                collision.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject);
            }
            if (!isPenetrate)
            {
                GameManager.Instance.Pool.ReleasePool(gameObject);
            }
        }
    }
}
