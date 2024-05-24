using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class playerShooterUpgrade : MonoBehaviour
{
    GameObject player;

    public int shooterLevel;
    public int characterNumber;
    private string rootPath;
    private string characterPathName;
    private string curLevelPath;
    private string spritePath;

    private PlayerStat playerStatScript;

    void Start()
    {
        player = transform.parent.gameObject;
        playerStatScript = player.GetComponent<PlayerStat>();
        shooterLevel = 1;
        characterNumber = playerStatScript.curPlayerID;
        UpdatePlayerId();
        
        rootPath = "Assets/Prefabs/Player/Player_Shooter/";
        characterPathName = ChangeShooterPathCharacter(characterNumber);
        curLevelPath = ChangeShooterPathLevel(shooterLevel);
        LoadShooterPrefab();
        UpdatePlayerId();
    }

    void Update()
    {
        characterPathName = ChangeShooterPathCharacter(characterNumber);
        curLevelPath = ChangeShooterPathLevel(shooterLevel);
        
    }
    private void LoadShooterPrefab()
    {
        GameObject shooter = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(rootPath + characterPathName + curLevelPath + ".prefab");
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
        playerStatScript.curPlayerID = characterNumber;
        playerStatScript.SetStat(characterNumber);
    }

    private string ChangeShooterPathCharacter(int BS_num)
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

    private string ChangeShooterPathLevel(int shooter_level)
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

    private void ChangeCharacter()
    {
        ChangePlayerImage(characterNumber);
        GameObject oldShooter = transform.GetChild(0).gameObject;
        if (oldShooter != null)
        {
            Destroy(oldShooter);
        }
        characterPathName = ChangeShooterPathCharacter(characterNumber);
        curLevelPath = ChangeShooterPathLevel(shooterLevel);

        GameObject shooter = AssetDatabase.LoadAssetAtPath<GameObject>(rootPath + characterPathName + curLevelPath + ".prefab");
        if (shooter == null)
        {
            Debug.Log("load fail");
        }
        else
        {
            Instantiate(shooter, transform);
        }
    }

    private void ChangePlayerImage(int number)
    {
        SpriteRenderer player_sprite = transform.GetComponentInParent<SpriteRenderer>();
        switch (number)
        {
            case 1:
                spritePath = "Assets/Sprites/Player/Basic.png";
                break;
            case 2:
                spritePath = "Assets/Sprites/Player/Bomber.png";
                break;
            case 3:
                spritePath = "Assets/Sprites/Player/Tanker.png";
                break;
            case 4:
                spritePath = "Assets/Sprites/Player/Splash.png";
                break;
        }
        player_sprite.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    //debug buttons
    public void ShooterUPBtn()
    {
        if (shooterLevel < 6)
        {
            shooterLevel += 1;
        }

        ChangeCharacter();
    }

    public void ShooterDownBtn()
    {
        if (shooterLevel > 1)
        {
            shooterLevel -= 1;
        }

        ChangeCharacter();
    }

    public void NextBtn()
    {
        if (characterNumber < 4)
        {
            characterNumber += 1;
        }
        else
        {
            characterNumber = 1;
        }
        UpdatePlayerId();
        ChangeCharacter();
    }
    public void PrevBtn()
    {
        if (characterNumber > 1)
        {
            characterNumber -= 1;
        }
        else
        {
            characterNumber = 4;
        }
        UpdatePlayerId();
        ChangeCharacter();
    }
}
