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
            case 101:
                return "Basic_Shooter/basic_";
            case 102:
                return "Bomber_Shooter/bomber_";
            case 103:
                return "Tanker_Shooter/tanker_";
            case 104:
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
                return "lvMax";
            default:
                return "lvMax";
        }
    }

    //슈터의 오브젝트 풀도 만들어보기
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
            case 101:
                spritePath = "Assets/Sprites/Player/Basic.png";
                break;
            case 102:
                spritePath = "Assets/Sprites/Player/Bomber.png";
                break;
            case 103:
                spritePath = "Assets/Sprites/Player/Tanker.png";
                break;
            case 104:
                spritePath = "Assets/Sprites/Player/Splash.png";
                break;
        }
        player_sprite.sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
    }

    //debug StoreBtns
    public void ShooterUPBtn()
    {
        if (shooterLevel < 3)
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
        if (characterNumber < 104)
        {
            characterNumber += 1;
        }
        else
        {
            characterNumber = 101;
        }
        UpdatePlayerId();
        ChangeCharacter();
    }
    public void PrevBtn()
    {
        if (characterNumber > 101)
        {
            characterNumber -= 1;
        }
        else
        {
            characterNumber = 104;
        }
        UpdatePlayerId();
        ChangeCharacter();
    }
}
