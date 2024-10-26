using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauncherStat : MonoBehaviour
{
    public const float bulletBaseInterval = 2f;  // �� �⺻ �߻� �ֱ�
    public const float MissileBaseInterval = 3f; // �Ѿ��� 2�ʿ� �ѹ�. �̻����� 3�ʿ� �ѹ�. ȣ���� 2�ʿ� �ѹ�
    public const float HomingBaseInterval = 1f;  // 

    public const int bulletBaseDamageRate = 100;  // �� �⺻ ������ ������
    public const int MissileBaseDamageRate = 150; // ���� ������  = �÷��̾� ���ݷ� + (�ð� * ������)
    public const int ExplosionBaseDamageRate = 80;
    public const int HomingBaseDamageRate  = 30;

    public const int bulletBaseSpeed= 10;
    public const int MissileBaseSpeed = 5;
    public const int HomingBaseSpeed = 15;

    public const float ExplosionBaseLiveTime = 1; // 1�� �����ϵ�
    public const float ExplosionBaseRange = 1; //-> ũ�� 1 -> 1.5 -> 2

    //�÷��̾��� �⺻ �Ѿ�, �̻���, ȣ�ֹ̻���
    protected PlayerStat pStat;
    protected PlayerControl pControl;
    protected PlayerProjType projType; //�߻��� Ÿ�� -> �ʼ� ���

    protected float projFireDelay; //�߻�ӵ�
    protected float attackInterval; //���� �߻� �ֱ�(��)  = �÷��̾� ���ݼӵ� = ���⺰ �⺻ ���ݼӵ� / (1+(�÷��̾� ���ݼӵ� - 10(����)) / 10(����))
    protected int projDamageRate; //�߻�ü�� ������ ���� 100, 150, 50
    protected int projSpeed; //�߻�ü�� �ӵ�. �Ѿ� 10, �̻��� 5, ȣ��15

  
    protected float pAtkSpd => pStat.attackSpeed;
    protected bool playerReady => pStat.CanAttack; //�÷��̾�� �� �غ� �Ǿ���
 
    protected bool isReadyToAttack; //���İ� �� �غ� �Ǿ���

    protected Coroutine LaunchCoroutine;

    protected virtual void Awake()
    {
        //������Ʈ ����
        pStat = PlayerMain.pStat;
        pControl = PlayerMain.pControl;
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
            if (!pStat.CanAttack || !isReadyToAttack) 
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
