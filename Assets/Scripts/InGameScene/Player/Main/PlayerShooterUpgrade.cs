using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class playerShooterUpgrade : MonoBehaviour
{
    private PlayerStat pStat;
    private Transform InstanceTransform;
    public int shooterLevel 
    {
        get => pStat.weaponLevel;
        set
        {
            pStat.weaponLevel = value;
            SetShooter(charId, value);
        }
    }

    public int charId
    {
        get => pStat.curPlayerID;
        set{ 
            pStat.curPlayerID = value;
            pStat.ChangePlayer(value);
            SetCharSprite(value);
            SetShooter(value, shooterLevel);
        }
    }

    private string rootPath;
    private string charSpritePath;
    private string curLevelPath;
    private string spritePath;

    

    void Start()
    {
        pStat = PlayerMain.pStat;
        InstanceTransform = transform.GetChild(0);
        SetShooter(charId, shooterLevel);
    }

    private void SetCharSprite(int id)
    {
        SpriteRenderer playerSprite = GameManager.Instance.myPlayer.GetComponent<SpriteRenderer>();
        Sprite charSprite = Resources.Load<Sprite>($"Sprite/Player/Character/{id}");
        if(charSprite== null)
        {
            Debug.Log("캐릭터 스프라이트 : 잘못된 경로 할당");
        }

        playerSprite.sprite = charSprite;
    }

    private void SetShooter(int playerId, int shooterId)
    {
        GameObject oldShooter = InstanceTransform.GetChild(0).gameObject;
        if (oldShooter != null)
        {
            Destroy(oldShooter);
        }

        GameObject shooter = Resources.Load<GameObject>($"Prefab/Player/Shooters/{playerId}/{shooterId}");
        if (shooter == null)
        {
            Debug.Log("플레이어 슈터 : 잘못된 경로 할당");
        }
        else
        {
            Instantiate(shooter, InstanceTransform);
        }
    }

    //debug StoreBtns
    public void ShooterUPBtn()
    {
        if (shooterLevel < 3)
        {
            shooterLevel += 1;
        }
    }

    public void ShooterDownBtn()
    {
        if (shooterLevel > 1)
        {
            shooterLevel -= 1;
        }

    }

    public void NextBtn()
    {
        if (charId < 104)
        {
            charId += 1;
        }
        else
        {
            charId = 101;
        }
    }
    public void PrevBtn()
    {
        if (charId > 101)
        {
            charId -= 1;
        }
        else
        {
            charId = 104;
        }
    }
}
