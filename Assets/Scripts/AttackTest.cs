using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AttackTest : MonoBehaviour
{
    [Header("멀티샷 전용")]
    public int projCount = 3;
    public int spreadAngel = 30;

    [Header("싱글샷 전용")]
    public int singleAngle = 0;

    [Header("분열탄 전용")]
    public int splitCount = 5;

    [Header("레이저 전용")]
    

    [Header("공통 전용")]
    public bool isAim;
    public OtherProjType type = OtherProjType.Enemy_Bullet;

    private void Awake()
    {
       // StartCoroutine(attacking());
    }

    private IEnumerator attacking()
    {
        while (true)
        {
            FireMulti(type, projCount, spreadAngel, isAim);
            yield return new WaitForSeconds(1f);
        }
    }

    [ContextMenu("멀티 공격")]
    public void MultiAttack()
    {
        EnemyProjectile[] projs = FireMulti(type, projCount, spreadAngel, isAim);
        foreach (EnemyProjectile proj in projs)
        {
            proj.SetProjParameter(10, 0);
            if (type == OtherProjType.Enemy_Split)
            {
                proj.SetSplitCount(splitCount);
            }
            else if (type == OtherProjType.Enemy_Laser)
            {
                proj.SetLaser(gameObject, isAim);
            }
        }
    }

    [ContextMenu("싱글 공격")]
    public void SingleAttack()
    {
        EnemyProjectile proj = FireSingle(type, singleAngle, isAim);
        proj.SetProjParameter(10, 0);
        if (type == OtherProjType.Enemy_Split)
        {
            proj.SetSplitCount(splitCount);
        }
        else if (type == OtherProjType.Enemy_Laser)
        {
            proj.SetLaser(gameObject, isAim);
        }
    }

    public EnemyProjectile FireSingle(OtherProjType _proj,float angle = 0, bool _isAim = false)
    {
        EnemyProjectile proj = GameManager.Game.Pool.GetOtherProj(_proj, transform.position, transform.rotation).GetComponent<EnemyProjectile>();

        if (_isAim)
        {
            // 플레이어를 향한 방향을 계산하고 angle만큼 회전시킴
            Vector3 dir = (PlayerMain.Instance.transform.position - transform.position).normalized;
            proj.transform.up = Quaternion.Euler(0, 0, angle) * dir;
        }
        else
        {
            // transform.up 방향에서 angle만큼 회전시킴
            proj.transform.up = Quaternion.Euler(0, 0, angle) * transform.up;
        }
        
        return proj;
    }


    public EnemyProjectile[] FireMulti(OtherProjType _proj, int _projectileCount = 1, float _spreadAngle = 0, bool _isAim = false)
    {
        EnemyProjectile[] proj = new EnemyProjectile[_projectileCount];
        float startAngle = -_spreadAngle / 2;
        for (int i = 0; i < _projectileCount; i++)
        {
            // 각 발사체가 발사되는 각도 계산
            float angle = startAngle + (i * (_spreadAngle / (_projectileCount - 1)));
            if(_projectileCount == 1)
            {
                angle = 0;
            }

            proj[i] = FireSingle(_proj, angle, _isAim);
        }

        return proj;
    }

}
