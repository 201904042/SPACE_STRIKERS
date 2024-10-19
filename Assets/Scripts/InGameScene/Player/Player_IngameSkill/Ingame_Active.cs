using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Ingame_Active : MonoBehaviour
{
    private PlayerStat playerStat;
    protected SkillInterface skillInterface;

    [Header("�ΰ��� ��ų ���� ����")]
    public int level; //��ų�� ����
    public float Stat_Damage; //�÷��̾��� ���ݷ�
    public float damageRate; //��ų�� ���
    public float coolTime; //��ų�� ��Ÿ��
    public int projNum; //��ų�� �߻�ü ����
    public bool isLevelUp; //������ UI���� ��ų�� ������ �ٲ��ٶ� true�� �ٲ��ֱ�
    public bool activated;

    protected virtual void Awake()
    {
        playerStat = GameManager.Instance.myPlayer.GetComponent<PlayerStat>();
        skillInterface = transform.GetComponent<SkillInterface>();
        level = skillInterface.level;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void Update()
    {
        if(level != skillInterface.level)
        {
            level = skillInterface.level;
            isLevelUp = true;
        }
    }

    protected virtual void Init()
    {
        activated = false;
        isLevelUp = false;
        Stat_Damage = playerStat.damage;
    }

    protected virtual void LevelSet(int level)
    {
        Debug.Log($"{gameObject.name} : ��ų ������. ���緹�� {level} ");
    }

}
