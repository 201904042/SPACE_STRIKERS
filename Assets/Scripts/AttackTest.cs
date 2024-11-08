using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AttackTest : MonoBehaviour
{
    [Header("��Ƽ�� ����")]
    public int projCount = 3;
    public int spreadAngel = 30;

    [Header("�̱ۼ� ����")]
    public int singleAngle = 0;

    [Header("�п�ź ����")]
    public int splitCount = 5;

    [Header("������ ����")]
    

    [Header("���� ����")]
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

    [ContextMenu("��Ƽ ����")]
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

    [ContextMenu("�̱� ����")]
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
            // �÷��̾ ���� ������ ����ϰ� angle��ŭ ȸ����Ŵ
            Vector3 dir = (PlayerMain.Instance.transform.position - transform.position).normalized;
            proj.transform.up = Quaternion.Euler(0, 0, angle) * dir;
        }
        else
        {
            // transform.up ���⿡�� angle��ŭ ȸ����Ŵ
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
            // �� �߻�ü�� �߻�Ǵ� ���� ���
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
