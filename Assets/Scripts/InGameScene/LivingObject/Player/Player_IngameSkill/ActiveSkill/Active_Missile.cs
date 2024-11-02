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
        // 스킬 초기화 코드 (예: 스킬 레벨 세팅)
        Debug.Log("Active_Missile 초기화 완료");
        isInit = true;
    }

    public override void LevelUp()
    {
        base.LevelUp(); // 부모 클래스의 LevelUp 호출
    }

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);
        
        for (int i = 0; i < CurSkillValue.ProjCount; i++)
        {
            Skill_Missile proj = GameManager.Game.Pool.GetPlayerProj(projType, instantPoint.position, instantPoint.rotation).GetComponent<Skill_Missile>();
           
            Vector2 RandomDir = DirectionToRandomEnemy();
            proj.transform.up = RandomDir;

            proj.SetAddParameter(expDmg, expLiveTime, expSize);
            proj.SetProjParameter(projSpd, dmgRate, 0, 0); //발사체의 속도와 데미지, 폭발의 시간과 크기
        }
    }


    
}
