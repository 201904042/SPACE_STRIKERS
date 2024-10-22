using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerProjectile : MonoBehaviour
{
    public PlayerStat playerStat; //�÷��̾��� �������� �ҷ��� ����
    protected bool isParameterSet; //�Ķ���Ͱ� ������? �����Ǿ�� ������ : default =false

    //protected bool isSlow;
    //protected int isSlowCount;
    //protected bool isSlowExtraDamage;
    //protected int slowExtraDamageRate;

    //protected bool isPenetrate;
    //protected int penetrateCount;
    //protected int damageCount;

    //protected bool isCycleDamage; //������ �������� �ִ� ���� �ΰ� = ���ǽ�ų
    //protected int cycleRate;

    //protected bool isShootable; //�ش� ��ų�� �߻� ������ ��ų�ΰ�

    //protected bool isDrone;
    //protected int droneAtkSpd;

    //�����ϴ� ��ũ��Ʈ���� ����(set) ����
    protected int damageRate;
    protected int speed;
    protected float liveTime;
    protected float range;

    protected int finalDamage; //�÷��̾��� ���ݰ� �߻�ü�� ������������ ���Ͽ� ���������� ������ ������
    protected bool isHitOnce;  //�ش� �߻�ü�� �ѹ��� Ÿ�ݸ��� �ٷ���� : default = true �⺻������ �ѹ��� ������ ó�� ����
    protected List<GameObject> hittedEnemyList; //isHitOnce�� false��� �浹�� ���� ����
    //todo -> ���������� �� ��ũ��Ʈ�� �ٲ㺸��



    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        ResetProj();
    }

    protected virtual void OnDisable()
    {
        //��Ȱ��ȭ �� �ʱ�ȭ
        ResetProj();
    }

    protected virtual void ResetProj()
    {
        hittedEnemyList = new List<GameObject>();
        isParameterSet = false;

        //isSlow = false;
        //isSlowCount = 0;
        //isSlowExtraDamage = false; 
        //slowExtraDamageRate = 0;
        //isPenetrate = false; 
        //penetrateCount = 0;
        //damageCount = 0;
        //isCycleDamage = false;
        //cycleRate = 0;
        //isShootable = false;
        //isDrone = false; ;
        //droneAtkSpd = 0;

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0; 
        isHitOnce = false; 
        hittedEnemyList = new List<GameObject>(); 
    }

    protected void MoveLogic()
    {

    }

    protected void FollowPlayer()
    {
        transform.position = GameManager.Instance.myPlayer.transform.position;
    }

    protected void MoveUp()
    {
        if (isParameterSet)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    protected bool ableToMove;
    protected bool isAreaSkill;
    public virtual void SetProjParameter(int _projSpeed,int _dmgRate, float _liveTime, float _range, float value1 = 0, float value2 = 0)
    {
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;

        if (_projSpeed == 0)
        {
            ableToMove = false;
        }
        else
        {
            ableToMove = true;
            speed = _projSpeed;
        }

        if (_dmgRate == 0)
        {
            Debug.LogError("�������� �������� ����");
        }
        else
        {
            damageRate = _dmgRate;
            finalDamage = finalDamage = (int)playerStat.damage * damageRate / 100; //�⺻ ���� ������ ����. ���������� ���� ������
        }

        if (_liveTime != 0)
        {
            liveTime = _liveTime; //������ �߻�ü�� �� �� �ð��Ŀ� �ڵ����� �ı���
            StartCoroutine(LiveTimer(liveTime));
        }

        if (_range == 0)
        {
            isAreaSkill = false;
        }
        else
        {
            range = _range;
            transform.localScale = new Vector3(range, range, 0);
            isAreaSkill = true;
        }
    }

    //������ ������ ����. 
    protected virtual IEnumerator AreaDamageLogic()
    {
        while (isCycleDamage)
        {
            MultiEnemyDamage(); // ���� ������ ���� ȣ��

            yield return new WaitForSeconds(cycleRate); // �ֱ������� ������ ����
        }
    }

    //����Ʈ�� �Ѹ����� ���� Ÿ��
    protected virtual void SingleEnemyDamage()
    {
        GameObject enemy = hittedEnemyList[0];
        enemy.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject);
    }

    //����Ʈ�� ���� ���������� �� Ÿ��. ������� ������ ���̺� Ÿ������ ����
    protected virtual void MultiEnemyDamage()
    {
        for (int i = hittedEnemyList.Count - 1; i >= 0; i--)
        {
            GameObject enemy = hittedEnemyList[i];

            if (!enemy.activeSelf)
            {
                hittedEnemyList.RemoveAt(i); // ��Ȱ��ȭ�� �� ����
            }
            else
            {
                enemy.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject); // ������ ������

                // isHitOnce�� true�� ���, Ÿ�� �� ���� ����Ʈ���� ����
                if (isHitOnce)
                {
                    hittedEnemyList.RemoveAt(i);
                }
            }
        }
    }


    protected IEnumerator LiveTimer(float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        
        GameManager.Instance.Pool.ReleasePool(gameObject);
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletBorder")
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            TriggedEnemy(collision);
        }
    }

    //penetrate�ʿ�� ���⿡ �߰�
    protected virtual void TriggedEnemy(Collider2D collision)
    {
        //�⺻ �������� �������
        if (collision.GetComponent<EnemyObject>() == null)
        {
            Debug.Log("�� ��ũ��Ʈ�� ���� ��ȿ���� ����");
            return;
        }

        hittedEnemyList.Add(collision.gameObject);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) //���� ������ �����ٸ� ����Ʈ���� �� ����
    {
        if (hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Remove(collision.gameObject);
        }
        
    }
}
