using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerExplosion : PlayerProjectile
{

    protected override void Update()
    {
        //�θ� ������Ʈ ���� ����
    }

    /// <summary>
    /// t: �������� �����ϸ� ����Ʈ���� ����  f:������ �����ص� ����Ʈ���� ���ž���
    /// </summary>
    /// <param name="tf"></param>
    public void OnHitOnce(bool tf)
    {
        isHitOnce = tf;
    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        base.SetProjParameter(_projSpeed, _dmgRate, _liveTime , _range);
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;
        isShootingObj = true; //true�� ���� ������ �÷��̾� ��ġ�� �������� ���� speed�� 0�� �ǵ��� -> ������ ��ġ�� �״�� ���̺�Ÿ�Ӹ�ŭ ����
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

        if (isHitOnce)
        {
            MultiEnemyDamage();
        }
        else
        {
            if (damaging == null)
            {
                damaging = StartCoroutine(AreaDamageLogic(0.5f));
            }
        }
        

    }
}

