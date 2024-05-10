using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerjsonReader;

[System.Serializable]
public class Player
{
    public int id;
    public string name;
    public int level;
    public float damage;
    public float defence;
    public float move_speed;
    public float attack_speed;
    public float hp;
}

[System.Serializable]
public class PlayerList
{
    public Player[] player; //json파일의 분류의 이름과 동일한지 주의
}

public class PlayerjsonReader : MonoBehaviour
{
    public TextAsset PlayertextJSON;
    public PlayerList myPlayerList = new PlayerList();

    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (PlayertextJSON != null)
        {
            string json = PlayertextJSON.text;
            myPlayerList = JsonUtility.FromJson<PlayerList>(json);

        }
        else
        {
            Debug.LogError("No JSON file assigned.");
        }
    }


}
