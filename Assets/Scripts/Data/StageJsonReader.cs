using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;




[System.Serializable]
public class Item
{
    public string itemName;
    public string itemType;
    public int itemCode;
    public int itemAmount;
}

[System.Serializable]
public class StageData
{
    public int stageCode;
    public int[] enemyCode;
    public Item[] stageFirstGain;
    public Item[] stageDefaultGain;
    public Item[] defaultFullGain;
}

[System.Serializable]
public class StageDataList
{
    public StageData[] stage;
}

public class StageJsonReader : MonoBehaviour
{
    private string filePath = "Assets/JSON_Data/stage_data.json";
    public StageDataList stageList = new StageDataList();

    protected virtual void Awake()
    {
        string json = File.ReadAllText(filePath);
        stageList = JsonUtility.FromJson<StageDataList>(json);
    }
}


