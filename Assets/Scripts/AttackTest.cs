using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AttackTest : MonoBehaviour
{
    public int projCount = 1;
    public int spreadAngel = 30;
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
            FireMulti(type, 10, 0, 0, projCount, spreadAngel, isAim);
            yield return new WaitForSeconds(1f);
        }
    }

    [ContextMenu("����")]
    public void Attack()
    {
        FireMulti(type, 10, 0, 0, projCount, spreadAngel, isAim);
    }

    public EnemyProjectile FireSingle(OtherProjType _proj, int _dmgRate, int _liveTime = 0, int _size = 0, bool _isAim = false)
    {
        EnemyProjectile proj = GameManager.Game.Pool.GetOtherProj(_proj, transform.position, transform.rotation).GetComponent<EnemyProjectile>();

        if (_isAim)
        {
            Vector3 dir = (PlayerMain.Instance.transform.position - transform.position).normalized;
            proj.transform.up = dir; 
        }
        else
        {
            proj.transform.up = transform.up;
        }

        proj.SetProjParameter(_dmgRate, _liveTime, _size);
        return proj;
    }

    public void FireMulti(OtherProjType _proj, int _dmgRate, int _liveTime = 0, int _size = 0, int _projectileCount = 1, float _spreadAngle = 0, bool _isAim = false)
    {
        // _isAim�� false�̸� ������ ����, true�̸� �÷��̾��� ������ �������� ��
        Vector3 pDir = Vector3.zero;
        if (_isAim)
        {
            // �÷��̾��� ��ġ�� �������� ������ ���
            pDir = (PlayerMain.Instance.transform.position - transform.position).normalized;
        }

        // �߻�Ǵ� ���� ���� ����
        float startAngle = -_spreadAngle / 2;
        for (int i = 0; i < _projectileCount; i++)
        {
            // �� �߻�ü�� �߻�Ǵ� ���� ���
            float angle = startAngle + (i * (_spreadAngle / (_projectileCount - 1)));
            if(_projectileCount == 1)
            {
                angle = 0;
            }
            //// isAim�� true�̸� pDir�� �������� ȸ��, false�̸� transform.up�� �������� ȸ��
            Vector3 dir = _isAim ? Quaternion.Euler(0, 0, angle) * pDir : Quaternion.Euler(0, 0, angle) * transform.up;

            // Ensure the direction is normalized
            dir.Normalize();

            EnemyProjectile proj = FireSingle(_proj, _dmgRate, _liveTime, _size, _isAim);
            proj.transform.up = dir;
        }
    }

}
