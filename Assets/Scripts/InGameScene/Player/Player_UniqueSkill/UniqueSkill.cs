using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UniqueSkill : InGameSkill
{
    public Skill_LevelValue CurSkillValue;
    public Transform instantPoint;
    protected PlayerProjType projType;

    public int useCharCode; //����ϴ� ĳ������ id

    protected int projNum;  //�߻�ü ����
    protected int projSpeed; //�߻�ü�� �ӵ�
    protected int dmgRate; //��������
    protected float liveTime; //�ߵ� �ð�
    protected float range; //ũ��

    public bool isSkillActive;

    //�ٸ� ��ų�� �޸� ��ų ������ �ܺο��� �޾ƿ� (����)

    public override void Init()
    {
        SkillLevels = new Dictionary<int, Skill_LevelValue>();
        instantPoint = PlayerMain.Instance.transform;
        type = SkillType.Unique;
        isSkillActive = false;
    }

    public override void SetLevel()
    {
        Debug.Log($"{SkillCode}�� ���������� ����");
    }

    public override void LevelUp()
    {
        //�������� �ǹ� ����
    }

    protected virtual void SkillParameterSet(int level)
    {
        CurSkillValue = SkillLevels[level];
        projNum = CurSkillValue.ProjNum;
        projSpeed = CurSkillValue.ProjSpeed;
        liveTime = CurSkillValue.LiveTime;
        dmgRate = CurSkillValue.DamageRate;
        range = CurSkillValue.Range;
        
    }


    public virtual IEnumerator ActivateSkillCoroutine(int level)
    {
        SkillParameterSet(level); //������ �޾ƿ� ���� �Ķ���͸� ����
        ActivateSkill(level); // ��ų �ߵ�
        //��ų �ߵ���
        isSkillActive = true;
        yield return new WaitForSeconds(liveTime); // ��Ÿ�� ���� ���
        //��ų�ߵ� ����

        isSkillActive = false;
    }

    /// <summary>
    /// �� ��ų�� �������� ����
    /// </summary>
    public virtual void ActivateSkill(int level)
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
