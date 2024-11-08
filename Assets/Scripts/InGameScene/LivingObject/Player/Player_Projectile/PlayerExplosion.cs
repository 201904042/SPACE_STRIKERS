using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerExplosion : PlayerProjectile
{
    // �ʿ� ���� : ������, �����ð�, ũ��, ƽ
    public float tikDelay;
    protected override void Awake()
    {
         base.Awake();
    }

    protected override void Update()
    {
        //�θ� ������Ʈ ���� ����
    }

    public override void SetAddParameter(float value1, float value2 = 0, float value3 = 0, float value4 = 0)
    {
        base.SetAddParameter(value1, value2, value3, value4);
        if(value1 != 0) //tik ����
        {
            tikDelay = value1;
            isHitOnce = false;
        }
        else
        {
            isHitOnce = true ;
        }
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
                damaging = StartCoroutine(AreaDamageLogic(tikDelay));
            }
        }
    }
}

