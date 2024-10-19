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

        Debug.Log("��ų �ʱ�ȭ �Ϸ�");
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
        //Ÿ�Ժ� if �߰�?   
        var existingActiveSkill = ingameSkillList.Find(s => s.SkillCode == skillCode);
        if (existingActiveSkill != null)
        {
            Debug.Log($"{existingActiveSkill.SkillCode}�� ã��");
            return existingActiveSkill;
        }

        Debug.Log($"�ش� ��ų ã������ {skillCode}");
        return null;
    }


    public void AddActiveSkill(NewActiveSkill skill)
    {
        var existingSkill = usingActiveSkills.Find(s => s.SkillCode == skill.SkillCode);
        if (existingSkill != null)
        {
            existingSkill.LevelUp(); // ���� ��ų�� ������
        }
        else
        {
            usingActiveSkills.Add(skill); // ���ο� ��ų �߰�
            ActivateSkill(skill);
        }
        Debug.Log($"{skill.SkillCode}�� ���� {skill.currentLevel}����");
    }

    public void AddPassiveSkill(NewPassiveSkill skill)
    {
        var existingSkill = usingPassiveSkills.Find(s => s.SkillCode == skill.SkillCode);
        if (existingSkill != null)
        {
            existingSkill.LevelUp(); // ���� ��ų�� ������
        }
        else
        {
            usingPassiveSkills.Add(skill); // ���ο� ��ų �߰�
            ApplySkill(skill);
        }
        Debug.Log($"{skill.SkillCode}�� ���� {skill.currentLevel}����");
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
        Debug.Log($"{skill.SkillCode}�� ȿ�� ����");
        skill.ApplyEffect();
    }
}
