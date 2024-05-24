using System.IO;
using UnityEditor;
using UnityEngine;

public class PlayerJsonReader : MonoBehaviour
{
    private string filePath = "Assets/JSON_Data/player_data.json";
    public PlayerList PlayerList = new PlayerList();

    private void Awake()
    {
        string json = File.ReadAllText(filePath);
        PlayerList = JsonUtility.FromJson<PlayerList>(json);
    }
}
