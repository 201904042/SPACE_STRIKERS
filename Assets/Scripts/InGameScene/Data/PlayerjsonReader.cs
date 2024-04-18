using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerjsonReader;

public class PlayerjsonReader : MonoBehaviour
{
    public TextAsset PlayertextJSON;
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

    public class PlayerList
    {
        public Player[] player; //json파일의 분류의 이름과 동일한지 주의
    }

    [SerializeField]
    public PlayerList myPlayerList = new PlayerList();

    public void LoadData()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(PlayertextJSON.text);
    }

}
