using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    //�̰� ������ �Ѿ� 100% , �п��Ѿ� 200%, ������ 150%
    protected const int BulletDmgRate = 100;
    protected const int SplitBulletDmgRate = 200;
    protected const int LaserDmgRate = 150;

    protected const int BulletSpeed = 5;
    protected const int SplitBulletSpeed = 3;
    protected const int LaserSpeed = 0;

    //���� �ʿ� �Ķ���� = ��������Ʈ�� ������ ����, �ӵ�, �ߵ��ð�, ũ��
    public int enemyDmg;

    protected virtual void Awake()
    {
        projScaleInstance = transform.localScale; //�߻�ü�� ���� ũ�⸦ ����

        ResetProj();
    }

    protected virtual void OnEnable()
    {

    }


    protected virtual void OnDisable()
    {
        //��Ȱ��ȭ �� �ʱ�ȭ
        ResetProj();
    }

    protected virtual void Update()
    {
        if (!isParameterSet)
        {
            //Debug.Log("�Ķ���Ͱ� �������� ����");
            return;
        }

        MoveUp();
    }

    protected override void ResetProj()
    {
        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0;

        if (activated != null)
        {
            StopCoroutine(activated);
            activated = null;
        }

        transform.localScale = projScaleInstance;

        isParameterSet = false;
    }

    public virtual void SetProjParameter(int _dmgRate, float _range = 0) //���ǵ�, ������������, �����ð�, ũ��
    {
        enemyDmg = _dmgRate;
        finalDamage = (enemyDmg * defaultDmgRate / 100) + (enemyDmg * damageRate / 100);  //damageRate = �߻�ü�� ������ + ������ ������

        //�̰͵� �������� �������� ���� ���ݾ� Ŀ���� �ҵ�, Ȥ�� ������ ũ�⸦ �����ϰų�
        if (_range == 0)
        {
            range = 1;
        }
        else
        {
            range = _range;
            transform.localScale = projScaleInstance * range;
        }

        isParameterSet = true;
    }

    public virtual void SetSplitCount(int count)
    {
        //�ӽ�
    }

    public virtual void SetLaser(GameObject _startObj, bool isAim, float angle = 0, float _laserTime = 1, float chargingTime = 1f, float laserWidthRate = 1)
    {
        //�ӽ�
    }

    protected virtual IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);

        GameManager.Game.Pool.ReleasePool(gameObject);
    }


    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerMain.pStat.PlayerDamaged(finalDamage, gameObject);
            GameManager.Game.Pool.ReleasePool(gameObject);
        }

        if (collision.transform.tag == "BulletBorder")
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }
    }
}
