using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UniqueSkill : InGameSkill
{
    protected Skill_LevelValue CurSkillValue => SkillLevels[currentLevel - 1];
    public Transform instantPoint;
    public Coroutine skillCoroutine;
    protected PlayerProjType projType;

    protected int projNum;  //�߻�ü ����
    protected int projSpeed; //�߻�ü�� �ӵ�
    protected int dmgRate; //��������
    protected float liveTime; //�ߵ� �ð�
    protected float range; //ũ��

    public override void Init()
    {
        SkillLevels = new List<Skill_LevelValue>();
        instantPoint = GameManager.Instance.myPlayer.transform;
        skillCoroutine = null;
        type = SkillType.Unique;
        currentLevel = 1;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        //�������� �ǹ� ����
    }

    protected void SkillParameterSet()
    {
        Skill_LevelValue skillData = SkillLevels[currentLevel - 1];
        projNum = skillData.ProjNum;
        projSpeed = skillData.ProjSpeed;
        liveTime = skillData.LiveTime;
        dmgRate = skillData.DamageRate;
        range = skillData.Range;
    }


    public virtual IEnumerator ActivateSkillCoroutine()
    {
        ActivateSkill(); // ��ų �ߵ�
        //��ų �ߵ���
        yield return new WaitForSeconds(liveTime); // ��Ÿ�� ���� ���
        //��ų�ߵ� ����
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    public virtual void ActivateSkill()
    {
        // �߻�ü ���� �ڵ�
        Debug.Log($"����ũ ��ų �ߵ�");
    }

    ///// <summary>
    ///// �߻�ü�� ������ ���� �ܳ��ϵ���
    ///// </summary>
    ///// <returns></returns>
    //protected Vector2 DirectionToRandomEnemy()
    //{
    //    Debug.Log($"���� Ȱ��ȭ�� ���� ��: {GameManager.Instance.Spawn.activeEnemyList.Count}");

    //    if (GameManager.Instance.Spawn.activeEnemyList.Count == 0)
    //    {
    //        return Vector2.up; // ���� ���� ��� �⺻ ����
    //    }

    //    int randomIndex = Random.Range(0, GameManager.Instance.Spawn.activeEnemyList.Count);
    //    GameObject target = GameManager.Instance.Spawn.activeEnemyList[randomIndex];

    //    Vector2 direction = (target.transform.position - instantPoint.position).normalized;
    //    Debug.Log($"����: {direction}");
    //    return direction;
    //}

}
