using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Troop : PlayerProjectile
{
    GameObject player;
    private Coroutine behaviorCoroutine;
    private bool isSkillEnd;
    private bool isAttack;
    private float normalSpeed;
    private float slowSpeed = 0.1f;
    private float slowPointY = -2f;
    protected override void ResetProj()
    {
        base.ResetProj();

        player = PlayerMain.Instance.gameObject;
        isSkillEnd = false;
        normalSpeed = speed; // �ʱ� �ӵ��� �����մϴ�.

        if (behaviorCoroutine != null)
        {
            StopCoroutine(behaviorCoroutine);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //���Ϸ� ���� ����
        Transform instantTransform = transform.GetChild(0);
        for (int i = instantTransform.childCount - 1; i >= 0; i--)
        {
            Destroy(instantTransform.GetChild(i).gameObject);
        }

    }

    public override void SetProjParameter(int _projSpeed, int _dmgRate, float _liveTime, float _range)
    {
        ResetProj();

        Debug.Log("��� �Ķ���� ����");
        isParameterSet = true;
        isShootingObj = true;
        speed = _projSpeed;
        normalSpeed = speed;

        if (_dmgRate == 0)
        {
            Debug.LogError("�������� �������� ����");
        }
        else
        {
            damageRate = _dmgRate;
        }

        if (_liveTime != 0)
        {
            liveTime = _liveTime;
            StartCoroutine(LiveTimer(liveTime));
        }

        behaviorCoroutine = StartCoroutine(TroopBehavior());
    }

    protected override IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        isSkillEnd = true;
    }

    private IEnumerator TroopBehavior()
    {
        while (true)
        {
            // ���� ��ġ(y = -2)�� �����ϸ� �ӵ��� ������ ��
            if (transform.position.y >= slowPointY && !isSkillEnd)
            {
                transform.position += transform.up * slowSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += transform.up * normalSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TroopBorder")
        {
            GameManager.Game.Pool.ReleasePool(gameObject);
        }

        if (collision.CompareTag("Enemy_Projectile"))
        {
            EnemyBullet enemyBullet = collision.GetComponent<EnemyBullet>();
            EnemySplitBullet enemySplitBullet = collision.GetComponent<EnemySplitBullet>();
            if (enemyBullet != null || enemySplitBullet != null)
            {
                GameManager.Game.Pool.ReleasePool(collision.gameObject);
            }
        }
    }

    

    protected override void Update()
    {
        //������Ʈ�� ������� ����
    }

    public int GetDamageRate()
    {
        return damageRate;
    }
}