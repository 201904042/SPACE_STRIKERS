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
        public string e_grade; //���� ��� common,elite,midBoss, Boss
        public string e_name;
        public bool e_move_attack; //false�� ���缭 ���� true�� �̵��ϸ鼭 ����
        public bool e_is_aiming; //true�� �����Ͽ� ��� false�� �׳� ������ ���� ���
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
