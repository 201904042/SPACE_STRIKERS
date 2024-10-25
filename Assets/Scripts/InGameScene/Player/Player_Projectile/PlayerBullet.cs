using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerBullet : PlayerProjectile
{
    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;

        isShootingObj = true;
        speed = _projSpeed;
        damageRate = _dmgRate;
        finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //�⺻ ���� ������ ����. ���������� ���� ������
    }

    protected override void TriggedEnemy(Collider2D collision)
    {
        base.TriggedEnemy(collision);
        SingleEnemyDamage();
    }

}
