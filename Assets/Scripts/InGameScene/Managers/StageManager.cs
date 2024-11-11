using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.EditorTools;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Rendering;

public class StageManager
{
    [Header("�������� ����")]
    public GameMode curMode;
    public int planet;
    public int stage;
    
    public Dictionary<int, int> enemyCodeAmountFair = new Dictionary<int, int>(); //���� �ڵ�, ��
    public List<StageReward> ClearReward = new List<StageReward>();
    public int curEnemyAmount;
    public int stageBossId;

    public void Init()
    {
        SetStageData();
    }

    /// <summary>
    /// ���������� �ڵ带 �޾� �������� ���� ���� ������ ����Ʈ�� ������Ʈ ��
    /// </summary>
    public void SetStageData()
    {
        AccountData account = DataManager.account.GetData(); //��ī��Ʈ�� ����� �����Ϳ��� �÷����� �༺��ȣ�� �������� ��ȣ�� ����

        //�������� �ڵ� ����
        planet = account.planetIndex;
        stage = account.stageIndex;
        int StageCode = (planet-1) * 10 + stage;

        StageData curStageData = DataManager.stage.GetData(StageCode);

        curMode = StageCode > account.stageProgress 
            ? curStageData.stageType : GameMode.Infinite;

        curEnemyAmount = 0;
        foreach (StageEnemyData enemyInfo in curStageData.stageEnemy) //���������� �� ���
        {
            enemyCodeAmountFair.Add(enemyInfo.enemyId,enemyInfo.quantity);
            if (enemyInfo.enemyId > 20)
            {
                stageBossId = enemyInfo.enemyId; //������ curEnemyAmount�� ������ ���Ե��� ����
                continue;
            }
            curEnemyAmount += enemyInfo.quantity;
        }

        //�������� ���� ���
        var rewards = (curMode == GameMode.Infinite) ? curStageData.defaultReward : curStageData.firstReward;
        ClearReward.AddRange(rewards);
    }

    /// <summary>
    /// �� ����� ����. ���������� ��ϵ� ���� ���� ����. ���Ѹ���� ��쿡�� �ʿ� ����
    /// </summary>
    /// <param name="enemyId"></param>
    public void DiscountEnemyQuantity(int enemyId)
    {
        enemyCodeAmountFair[enemyId] -= 1;
        curEnemyAmount -= 1;
    }
}
