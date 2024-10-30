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


}
