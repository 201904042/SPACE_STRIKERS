using System.Collections;
using UnityEngine;

public class USkill_Char4 : UniqueSkill
{
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 694;
        useCharCode = 104;
        projType = PlayerProjType.Spcial_Player4;
        SetLevel();
        Debug.Log("USkill_Char4 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    public override IEnumerator ActivateSkillCoroutine(int level)
    {
        SkillParameterSet(level); //������ �޾ƿ� ���� �Ķ���͸� ����

        int projIndex = 0;
        float projDelay = liveTime / projCount;

        ActivateSkill(level); //��ź �߻�
        projIndex++;
        while (projIndex != projCount)
        {
            yield return new WaitForSeconds(projDelay);
            ActivateSkill(level); // ��ų �ߵ�
            projIndex++;
        }
        
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        USkill_TrackingMissile proj = GameManager.Instance.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_TrackingMissile>();
        proj.SetProjParameter(projSpd, dmgRate, 0, size); //���⼱ ���̺�Ÿ���� �ٸ� ������� ���
    }

    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 10,
            ProjSpd = 10,
            DmgRate = 100,
            LiveTime = 5f //5�ʾȿ� ���ι߻�
        };

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 15,
            ProjSpd = 10,
            DmgRate = 120,
            LiveTime = 5f
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 25,
            ProjSpd = 10,
            DmgRate = 150,
            LiveTime = 5f
        };
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }
}

