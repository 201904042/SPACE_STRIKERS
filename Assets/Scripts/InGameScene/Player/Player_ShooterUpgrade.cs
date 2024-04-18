using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class player_ShooterUpgrade : MonoBehaviour
{
    GameObject player;
    public int shooter_level;
    public int battleShip_number;
    private string root_path;
    private string battleShip_path_name;
    private string cur_level_path;
    private string sprite_path;

    private PlayerStat playerStatScript;

    void Start()
    {
        player = GameObject.Find("Player");
        playerStatScript = player.GetComponent<PlayerStat>();
        shooter_level = 1;
        battleShip_number = playerStatScript.cur_playerID;
        UpdatePlayerId();
        
        root_path = "Assets/Prefabs/Player/Player_Shooter/";
        battleShip_path_name = change_shooter_path_battlship(battleShip_number);
        cur_level_path = change_shooter_path_level(shooter_level);
        LoadShooterPrefab();
        UpdatePlayerId();
    }

    void Update()
    {
        battleShip_path_name = change_shooter_path_battlship(battleShip_number);
        cur_level_path = change_shooter_path_level(shooter_level);
        
    }
    private void LoadShooterPrefab()
    {
        GameObject shooter = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(root_path + battleShip_path_name + cur_level_path + ".prefab");
        if (shooter == null)
        {
            Debug.Log("load fail");

        }
        else
        {
            Instantiate(shooter, transform);
        }
    }

    private void UpdatePlayerId()
    {
        playerStatScript.cur_playerID = battleShip_number;
        playerStatScript.setStat(battleShip_number);
    }

    private string change_shooter_path_battlship(int BS_num)
    {
        switch (BS_num)
        {
            case 1:
                return "Basic_Shooter/basic_";
            case 2:
                return "Bomber_Shooter/bomber_";
            case 3:
                return "Tanker_Shooter/tanker_";
            case 4:
                return "Splash_Shooter/splash_";
            default:
                return "Basic_Shooter/basic_";
        }
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
                return "lv3";
            case 4:
                return "lv4";
            case 5:
                return "lv5";
            case 6:
                return "lvMax";
            default:
                return "lvMax";
        }
    }
    private void change_BS()
    {
        //playerStatScript.setStat(battleShip_number);
        change_player_image(battleShip_number);
        GameObject old_shooter = transform.GetChild(0).gameObject;
        if (old_shooter != null)
        {
            Destroy(old_shooter);
        }
        battleShip_path_name = change_shooter_path_battlship(battleShip_number);
        cur_level_path = change_shooter_path_level(shooter_level);

        GameObject shooter = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(root_path + battleShip_path_name + cur_level_path + ".prefab");
        if (shooter == null)
        {
            Debug.Log("load fail");
        }
        else
        {
            Instantiate(shooter, transform);
        }
    }
    private void change_player_image(int number)
    {
        SpriteRenderer player_sprite = transform.GetComponentInParent<SpriteRenderer>();
        switch (number)
        {
            case 1:
                sprite_path = "Assets/Sprites/Player/Basic.png";
                break;
            case 2:
                sprite_path = "Assets/Sprites/Player/Bomber.png";
                break;
            case 3:
                sprite_path = "Assets/Sprites/Player/Tanker.png";
                break;
            case 4:
                sprite_path = "Assets/Sprites/Player/Splash.png";
                break;
        }
        player_sprite.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(sprite_path);
    }
    public void level_UP()
    {
        if (shooter_level < 6)
        {
            shooter_level += 1;
        }

        change_BS();
    }
    public void level_Down()
    {
        if (shooter_level > 1)
        {
            shooter_level -= 1;
        }

        change_BS();
    }
    public void NextBtn()
    {
        if (battleShip_number < 4)
        {
            battleShip_number += 1;
        }
        else
        {
            battleShip_number = 1;
        }
        UpdatePlayerId();

        change_BS();
    }
    public void PrevBtn()
    {
        if (battleShip_number > 1)
        {
            battleShip_number -= 1;
        }
        else
        {
            battleShip_number = 4;
        }
        UpdatePlayerId();
        change_BS();
    }
}
