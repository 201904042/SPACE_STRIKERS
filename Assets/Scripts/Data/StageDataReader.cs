using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;


public class StageDataReader
{
    public Dictionary<int, StageData> stageDataDic;

    public void LoadData()
    {
        stageDataDic = DataManager.SetDictionary<StageData, StageDatas>("JSON/ReadOnly/stageData",
            data => data.StageData,
            item => item.stageCode
            );
    }

    public StageData? GetData(int targetId)
    {
        if (!stageDataDic.ContainsKey(targetId))
        {
            Debug.Log($"해당 아이디 없음");
            return null;
        }
        return stageDataDic[targetId];
    }

}


