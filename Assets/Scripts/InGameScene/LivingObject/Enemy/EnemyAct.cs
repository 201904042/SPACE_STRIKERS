//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class EnemyAct : EnemyObject
//{
//    public static void SingleShot(EnemyObject enemy,Vector3 velocity, bool split = false, int splitCount = 3)
//    {
//        GameObject enemyProj;

//        if (split)
//        {
//            enemyProj = GameManager.Game.Pool.GetOtherProj(OtherProjType.Enemy_Split, enemy.transform.position, Quaternion.identity);
//        }
//        else
//        {
//            enemyProj = GameManager.Game.Pool.GetOtherProj(OtherProjType.Enemy_Bullet, enemy.transform.position, Quaternion.identity);
//        }

//        if (enemyProj == null)
//        {
//            Debug.Log("�Ѿ��� �ҷ����� ����");
//            return;
//        }
        

//        Rigidbody2D rigid = enemyProj.GetComponent<Rigidbody2D>();
//        rigid.velocity = Vector2.zero;

//        if (split)
//        {
//            //enemyProj.GetComponent<EnemySplitBullet>().splitCount = splitCount;
//        }
//        else
//        {
//            //enemyProj.GetComponent<EnemyBullet>().SetDamage(enemy.enemyStat.damage);
//        }

//        rigid.velocity = new Vector2(velocity.x, velocity.y); //fireDirection * bulletSpeed;
//    }


//    //�߻��� �Ѿ� ������, �п��Ѿ��̶�� true
//    public static void TargetShot(EnemyObject enemy,  bool split = false, float speed = 1, int splitCount = 3)
//    {   
//        Transform player = PlayerMain.Instance.transform;
//        Vector3 dirToPlayer = (player.position - enemy.transform.position).normalized;
//        if (split)
//        {
//            SingleShot(enemy,dirToPlayer * speed, true, splitCount);
//        }
//        else
//        {
//            SingleShot(enemy, dirToPlayer * speed);
//        }

//    }

//    //�߻��� �Ѿ�, �Ѿ˰���, ��ݰ�������, �Ѿ˼ӵ�, ���ؿ���, �߻�ü�� �⺻ �ޱ�, �п��Ѿ˿���
//    public static void BulletAttack(EnemyObject enemy, int projNum, float projAngle, float bulletSpeed = 10f, bool isAimToPlayer = false, float projBasicAngle = -180, bool split = false, int splitCount = 3)
//    {
//        //Debug.Log($"�� ����" +
//           // $"projCount {projCount} projAngle {projAngle}  bulletSpeed {bulletSpeed},  isAimToPlayer {isAimToPlayer} , projBasicAngle {projBasicAngle} , split {split}");
//        float angleStep = projAngle / projNum;
//        float angleInit;
//        if (projNum % 2 == 0) //¦��
//        {
//            angleInit = -projBasicAngle - projAngle / 2 + angleStep / 2;
//        }
//        else // Ȧ��
//        {
//            angleInit = -projBasicAngle - (angleStep * (projNum / 2));
//        }

//        for (int i = 0; i < projNum; i++)
//        {
//            float angle = angleInit + angleStep * i;
//            if (isAimToPlayer)
//            {
//                //������ ���
//                Vector3 dirToPlayer;
//                Transform player = PlayerMain.Instance.transform;
//                dirToPlayer = (player.position - enemy.transform.position).normalized;

//                Vector3 velocity = Quaternion.Euler(0, 0, angle) * -dirToPlayer;
//                if (split)
//                {
//                    SingleShot(enemy, velocity * bulletSpeed, true, splitCount);
//                }
//                else
//                {
//                    SingleShot(enemy, velocity * bulletSpeed);
//                }
//            }
//            else
//            {
//                //������ �ƴҰ��
//                float projectileDirXPosition = enemy.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
//                float projectileDirYPosition = enemy.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

//                Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
//                Vector2 projectileMoveDirection = (projectileVector - (Vector2)enemy.transform.position).normalized * bulletSpeed;
//                Vector3 velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);
//                if (split)
//                {
//                    SingleShot(enemy, velocity, true,splitCount);
//                }
//                else
//                {
//                    SingleShot(enemy, velocity);
//                }
//            }
//        }
//    }

//    //������ �������� ����� �ޱ�(�⺻ 0), ���ؿ���
//    public static void Laser(EnemyObject enemy,float multiAngle = 0, bool isAimtoPlayer = false)
//    {
//        EnemyLaser laserObject = GameManager.Game.Pool.GetOtherProj(OtherProjType.Enemy_Laser, enemy.transform.position, Quaternion.identity).GetComponent<EnemyLaser>();
//        //laserObject.SetDamage(enemy.enemyStat.damage);
//        if (isAimtoPlayer)
//        {
//            Transform player = PlayerMain.Instance.transform;
//            Vector3 direction = player.position - laserObject.gameObject.transform.position;
//            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//            laserObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90 - multiAngle));
//        }
//        else
//        {
//            laserObject.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, multiAngle));
//        }

//        laserObject.SetLaser(enemy.gameObject);
//    }

//    //�߻簳��, �߻� �ִ����, ���ؿ��� , �⺻�������ޱ�(�⺻ 0)
//    public static void MultiLaser(EnemyObject enemy, int projNum, float projAngle, bool isAimToPlayer = false, float projBasicAngle = 0)
//    {
//        float angleStep = projAngle / projNum;
//        float angleInit;
//        if (projNum % 2 == 0) //¦��
//        {
//            angleInit = projBasicAngle - projAngle / 2 + angleStep / 2;
//        }
//        else // Ȧ��
//        {
//            angleInit = projBasicAngle - (angleStep * (projNum / 2));
//        }

//        for (int i = 0; i < projNum; i++)
//        {
//            float angle = angleInit + angleStep * i;
//            if (isAimToPlayer)
//            {
//                Debug.Log(isAimToPlayer);
//                Laser(enemy,angle, isAimToPlayer);
//            }
//            else
//            {
//                Laser(enemy, angle, isAimToPlayer);
//            }
//        }
//    }


//    public static void EnemyMoveForward(GameObject movingObject)
//    {
//        Vector2 moveDirection = movingObject.transform.up;
//        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();
//        EnemyObject movingObjectStat = movingObject.GetComponent<EnemyObject>();
//        enemyRigid.velocity = moveDirection * movingObjectStat.enemyStat.moveSpeed;
//    }

//    public static void EnemyMoveStop(GameObject movingObject)
//    {
//        Rigidbody2D enemyRigid = movingObject.GetComponent<Rigidbody2D>();

//        enemyRigid.velocity = Vector2.zero;
//    }



//    protected override void OnTriggerEnter2D(Collider2D collision)
//    {
//        base.OnTriggerEnter2D(collision);
//    }
//}
