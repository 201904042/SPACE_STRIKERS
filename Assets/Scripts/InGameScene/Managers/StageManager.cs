using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.EditorTools;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Rendering;

public class StageManager
{
    [Header("스테이지 관련")]
    public GameMode curMode;
    public int planet;
    public int stage;
    public int stageCode;
    
    public Dictionary<int, int> enemyCodeAmountFair = new Dictionary<int, int>(); //적의 코드, 양
    public List<StageReward> ClearReward = new List<StageReward>();
    public int curEnemyAmount;
    public int stageBossId;

    public void Init()
    {
        SetStageData();
    }

    /// <summary>
    /// 스테이지의 코드를 받아 스테이지 내의 적의 데이터 리스트를 업데이트 함
    /// </summary>
    public void SetStageData()
    {
        AccountData account = DataManager.account.GetData(); //어카운트에 저장된 데이터에서 플레이할 행성번호와 스테이지 번호를 추출

        //스테이지 코드 조합
        planet = account.planetIndex;
        stage = account.stageIndex;
        stageCode = (planet-1) * 10 + stage;

        StageData curStageData = DataManager.stage.GetData(stageCode);

        curMode = stageCode > account.stageProgress 
            ? curStageData.stageType : GameMode.Infinite;

        curEnemyAmount = 0;
        foreach (StageEnemyData enemyInfo in curStageData.stageEnemy) //스테이지에 적 등록
        {
            enemyCodeAmountFair.Add(enemyInfo.enemyId,enemyInfo.quantity);
            if (enemyInfo.enemyId > 530)
            {
                stageBossId = enemyInfo.enemyId; //보스는 curEnemyAmount에 수량이 포함되지 않음
                continue;
            }
            curEnemyAmount += enemyInfo.quantity;
        }

        //스테이지 보상 등록
        var rewards = (curMode == GameMode.Infinite) ? curStageData.defaultReward : curStageData.firstReward;
        ClearReward.AddRange(rewards);
    }

    /// <summary>
    /// 적 사망시 실행. 스테이지에 등록된 적의 수를 줄임. 무한모드일 경우에는 필요 없음
    /// </summary>
    /// <param name="enemyId"></param>
    public void DiscountEnemyQuantity(int enemyId)
    {
        enemyCodeAmountFair[enemyId] -= 1;
        curEnemyAmount -= 1;
    }
}
