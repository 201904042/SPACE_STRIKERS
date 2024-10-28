using UnityEngine;

public class USkill_Char1 : UniqueSkill
{
    
    public override void Init()
    {
        base.Init();
        SkillCode = 691;
        useCharCode = 101;
        projType = PlayerProjType.Spcial_Player1;
        SetLevel();
        Debug.Log("USkill_Char1 초기화 완료");
    }

    public override void LevelUp()
    {
        //레벨업 개념이 없음
    }

    private float space = 0.75f;

    public override void ActivateSkill(int level)
    {
        base.ActivateSkill(level);

        float spawnXpos = GetInitialSpawnXpos();
        float spawnYpos = -4.5f;

        for (int i = 0; i < projNum; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnXpos + (i * space), spawnYpos, 0f);
            Troop troop = GameManager.Instance.Pool.GetPlayerProj(projType, spawnPosition, instantPoint.rotation).GetComponent<Troop>();
            troop.SetProjParameter(projSpeed, dmgRate, liveTime, range);

            // 이전 슈터 제거
            Transform instantTransform = troop.transform.GetChild(0);
            //새로운 슈터 할당
            GameObject Shooter = Resources.Load<GameObject>($"Prefab/Player/Shooters/Troop/Lv{level}");
            if(Shooter == null)
            {
                Debug.LogError($"트룹의 슈터가 지정되지 않음 경로 : Prefab/Player/Shooter/Troop/Lv{level}");
            }
            GameObject.Instantiate(Shooter, instantTransform);
        }
    }

    // 초기 spawnXpos를 계산하는 메서드
    private float GetInitialSpawnXpos()
    {

        float spawnXpos = instantPoint.position.x - (projNum / 2) * space;

        // X 위치를 -2.5에서 2.5 사이로 제한
        if (spawnXpos < -2.5f)
        {
            spawnXpos = -2.5f;
        }
        else if (spawnXpos + (projNum * space) > 2.5f) //생성지점부터 생성개수*오프셋값을 더하여 끝점의 위치를 알아내고 비교
        {
            spawnXpos = 2.5f - (projNum * space);
        }

        return spawnXpos;
    }


    public override void SetLevel()
    {
        Skill_LevelValue lv1 = new Skill_LevelValue()
        {
            level = 1,
            ProjNum = 1,
            ProjSpeed = 5,
            LiveTime = 5,
            DamageRate = 50
        };

        SkillLevels.Add(lv1.level, lv1);

        Skill_LevelValue lv2 = new Skill_LevelValue()
        {
            level = 2,
            ProjNum = 3,
            ProjSpeed = 5,
            LiveTime = 7.5f,
            DamageRate = 75
        };
        SkillLevels.Add(lv2.level, lv2);

        Skill_LevelValue lv3 = new Skill_LevelValue()
        {
            level = 3,
            ProjNum = 5,
            ProjSpeed = 5,
            LiveTime = 10,
            DamageRate = 100
        };
        SkillLevels.Add(lv3.level, lv3);

        Debug.Log($"{SkillCode}의 레벨 {SkillLevels.Count}개 등록");
    }

}
