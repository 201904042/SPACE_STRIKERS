using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauncherStat : MonoBehaviour
{
    protected const float bulletBaseInterval = 2f;  // �� �⺻ �߻� �ֱ�
    protected const float MissileBaseInterval = 3f; // �Ѿ��� 2�ʿ� �ѹ�. �̻����� 3�ʿ� �ѹ�. ȣ���� 2�ʿ� �ѹ�
    protected const float HomingBaseInterval = 1f;  // 

    protected const int bulletBaseDamageRate = 100;  // �� �⺻ ������ ������
    protected const int MissileBaseDamageRate = 150; // ���� ������  = �÷��̾� ���ݷ� + (�ð� * ������)
    protected const int HomingBaseDamageRate  = 30;  

    protected const int bulletBaseSpeed= 10;  
    protected const int MissileBaseSpeed = 5; 
    protected const int HomingBaseSpeed = 15;  

    //�÷��̾��� �⺻ �Ѿ�, �̻���, ȣ�ֹ̻���
    protected PlayerStat pStat;
    protected PlayerControl pControl;
    protected PlayerProjType projType; //�߻��� Ÿ�� -> �ʼ� ���

    protected float projFireDelay; //�߻�ӵ�
    protected float attackInterval; //���� �߻� �ֱ�(��)  = �÷��̾� ���ݼӵ� = ���⺰ �⺻ ���ݼӵ� / (1+(�÷��̾� ���ݼӵ� - 10(����)) / 10(����))
    protected int projDamageRate; //�߻�ü�� ������ ���� 100, 150, 50
    protected int projSpeed; //�߻�ü�� �ӵ�. �Ѿ� 10, �̻��� 5, ȣ��15

  
    protected float pAtkSpd => pStat.attackSpeed;
    protected bool playerReady => pControl.isAttackable; //�÷��̾�� �� �غ� �Ǿ���
 
    protected bool isReadyToAttack; //���İ� �� �غ� �Ǿ���

    protected Coroutine LaunchCoroutine;

    protected virtual void Awake()
    {
        //������Ʈ ����
        pStat = GameManager.Instance.myPlayerStat;
        pControl = GameManager.Instance.myPlayerControl;
        projFireDelay = 0;
        attackInterval = 0;
        projSpeed = 0;
        projDamageRate = 0;

        isReadyToAttack = false;
        if (LaunchCoroutine != null)
        {
            LaunchCoroutine = null;
        }
    }

    protected virtual void Start()
    {
        SetLauncher();
    }

    //�ʼ� ���
    protected virtual void SetLauncher()
    {
        //���⺰ ���� ����
    }

    protected virtual IEnumerator AttackRoutine(float delay)
    {
        while (true)
        {
            if (!pControl.isAttackable || !isReadyToAttack) 
            {
                yield return null;
            }

            Fire();
            yield return new WaitForSeconds(delay);
        }
    }

    //�ʼ� ���
    protected virtual void Fire()
    {
        Debug.Log($"{nameof(projType)}�� �߻��");
        //GameManager.Instance.Pool.GetPlayerProj(projType, transform.position, transform.rotation);
    }

    
}
