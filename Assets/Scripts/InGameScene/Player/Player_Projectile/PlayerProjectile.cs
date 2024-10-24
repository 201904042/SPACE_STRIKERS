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
    [SerializeField] protected bool isParameterSet; //�Ķ���Ͱ� ������? �����Ǿ�� ������ : default =false


    [SerializeField] protected int damageRate;
    [SerializeField] protected int speed;
    [SerializeField] protected float liveTime;
    [SerializeField] protected float range;

    protected int finalDamage; //�÷��̾��� ���ݰ� �߻�ü�� ������������ ���Ͽ� ���������� ������ ������
    [SerializeField] protected bool isHitOnce;  //�ش� �߻�ü�� �ѹ��� Ÿ�ݸ��� �ٷ���� : default = true �⺻������ �ѹ��� ������ ó�� ����
    [SerializeField] protected List<GameObject> hittedEnemyList; //isHitOnce�� false��� �浹�� ���� ����
    //todo -> ���������� �� ��ũ��Ʈ�� �ٲ㺸��

    protected bool isShootingObj; //�߻�Ǵ��� �ƴϸ� �÷��̾ ����ٴϴ���

    protected Coroutine activated;
    protected Coroutine damaging;

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

        damageRate = 0;
        speed = 0;
        liveTime = 0;
        range = 0;
        finalDamage = 0; 
        isHitOnce = true;
        isShootingObj = false;
        activated = null;
        damaging= null;
    }

    protected virtual void Update()
    {
        if (!isParameterSet)
        {
            Debug.Log("�Ķ���Ͱ� �������� ����");
            return;
        }

        if (isShootingObj) 
        {
            MoveUp();
        }
        else
        {
            FollowPlayer();
        }
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

    public virtual void SetAddParameter(float value1, float value2 =0, float value3 = 0)
    {
        Debug.Log("���� �Ķ���� ����");
    }

    // -> �ʼ� ���
    public virtual void SetProjParameter(int _projSpeed,int _dmgRate, float _liveTime, float _range)
    {
        Debug.Log("���� �Ķ���� ����");
        isParameterSet = true;

        if (_projSpeed == 0)
        {
            isShootingObj = false;
        }
        else
        {
            isShootingObj = true;
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
            Debug.Log("�⺻ ũ��");
        }
        else
        {
            range = _range;
            transform.localScale = new Vector3(range, range, 0);
        }
    }

    //������ ������ ����. ������ �� 1ȸ�� Ÿ���� ����Ŭ �ð����� �ݺ�
    protected virtual IEnumerator AreaDamageLogic(float _cycleRate)
    {
        while (true)
        {
            if(hittedEnemyList.Count == 0)
            {
                yield return new WaitForSeconds(_cycleRate);
                continue;
            }
            MultiEnemyDamage(); // ���� ������ ���� ȣ��

            yield return new WaitForSeconds(_cycleRate); // �ֱ������� ������ ����
        }
    }

    //�浹�� �� 1ü�� ������ ���� �ı����� ����
    protected virtual void SingleEnemyDamage()
    {
        GameObject enemy = hittedEnemyList[0];
        enemy.GetComponent<EnemyObject>().EnemyDamaged(finalDamage, gameObject);
        hittedEnemyList.RemoveAt(0);
        if (isHitOnce)
        {
            GameManager.Instance.Pool.ReleasePool(gameObject);
        }
    }

    //���� �� �� 1ȸ�� Ÿ��
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
                if (isHitOnce)
                {
                    hittedEnemyList.RemoveAt(i);
                }
            }
        }
    }


    protected virtual IEnumerator LiveTimer(float activeTime)
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

    //������ ��������ƾ�� �ڷ�ƾ�� ���۽�ų�� -> �ʼ� ���
    protected virtual void TriggedEnemy(Collider2D collision)
    {
        //�⺻ �������� �������
        if (collision.GetComponent<EnemyObject>() == null)
        {
            Debug.Log("�� ��ũ��Ʈ�� ���� ��ȿ���� ����");
            return;
        }
        if (!hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Add(collision.gameObject);
        }
        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) //���� ������ �����ٸ� ����Ʈ���� �� ����
    {
        if (hittedEnemyList.Contains(collision.gameObject))
        {
            hittedEnemyList.Remove(collision.gameObject);
        }
        
    }
}
