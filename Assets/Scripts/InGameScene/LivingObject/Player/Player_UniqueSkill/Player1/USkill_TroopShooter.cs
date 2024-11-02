using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class TroopShooter : LauncherStat
{
    //todo => ������ �׳� �÷��̾� �ҷ����Ŀ� �Ȱ����� �ð��Ǹ� Ʈ�� ���� �Ѿ˵� �����
    private Troop userTroop;

    protected override void Awake()
    {
        base.Awake();
        userTroop = GetComponentInParent<Troop>();
    }

    private void OnEnable()
    {
        SetLauncher();
    }

    protected override void SetLauncher()
    {
        base.SetLauncher();
        
        projType = PlayerProjType.Player_Bullet;
        
        //projFireDelay = TroopBaseInterval; �Ⱦ�
        projDamageRate = PlayerMain.TroopBaseDamageRate;
        projSpeed = PlayerMain.TroopBaseSpeed;
        //���� �߻� �ֱ�(��) = �÷��̾� ���ݼӵ� = ���⺰ �⺻ ���ݼӵ� / (1 + (�÷��̾� ���ݼӵ� - 10(����)) / 10(����))
        attackInterval = PlayerMain.TroopBaseInterval;

        isReadyToAttack = true;
        if (LaunchCoroutine == null)
        {
            LaunchCoroutine = StartCoroutine(AttackRoutine(attackInterval));
        }
    }

    protected override IEnumerator AttackRoutine(float delay)
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


    protected override void Fire()
    {
        base.Fire();
        PlayerBullet proj = GameManager.Game.Pool.GetPlayerProj(projType, transform.position, transform.rotation).GetComponent<PlayerBullet>();
        proj.SetProjParameter(projSpeed, projDamageRate, 0, 0);
    }

}
