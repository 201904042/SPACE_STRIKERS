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
    public List<ActiveSkill> usingActiveSkills; //ȹ���� ��Ƽ�� ��ų ����Ʈ
    public List<InGamePassiveSkill> usingPassiveSkills; //ȹ���� �нú� ��ų ����Ʈ

    public void Init()
    {
        ingameSkillList = new List<InGameSkill>();
        SetAllSkill();

        usingActiveSkills = new List<ActiveSkill>();
        usingPassiveSkills = new List<InGamePassiveSkill>();

        Debug.Log("��ų �ʱ�ȭ �Ϸ�");

        
    }

    //�����ϴ� ��� ��ų���� �ΰ��� ��ų ����Ʈ�� ��� => ���⼭ ���õ� ��ų�� ã�Ƽ� using����Ʈ�� ���
    private void SetAllSkill()
    {
        
        Active_ChargeShot active_chargeShot = new Active_ChargeShot();
        ingameSkillList.Add(active_chargeShot);
        Active_ElecShock active_elecShock = new Active_ElecShock();
        ingameSkillList.Add(active_elecShock);
        Active_EnergeField active_energyField = new Active_EnergeField();
        ingameSkillList.Add(active_energyField);
        Active_HomingMissile active_homingMissile = new Active_HomingMissile();
        ingameSkillList.Add(active_homingMissile);
        Active_MiniDrone active_miniDrone = new Active_MiniDrone();
        ingameSkillList.Add(active_miniDrone);
        Active_Missile active_missile = new Active_Missile();
        ingameSkillList.Add(active_missile);
        Active_Shield active_Shield = new Active_Shield();
        ingameSkillList.Add(active_Shield);

        Passive_Damage passive_damage = new Passive_Damage();
        ingameSkillList.Add(passive_damage);
        Passive_Defence passive_defence = new Passive_Defence();
        ingameSkillList.Add(passive_defence);
        Passive_MoveSpeed passive_moveSpeed = new Passive_MoveSpeed();
        ingameSkillList.Add(passive_moveSpeed);
        Passive_AttackSpeed passive_attackSpeed = new Passive_AttackSpeed();
        ingameSkillList.Add(passive_attackSpeed);
        Passive_HpRegeneration passive_hpRegeneration = new Passive_HpRegeneration();
        ingameSkillList.Add(passive_hpRegeneration);

        for (int i = 0; i < ingameSkillList.Count; i++) 
        {
            ingameSkillList[i].SkillReset();
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

    public void AddSkill(InGameSkill skill)
    {
        if(skill.type == SkillType.Active)
        {
            AddActiveSkill((ActiveSkill)skill);
        }
        else if(skill.type == SkillType.Passive)
        {
            AddPassiveSkill((InGamePassiveSkill)skill);
        }
        else
        {
            //addotherskill
        }
    }

    public void AddActiveSkill(ActiveSkill skill)
    {
        ActiveSkill existingSkill = usingActiveSkills.Find(s => s.SkillCode == skill.SkillCode);
        if (existingSkill != null)
        {
            LevelUpSkill(existingSkill); // ���� ��ų�� ������
            if(existingSkill.curSkillLevel == existingSkill.SkillLevels.Count)
            {
                ingameSkillList.Remove(skill); //���� �ִ뷹���� �Ǹ� �ΰ��� ����Ʈ������ ����
            }
        }
        else
        {
            usingActiveSkills.Add(skill); // ���ο� ��ų �߰�
            ActivateSkill(skill);
        }
        Debug.Log($"{skill.SkillCode}�� ���� {skill.curSkillLevel}����");
    }

    public void AddPassiveSkill(InGamePassiveSkill skill)
    {
        var existingSkill = usingPassiveSkills.Find(s => s.SkillCode == skill.SkillCode);
        if (existingSkill != null)
        {
            existingSkill.LevelUp(); // ���� ��ų�� ������
            if (existingSkill.curSkillLevel == existingSkill.SkillLevels.Count)
            {
                ingameSkillList.Remove(skill);
            }
        }
        else
        {
            usingPassiveSkills.Add(skill); // ���ο� ��ų �߰�
            ApplySkill(skill);
        }
        Debug.Log($"{skill.SkillCode}�� ���� {skill.curSkillLevel}����");
    }

    public void LevelUpSkill(InGameSkill skill)
    {
        skill.LevelUp();
    }

    public void ActivateSkill(ActiveSkill skill)
    {
        StartCoroutine(skill.ActivateSkillCoroutine());
    }

    public void ApplySkill(InGamePassiveSkill skill)
    {
        Debug.Log($"{skill.SkillCode}�� ȿ�� ����");
        skill.ApplyEffect();
    }
}
