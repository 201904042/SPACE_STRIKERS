using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;


[System.Serializable]
public class StageEnemy
{
    public int enemyCode;
    public int enemyAmount;
}

[System.Serializable]
public class Item
{
    public string itemName;
    public int itemAmount;
}

[System.Serializable]
public class StageData
{
    public int stageCode;
    public StageEnemy[] stageEnemy;
    public Item[] stageFirstGain;
    public Item[] stageDefaultGain;
    public Item[] defaultFullGain;
}

[System.Serializable]
public class StageDataContainer
{
    public StageData[] stage;
}

public class StageJsonReader : MonoBehaviour
{
    public TextAsset jsonFile;
    public StageDataContainer dataContainer = new StageDataContainer();

    protected virtual void Awake()
    {
        if (jsonFile != null)
        {
            string jsonText = jsonFile.text;
            dataContainer = JsonUtility.FromJson<StageDataContainer>(jsonText);
        }
        else
        {
            Debug.LogError("JSON file is not assigned.");
        }
    }
}


