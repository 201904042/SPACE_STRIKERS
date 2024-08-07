using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Troop : MonoBehaviour
{
    private PlayerSpecialSkill palyerSpecialSkill;
    private float fireTime;
    private float startTimer;
    private string rootPath;
    private string curLevelPath;
    public float slowYPos = -2f;
    public float speed = 5f;
    private int skillLevel;

    private void Awake()
    {
        rootPath = "Assets/Prefabs/Player/Player_UniqueSkill/player1/shooter_";
        palyerSpecialSkill = GameManager.gameInstance.myPlayer.GetComponent<PlayerSpecialSkill>();
    }

    private void OnEnable()
    {
        startTimer = 0;
        fireTime = palyerSpecialSkill.specialFireTime;
        skillLevel = palyerSpecialSkill.powerLevel;
        curLevelPath = ChangeShooterLevelPath(skillLevel);
        InstantShooter();
    }

    private void Update()
    {
        
        if (transform.position.y > slowYPos) //속도 감소
        {
            speed = 0.2f;
        }
        startTimer += Time.deltaTime;
        if(startTimer > fireTime)
        {
            //특수공격 종료
            speed = 5f;
            palyerSpecialSkill.specialActive = false;
        }
        transform.Translate(Vector3.up* speed * Time.deltaTime); //이동
    }
    private string ChangeShooterLevelPath(int shooter_level)
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
    private void InstantShooter()
    {
        GameObject shooter = AssetDatabase.LoadAssetAtPath<GameObject>(rootPath + curLevelPath + ".prefab");
        if (shooter == null)
        {
            Debug.Log("load fail");
        }
        else
        {
            Instantiate(shooter, transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="Enemy_Projectile")
        {
            ObjectPool.poolInstance.ReleasePool(collision.gameObject);
        }

        if (collision.gameObject.tag == "TroopBorder")
        {
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }

    }
}
