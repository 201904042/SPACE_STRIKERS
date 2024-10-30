using UnityEngine;

public class Active_Missile : ActiveSkill
{
    public bool isInit = false;

    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 606;
        projType = PlayerProjType.Skill_Missile;
        SetLevel();
        // ��ų �ʱ�ȭ �ڵ� (��: ��ų ���� ����)
        Debug.Log("Active_Missile �ʱ�ȭ �Ϸ�");
        isInit = true;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // �θ� Ŭ������ LevelUp ȣ��
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);
        
        for (int i = 0; i < CurSkillValue.ProjCount; i++)
        {
            Skill_Missile proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Missile>();
           
            Vector2 RandomDir = DirectionToRandomEnemy();
            proj.transform.up = RandomDir;

            proj.SetAddParameter(expDmg, expLiveTime, expSize);
            proj.SetProjParameter(projSpd, dmgRate, 0, 0); //�߻�ü�� �ӵ��� ������, ������ �ð��� ũ��
        }
    }


    
}
