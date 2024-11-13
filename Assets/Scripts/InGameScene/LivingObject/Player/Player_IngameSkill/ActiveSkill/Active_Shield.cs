using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Active_Shield : ActiveSkill
{
    private Skill_Shield instantShield;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 607;
        projType = PlayerProjType.Skill_Shield;
        SetLevel(); 
        instantShield = null;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��

        if (instantShield != null) {
            GameManager.Game.Pool.ReleasePool(instantShield.gameObject); // �������� ���� ��ų �ı� �� �ٷ� �����
            instantShield = null;
        }
        
        ActivateSkill(curSkillLevel);
    }

    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        //ActivateSkill(curSkillLevel); // ��ų �ߵ�
        while (true)
        {
            if (instantShield != null) 
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(CurSkillValue.CoolTime); // ��Ÿ�� ���� ���
            ActivateSkill(curSkillLevel); // ��ų �ߵ�
        }
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    protected override void ActivateSkill(int level)
    {
        // �߻�ü ���� �ڵ�
        if(instantShield != null)
        {
            return;
        }
        Skill_Shield proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Shield>();
        instantShield = proj;
        proj.SetProjParameter(0, dmgRate, 0, size);
    }


    
}