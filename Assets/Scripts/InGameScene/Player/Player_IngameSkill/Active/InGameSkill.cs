using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillAddEffect
{
    None,
    Slow, //�ش� �߻�ü�� ���� ���� ������, �������� ����
    SlowExtraDamage, //�ش� �߻�ü�� ���� ���� ���ο� ���¶�� �߰�������, �߰���%
    Penetrate, //�ش� �߻�ü�� ����� , ��� ���� ��������
    CycleDamage, //�� ��ü�� �浹�ص� ������� ������ ���������� �������� �� ,���ʸ��� �������� ����
    Drone, //����� ��ȯ�Ͽ� ����, ����� ���ݼӵ�(�ð���)
    Shield //���彺ų����, ������ ��ø����
}

public class S_EffectValuePair
{
    public SkillAddEffect effect;
    public float value;

    public S_EffectValuePair(SkillAddEffect effect, float value)
    {
        this.effect = effect;
        this.value = value;
    }
}

public class Skill_LevelValue
{
    public int ProjNum; // ����ü ��. ��ġ��ŭ �ݺ��Ͽ� ����
    public int ProjSpeed; //����ü�� �ӵ�. ���� 10(�÷��̾��� �⺻�ӵ�). 20�̸� �÷��̾� 2��ӵ� 5�� �÷��̾�� �ݹ� ����
    public float Cooldown; // ��Ÿ�� (�ð� ��)
    public int DamageRate; // ������ ��� %. �÷��̾��� ������ * ����������. �÷��̾� ������ 10�̰� ������������ 120%��� �������� 12
    public float Range; // ����(�߻�ü�� ���� ������), �ַ� ������ ��ų�� ����
    public float LiveTime; //���ӽð�(�ð� ��). �� ���ӽð��� ������ �߻�ü�� �ı�
    public List<S_EffectValuePair> AdditionalEffects; // �߰� ȿ�� (��ȭ��, ����, ���� ��)

    public Skill_LevelValue()
    {
        AdditionalEffects = new List<S_EffectValuePair>();
    }
}

//���뽺ų , ����� ��ų
public abstract class InGameSkill
{
    public int SkillCode { get; protected set; }
    public SkillType type { get; protected set; }
    public List<Skill_LevelValue> SkillLevels { get; protected set; }
    public int currentLevel;
    public string description;
    public abstract void Init();
    public abstract void LevelUp();
    public abstract void SetLevel();
}
