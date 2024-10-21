using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerProjectile : MonoBehaviour
{
    public PlayerStat playerStat; //�÷��̾��� �������� �ҷ��� ����
    protected bool isParameterSet; //�Ķ���Ͱ� ������? �����Ǿ�� ������
    protected bool isAlreadyHit; //���ÿ� ������ü�� �浹�ϰ� �ɰ�� �Ѱ��� ��ü�� ������ó���� �ϵ��� ����
    protected int finalDamage; //�÷��̾��� ���ݰ� �߻�ü�� ������������ ���Ͽ� ���������� ������ ������

    //�����ϴ� ��ũ��Ʈ���� ����(set) ����
    protected int damageRate;
    protected int speed;
    protected float liveTime;
    protected float range;

    

    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        isParameterSet = false;
        isAlreadyHit = false;
    }

    protected virtual void OnDisable()
    {
        //��Ȱ��ȭ �� �ʱ�ȭ
        ResetProj();
        
    }

    protected virtual void ResetProj()
    {
        isParameterSet = false;
        isAlreadyHit = false;
        finalDamage = 0;

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
    }

    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    public virtual void SetProjParameter(int projSpeed,int dmgRate, float liveTime, float range)
    {
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;
    }

    public virtual void SetAddParameter(float value1, float value2 = 0, float value3 = 0)
    {
        Debug.Log("���� �Ķ���� ����");
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }
}
