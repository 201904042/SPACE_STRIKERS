using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMissileLauncher : NewActiveSkill
{
    public NewMissileLauncher()
    {
        SkillCode = 601;

        // 레벨 1 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 1,
            Cooldown = 3f,
            DamageMultiplier = 1.0f,
            Range = 150f,
        });

        // 레벨 2 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 2,
            Cooldown = 3f,
            DamageMultiplier = 1.2f,
            Range = 150f,
        });

        // 레벨 3 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 2,
            Cooldown = 2f,
            DamageMultiplier = 1.2f,
            Range = 150f,
        });

        // 레벨 4 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 4,
            Cooldown = 2f,
            DamageMultiplier = 1.3f,
            Range = 150f,
        });

        // 레벨 5 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 4,
            Cooldown = 1f,
            DamageMultiplier = 1.5f,
            Range = 300f,
        });

        // 레벨 6 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 5,
            Cooldown = 1f,
            DamageMultiplier = 1.8f,
            Range = 300f,
        });

        // 레벨 7 효과
        SkillLevels.Add(new SkillLevel
        {
            ProjectileCount = 5,
            Cooldown = 1f,
            DamageMultiplier = 2.0f,
            Range = 300f,
        });
    }
}
