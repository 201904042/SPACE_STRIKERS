using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerjsonReader;

public class EnemyJsonReader : MonoBehaviour
{
    public TextAsset EnemytextJSON;

    [System.Serializable]
    public class Enemy
    {
        public int e_id;
        public string e_grade; //적의 등급 common,elite,midBoss, Boss
        public string e_name;
        public bool e_move_attack; //false면 멈춰서 공격 true면 이동하면서 공격
        public bool e_is_aiming; //true면 조준하여 사격 false면 그냥 앞으로 직선 사격
        public float e_maxHP;
        public float e_damage;
        public float e_move_speed;
        public float e_attack_speed;
        public float e_exp_amount;
        public float e_score_amount;
    }

    public class EnemyList
    {
        public Enemy[] enemy;
    }

    public EnemyList myEnemyList = new EnemyList();
    // Update is called once per frame
    void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        myEnemyList = JsonUtility.FromJson<EnemyList>(EnemytextJSON.text);
    }
}
