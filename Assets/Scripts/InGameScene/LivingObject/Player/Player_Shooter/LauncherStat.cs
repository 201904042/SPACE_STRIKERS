using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LauncherLevel
{
    public int level; //���� ������ ����
    public int DamageRate; //������ ������ ������
    public float Delay; //������ ������ ���ð�
    public int ProjSpeed; //���Ͱ� �߻��ϴ� ��ü�� �ӵ�
}

public class LauncherStat : MonoBehaviour
{
    //�÷��̾��� �⺻ �Ѿ�, �̻���, ȣ�ֹ̻���
    protected PlayerStat pStat;
    protected PlayerControl pControl;
    protected PlayerProjType projType; //�߻��� Ÿ�� -> �ʼ� ���

    protected float projFireDelay; //�߻�ӵ�
    protected float attackInterval; //���� �߻� �ֱ�(��)  = �÷��̾� ���ݼӵ� = ���⺰ �⺻ ���ݼӵ� / (1+(�÷��̾� ���ݼӵ� - 10(����)) / 10(����))
    protected int projDamageRate; //�߻�ü�� ������ ���� 100, 150, 50
    protected int projSpeed; //�߻�ü�� �ӵ�. �Ѿ� 10, �̻��� 5, ȣ��15

  
    protected float pAtkSpd => pStat.IG_AttackSpeed;
    protected bool playerReady => PlayerMain.Instance.isOnAttack; //�÷��̾�� �� �غ� �Ǿ���
 
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
            if (!PlayerMain.Instance.isOnAttack) 
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
        //Debug.Log($"{nameof(projType)}�� �߻��");
        //GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation);
    }

    

}
