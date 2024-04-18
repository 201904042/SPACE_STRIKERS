using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class skill_MiniDrone : MonoBehaviour
{
    public GameObject proj;
    private GameObject[] enemies;

    private GameObject target;
    private GameObject player;
    private PlayerStat stat;
    Vector3 targetPosition;

    private float drone_damage;
    public float damageRate;
    private float shootSpeed;
    public float shootSpeedRate;
    private float moveSpeed;

    public float shootTimer;

    private float liveTime;

    private float attackRange;
    private bool is_enemySet;
    private bool is_firstSet;

    private float basic_speed;

    void Awake()
    {
        player = GameObject.Find("Player");
        stat = player.GetComponent<PlayerStat>();

        liveTime = 15f;
        attackRange = 5f;
        is_enemySet = false;
        drone_damage = stat.damage * damageRate;
        basic_speed = 1;
        shootSpeed = basic_speed - shootSpeedRate;
        moveSpeed = 10f;
        shootTimer = shootSpeed;

        setTarget();
    }

    // Update is called once per frame
    void Update()
    {
        liveTime -= Time.deltaTime;
        if (liveTime < 0)
        {
            Destroy(gameObject);
        }
        if (!is_firstSet)
        {
            drone_damage = stat.damage * damageRate;
            shootSpeed = 1;
            moveSpeed = 10f;
            shootTimer = shootSpeed;
            is_firstSet = true;
        }

        if (!is_enemySet) //타겟이 정해지지 않으면 다시 타겟을 정함
        {
            setTarget();
            returnToPlayer();
        }
        else // 타겟이 정해짐
        {
            if (target != null && Vector2.Distance(player.transform.position, target.transform.position) > attackRange)
            {
                is_enemySet = false;
            }
            else
            {
                if (target != null) //타겟이 존재한다면
                {
                    targetPosition = target.transform.position + new Vector3(0, -1, 0);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                    if (transform.position == targetPosition)
                    {
                        shootTimer -= Time.deltaTime;
                        if (shootTimer < 0)
                        {
                            attack();

                            shootTimer = shootSpeed;
                        }

                    }
                }
                else //어떤 이유로 타겟이 없다면
                {
                    is_enemySet = false;
                    returnToPlayer();
                    setTarget();
                }
            }

        }
    }

    void setTarget()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] inRangeEnemies = new GameObject[4];
        int count = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            float dist = Vector2.Distance(enemies[i].transform.position, player.transform.position);
            if (dist <= attackRange)
            {
                inRangeEnemies[count] = enemies[i];
                count++;
            }
        }
        if (count > 0)
        {
            target = inRangeEnemies[Random.Range(0, count)];
            is_enemySet = true;
        }
        else
        {
            target = null;
            is_enemySet = false;
        }

    }

    void returnToPlayer()
    {

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
    }

    void attack()
    {
        GameObject droneBullet = Instantiate(proj, transform.position, Quaternion.identity);
        droneBullet.GetComponent<skill_MiniDroneBullet>().damage = drone_damage;
    }

}
