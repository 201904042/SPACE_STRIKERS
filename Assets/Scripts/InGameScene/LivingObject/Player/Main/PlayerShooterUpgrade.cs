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
        get => pStat.IG_WeaponLv;
        set
        {
            if(shooterLevel  < 3)
            {
                pStat.IG_WeaponLv = value;
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

    

    private void SetShooter(int playerId, int shooterId)
    {
        if (InstanceTransform.childCount != 0)
        {
            Destroy(InstanceTransform.GetChild(0).gameObject);
        }

        GameObject shooter = Resources.Load<GameObject>($"Prefab/Player/Shooters/{playerId}/Lv{shooterId}");
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
