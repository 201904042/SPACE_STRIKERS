using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAttack : MonoBehaviour
{
    public GameObject enemyProjectile;

    [SerializeField]
    private bool isforwardShot;
    [SerializeField]
    private bool isTargetShot;
    [SerializeField]
    private bool isAimToPlayer; 
    [SerializeField]
    private bool ismultiForwardShot;
    

    [SerializeField]
    private float defaultSpeed;
    [SerializeField]
    private int enemyProjNum;
    [SerializeField]
    private float enemyProjAngle;
    [SerializeField]
    private float defaultStraightAngle;
    

    Vector2 dirUP;
    private void Awake()
    {
        isforwardShot = false;
        ismultiForwardShot = false;
        isTargetShot = false;
        isAimToPlayer = false;
        defaultSpeed = 10f;
        enemyProjNum = 1;
        enemyProjAngle = 45;
        defaultStraightAngle = -180;
        dirUP = transform.up;
    }

    private void Update()
    {
        if (isforwardShot)
        {
            SingleShot(enemyProjectile , dirUP*defaultSpeed);
            isforwardShot = false;
        }
        if (isTargetShot)
        {
            TargetShot(enemyProjectile);
            isTargetShot = false;
        }
        if (ismultiForwardShot)
        {
            MultiShot(enemyProjectile,enemyProjNum, enemyProjAngle, defaultSpeed, isAimToPlayer, defaultStraightAngle);
            ismultiForwardShot = false;
        }
        
    }


    private void SingleShot(GameObject enemyProjectile ,Vector3 velocity )
    { 
        GameObject enemyProj = Instantiate(enemyProjectile, transform.position, Quaternion.identity);
        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
        rigid.velocity = rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
    }

    private void TargetShot(GameObject enemyProjectile)
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        SingleShot(enemyProjectile, dirToPlayer * defaultSpeed);
    }

    //projNum�� �߻��ϰ� ���� �߻�ü�� ����
    //projAngle �߻��� ���� 45, 90 , 180, 360
    //projBasicAngle�� -180�϶� default�� �Ѿ��� �Ʒ��� �߻�� Ŀ������ �����߻� �������� �������� �߻�

    private void MultiShot(GameObject enemyProjectile, int projNum, float projAngle ,float bulletSpeed = 10f, bool isAimToPlayer= false, float projBasicAngle = -180)
    {
        float angleStep = projAngle / projNum;
        float angleInit;
        if (projNum % 2 == 0) //¦��
        {
            angleInit = -projBasicAngle - projAngle / 2 + angleStep / 2;
        }
        else // Ȧ��
        {
            angleInit = -projBasicAngle - (angleStep*(projNum/2));
        }

        for (int i = 0; i < projNum; i++)
        {
            float angle = angleInit + angleStep*i;
            if (isAimToPlayer) 
            {
                //������ ���
                Vector3 dirToPlayer;
                Transform player = GameObject.FindWithTag("Player").transform;
                dirToPlayer = (player.position - transform.position).normalized;

                Vector3 velocity = Quaternion.Euler(0, 0, angle) * - dirToPlayer;
                SingleShot(enemyProjectile, velocity * bulletSpeed);
            }
            else
            {
                //������ �ƴҰ��
                float projectileDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float projectileDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
                Vector2 projectileMoveDirection = (projectileVector - (Vector2)transform.position).normalized * bulletSpeed;
                Vector3 velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y,0);
                SingleShot(enemyProjectile, velocity);
            }



    }
    }

    

}
