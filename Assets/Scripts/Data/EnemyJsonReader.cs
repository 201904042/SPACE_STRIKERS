using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class EnemyJsonReader : MonoBehaviour
{
    private string filePath = "Assets/JSON_Data/enemy_data.json";
    public EnemyList EnemyList = new EnemyList();

    protected virtual void Awake()
    {
        string json = File.ReadAllText(filePath);
        EnemyList = JsonUtility.FromJson<EnemyList>(json);
    }
}
