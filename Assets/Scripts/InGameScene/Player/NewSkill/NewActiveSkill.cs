using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SkillLevel
{
    public int ProjectileCount; // 투사체 수
    public float Cooldown; // 쿨타임
    public float DamageMultiplier; // 데미지 배수
    public float Range; // 범위
    public float AdditionalEffect; // 추가 효과 (둔화율, 지속 시간 등)
}

public class NewActiveSkill : InGameSkill
{
    public List<SkillLevel> SkillLevels = new List<SkillLevel>();
    private int currentLevel = 0; // 현재 레벨

    public override void LevelUp()
    {
        if (currentLevel < SkillLevels.Count - 1)
        {
            currentLevel++;
        }
    }

    public void Activate()
    {
        // 발사체 생성 코드
        Debug.Log("액티브스킬 {id}의 발사체 생성 시도");

        SkillLevel currentSkillLevel = SkillLevels[currentLevel];

        for (int i = 0; i < currentSkillLevel.ProjectileCount; i++)
        {
            // 투사체 생성 로직
            //스킬의 프리팹을 플레이어 위치에서 생성 및 프리팹에 스텟 부여

        }

        // 쿨타임 설정
        Cooldown = currentSkillLevel.Cooldown;

        // 데미지 적용
        float damage = /*baseDamage * */ currentSkillLevel.DamageMultiplier;
    }
}
