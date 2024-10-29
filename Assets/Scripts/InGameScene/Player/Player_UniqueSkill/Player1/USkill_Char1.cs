using UnityEngine;

public class USkill_Char1 : UniqueSkill
{
    
    public override void SkillReset()
    {
        base.SkillReset();
        SkillCode = 691;
        useCharCode = 101;
        projType = PlayerProjType.Spcial_Player1;
        SetLevel();
        Debug.Log("USkill_Char1 �ʱ�ȭ �Ϸ�");
    }

    public override void LevelUp()
    {
        //������ ������ ����
    }

    private float space = 0.75f;

    protected override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        float spawnXpos = GetInitialSpawnXpos();
        float spawnYpos = -4.5f;

        for (int i = 0; i < projCount; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnXpos + (i * space), spawnYpos, 0f);
            Troop troop = GameManager.Instance.Pool.GetPlayerProj(projType, spawnPosition, instantPoint.rotation).GetComponent<Troop>();
            troop.SetProjParameter(projSpd, dmgRate, liveTime, size);

            // ���� ���� ����
            Transform instantTransform = troop.transform.GetChild(0);
            //���ο� ���� �Ҵ�
            GameObject Shooter = Resources.Load<GameObject>($"Prefab/Player/Shooters/Troop/Lv{level}");
            if(Shooter == null)
            {
                Debug.LogError($"Ʈ���� ���Ͱ� �������� ���� ��� : Prefab/Player/Shooter/Troop/Lv{level}");
            }
            GameObject.Instantiate(Shooter, instantTransform);
        }
    }

    // �ʱ� spawnXpos�� ����ϴ� �޼���
    private float GetInitialSpawnXpos()
    {

        float spawnXpos = instantPoint.position.x - (projCount / 2) * space;

        // X ��ġ�� -2.5���� 2.5 ���̷� ����
        if (spawnXpos < -2.5f)
        {
            spawnXpos = -2.5f;
        }
        else if (spawnXpos + (projCount * space) > 2.5f) //������������ ��������*�����°��� ���Ͽ� ������ ��ġ�� �˾Ƴ��� ��
        {
            spawnXpos = 2.5f - (projCount * space);
        }

        return spawnXpos;
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjCount = 1,
            ProjSpd = 5,
            LiveTime = 5,
            DmgRate = 50
        };

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjCount = 3,
            ProjSpd = 5,
            LiveTime = 7.5f,
            DmgRate = 75
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjCount = 5,
            ProjSpd = 5,
            LiveTime = 10,
            DmgRate = 100
        };
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}�� ���� {SkillLevels.Count}�� ���");
    }

}
