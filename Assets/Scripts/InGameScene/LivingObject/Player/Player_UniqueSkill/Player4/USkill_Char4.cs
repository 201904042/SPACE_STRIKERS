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
        //Debug.Log("USkill_Char4 �ʱ�ȭ �Ϸ�");
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

        USkill_TrackingMissile proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<USkill_TrackingMissile>();
        proj.SetProjParameter(projSpd, dmgRate, 0, size); //���⼱ ���̺�Ÿ���� �ٸ� ������� ���
    }

}

