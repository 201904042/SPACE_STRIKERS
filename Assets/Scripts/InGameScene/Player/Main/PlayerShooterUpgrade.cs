using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class playerShooterUpgrade : MonoBehaviour
{
    private PlayerStat pStat => PlayerMain.pStat;
    private Transform InstanceTransform;
    public int shooterLevel 
    {
        get => pStat.weaponLevel;
        set
        {
            if(shooterLevel  < 3)
            {
                pStat.weaponLevel = value;
                SetShooter(charId, value);
            }
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


    public void Init()
    {
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
        if (InstanceTransform.childCount != 0)
        {
            Destroy(InstanceTransform.GetChild(0).gameObject);
        }

        GameObject shooter = Resources.Load<GameObject>($"Prefab/Player/Shooters/{playerId}/{shooterId}");
        if (shooter == null)
        {
            Debug.Log($"플레이어 슈터 : 잘못된 경로 할당 p : {playerId}/ lv : {shooterId} ");
        }
        else
        {
            Instantiate(shooter, InstanceTransform);
        }
    }
}
