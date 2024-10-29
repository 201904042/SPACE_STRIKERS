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
        SkillParameterSet(curSkillLevel);// => 1������ ��ų �Ķ���� ����
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


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 1,
            ProjSpd = 15,
            CoolTime = 3,
            DmgRate = 150,
        };
        lv1.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 150, 1, 1));
        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 2,
            ProjSpd = 15,
            CoolTime = 3,
            DmgRate = 150,
        };
        lv2.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 150, 1, 1.2f));
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 2,
            ProjSpd = 15,
            CoolTime = 3,
            DmgRate = 170,
        };
        lv3.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 170, 1, 1.4f));
        SkillLevels.Add(lv3.level, lv3);

        Skill_LevelValue lv4 = new Skill_LevelValue()
        {
            level = 4,
            ProjCount = 2,
            ProjSpd = 15,
            CoolTime = 2,
            DmgRate = 170,
        };
        lv4.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 170, 1, 1.6f));
        SkillLevels.Add(lv4.level, lv4);

        Skill_LevelValue lv5 = new Skill_LevelValue()
        {
            level = 5,
            ProjCount = 4,
            ProjSpd = 15,
            CoolTime = 2,
            DmgRate = 170,
        };
        lv5.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 170, 1, 1.8f));
        SkillLevels.Add(lv5.level, lv5);

        Skill_LevelValue lv6 = new Skill_LevelValue()
        {
            level = 6,
            ProjCount = 4,
            ProjSpd = 15,
            CoolTime = 1,
            DmgRate = 200,
        };
        lv6.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 200, 1, 2f));
        SkillLevels.Add(lv6.level, lv6);

        Skill_LevelValue lv7 = new Skill_LevelValue()
        {
            level = 7,
            ProjCount = 4,
            ProjSpd = 15,
            CoolTime = 1,
            DmgRate = 200,
        };
        lv7.AddEffect.Add(new S_EffectValuePair(SkillAddEffect.Explosion, 200, 1, 2.5f));
        SkillLevels.Add(lv7.level, lv7);
        //Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }

}
