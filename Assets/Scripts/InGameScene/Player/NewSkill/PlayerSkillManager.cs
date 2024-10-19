using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public List<InGameSkill> ingameSkillList;
    public List<NewActiveSkill> usingActiveSkills;
    public List<NewPassiveSkill> usingPassiveSkills;

    public void Start()
    {
        ingameSkillList = new List<InGameSkill>();
        SetAllSkill();
        usingActiveSkills = new List<NewActiveSkill>();
        usingPassiveSkills = new List<NewPassiveSkill>();

        Debug.Log("스킬 초기화 완료");
    }

    private void SetAllSkill()
    {
        Active_Missile active_missile = new Active_Missile();
        ingameSkillList.Add(active_missile);
        
        Passive_Damage passive_damage = new Passive_Damage();
        ingameSkillList.Add(passive_damage);

        for (int i = 0; i < ingameSkillList.Count; i++) 
        {
            ingameSkillList[i].Init();
        }
    }

  

    public InGameSkill FindSkillByCode(int skillCode)
    {
        //타입별 if 추가?   
        var existingActiveSkill = ingameSkillList.Find(s => s.SkillCode == skillCode);
        if (existingActiveSkill != null)
        {
            Debug.Log($"{existingActiveSkill.SkillCode}를 찾음");
            return existingActiveSkill;
        }

        Debug.Log($"해당 스킬 찾지못함 {skillCode}");
        return null;
    }


    public void AddActiveSkill(NewActiveSkill skill)
    {
        var existingSkill = usingActiveSkills.Find(s => s.SkillCode == skill.SkillCode);
        if (existingSkill != null)
        {
            existingSkill.LevelUp(); // 기존 스킬의 레벨업
        }
        else
        {
            usingActiveSkills.Add(skill); // 새로운 스킬 추가
            ActivateSkill(skill);
        }
        Debug.Log($"{skill.SkillCode}은 현제 {skill.currentLevel}레벨");
    }

    public void AddPassiveSkill(NewPassiveSkill skill)
    {
        var existingSkill = usingPassiveSkills.Find(s => s.SkillCode == skill.SkillCode);
        if (existingSkill != null)
        {
            existingSkill.LevelUp(); // 기존 스킬의 레벨업
        }
        else
        {
            usingPassiveSkills.Add(skill); // 새로운 스킬 추가
            ApplySkill(skill);
        }
        Debug.Log($"{skill.SkillCode}은 현제 {skill.currentLevel}레벨");
    }

    public void LevelUpSkill(InGameSkill skill)
    {
        skill.LevelUp();
    }

    public void ActivateSkill(NewActiveSkill skill)
    {
        StartCoroutine(skill.ActivateSkillCoroutine());
    }

    public void ApplySkill(NewPassiveSkill skill)
    {
        Debug.Log($"{skill.SkillCode}의 효과 적용");
        skill.ApplyEffect();
    }
}
