using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Skill_Explosion : PlayerProjectile
{

    protected override void Update()
    {

    }


    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;

        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //�⺻ ���� ������ ����. ���������� ���� ������
        liveTime = _liveTime; //������ �߻�ü�� �� �� �ð��Ŀ� �ڵ����� �ı���
        StartCoroutine(LiveTimer(liveTime));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TriggedEnemy(collision);
        }
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);

        MultiEnemyDamage();
    }
}

