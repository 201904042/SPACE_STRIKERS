using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Troop : MonoBehaviour
{
    private GameObject player;
    private Player_specialSkill p_specialskill;
    private float fireTime;
    private float startTimer;
    private string root_path;
    private string cur_level_path;
    public float slow_yPos = -2f;
    public float speed = 5f;
    private int skill_level;

    private void Awake()
    {
        root_path = "Assets/Prefabs/Player/Player_UniqueSkill/Troop_Shooter/shooter_";
        player = GameObject.Find("Player");
        p_specialskill = player.GetComponent<Player_specialSkill>();
        fireTime = p_specialskill.special_FireTime;
        skill_level = p_specialskill.power_level;
        cur_level_path = change_shooter_path_level(skill_level);
        InstantShooter();
    }
    private void Update()
    {
        
        if (transform.position.y > slow_yPos ) //속도 감소
        {
            speed = 0.2f;
        }
        startTimer += Time.deltaTime;
        if(startTimer > fireTime)
        {
            //특수공격 종료
            speed = 5f;
            p_specialskill.special_Active = false;
        }
        transform.Translate(Vector3.up* speed * Time.deltaTime); //이동
    }
    private string change_shooter_path_level(int shooter_level)
    {
        switch (shooter_level)
        {
            case 1:
                return "lv1";
            case 2:
                return "lv2";
            case 3:
                return "lvMax";
            default:
                return "lvMax";
        }
    }
    void InstantShooter()
    {
        GameObject shooter = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(root_path + cur_level_path + ".prefab");
        if (shooter == null)
        {
            Debug.Log("load fail");

        }
        else
        {
            Instantiate(shooter, transform);
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.tag =="Enemy_Projectile")
        {
            Destroy(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TroopBorder")
        {
            Destroy(gameObject);
        }
    }
}
