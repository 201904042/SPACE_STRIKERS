using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using static UnityEngine.GraphicsBuffer;

public class TrackingMissile : PlayerShoot
{
    private PlayerSpecialSkill speicalScript;
    private float damage;

    GameObject[] enemies;
    GameObject targetEnemy;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 basicEndPosition; //적이 없을 경우의 엔드포인트
    private Vector2 handlePosition; // 한 개의 핸들 포인트

    public float handleDistanceMultiplier = 1.0f; // 핸들 포인트의 거리를 조절하는 계수
    public float speed = 10f;
    private bool targetSet;
    private float t = 0.0f;
    private int random;
    private bool onHit;
    protected override void Awake()
    {
        base.Awake();
        onHit = false ;
        speicalScript = GameManager.gameInstance.myPlayer.GetComponent<PlayerSpecialSkill>();
        damage = speicalScript.specialDamage*2;

        startPosition = GameManager.gameInstance.myPlayer.transform.position; //시작지점
        basicEndPosition = startPosition + Vector2.up * 20;
        handlePosition = startPosition + Vector2.right * Random.Range(-3, 4); //굽어지는 정도
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length != 0) //적이 있음
        {
            random = Random.Range(0, enemies.Length);
            targetEnemy = enemies[random];
            endPosition = targetEnemy.transform.position; //적이 있다면 적의 위치로

            targetSet = true;
        }
        else //생성됬을때 적이 없음
        {
            targetEnemy = null;
            endPosition = basicEndPosition;
            targetSet = false;
        }

    }

    protected override void OnEnable()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

    }


    void Update()
    {
        Vector2 curpos = new Vector2(transform.position.x, transform.position.y);

        if (curpos == endPosition && onHit == false) //적의 위치에 왔지만 타격하지 못함 -> 타겟의 위치로 재설정
        {
            endPosition = basicEndPosition;
        }

        if (targetSet == true)
        {
            if (targetEnemy != null)
            {
                targetSet = false; //적이 도중에 사라질경우 -> 타겟설정을 거짓으로 두고 목표위치를 기본목표위치로
            }
        }
        if (!targetSet)  //타겟이 설정되지 않았을시 
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length != 0) //적이 다시 생겼을 경우
            {
                endPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                random = Random.Range(0, enemies.Length);
                targetSet = true;
            }
            else
            {
                endPosition = basicEndPosition;
            }
        }

        

        t += Time.deltaTime * speed;
        if (t > 1.0f)
            t = 1.0f;

            // 베지어 곡선에서 현재 위치 계산
        Vector2 newPosition = BezierCurve(startPosition, handlePosition, endPosition, t);

            // 오브젝트 이동
        transform.position = newPosition;
                                
    }

    // 베지어 곡선을 계산하는 함수
    Vector2 BezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0; // (1 - t)^3 * p0
        p += 3 * uu * t * p1; // 3 * (1 - t)^2 * t * p1
        p += 3 * u * tt * p2; // 3 * (1 - t) * t^2 * p2
        p += ttt * p2; // t^3 * p3

        return p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyObject>().EnemyDamaged(damage, gameObject);
            onHit = true;
            ObjectPool.poolInstance.ReleasePool(gameObject);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}

