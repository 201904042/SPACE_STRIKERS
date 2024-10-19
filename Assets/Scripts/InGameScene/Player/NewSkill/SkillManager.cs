using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<NewActiveSkill> activeSkills = new List<NewActiveSkill>();
    public List<NewPassiveSkill> passiveSkills = new List<NewPassiveSkill>();

    public void AddActiveSkill(NewActiveSkill skill)
    {
        activeSkills.Add(skill);
    }

    public void AddPassiveSkill(NewPassiveSkill skill)
    {
        passiveSkills.Add(skill);
    }

    public void LevelUpSkill(InGameSkill skill)
    {
        skill.LevelUp();
    }

    public void ActivateSkill(NewActiveSkill skill)
    {
        skill.Activate();
    }

    public void ApplySkill(NewPassiveSkill skill)
    {
        skill.ApplyEffect(GameManager.game.myPlayer.GetComponent<PlayerStat>());
    }
}
