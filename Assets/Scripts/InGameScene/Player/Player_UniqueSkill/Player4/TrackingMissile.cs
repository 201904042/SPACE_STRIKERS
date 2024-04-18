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
    private Player_specialSkill speicalScript;
    private float damage;

    GameObject[] enemies;
    GameObject targetEnemy;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private Vector2 basic_endPosition; //���� ���� ����� ��������Ʈ
    private Vector2 handlePosition; // �� ���� �ڵ� ����Ʈ

    public float handleDistanceMultiplier = 1.0f; // �ڵ� ����Ʈ�� �Ÿ��� �����ϴ� ���
    public float speed = 10f;
    private bool targetSet;
    private float t = 0.0f;
    private int random;
    private bool onHit;
    protected override void Awake()
    {
        base.Awake();
        onHit = false ;
        speicalScript = player.GetComponent<Player_specialSkill>();
        damage = speicalScript.special_Damage*2;

        startPosition = player.transform.position; //��������
        basic_endPosition = startPosition + Vector2.up * 20;
        handlePosition = startPosition + Vector2.right * Random.Range(-3, 4); //�������� ����
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length != 0) //���� ����
        {
            random = Random.Range(0, enemies.Length);
            targetEnemy = enemies[random];
            endPosition = targetEnemy.transform.position; //���� �ִٸ� ���� ��ġ��

            targetSet = true;
        }
        else //���������� ���� ����
        {
            targetEnemy = null;
            endPosition = basic_endPosition;
            targetSet = false;
        }

    }

    void Update()
    {
        Vector2 curpos = new Vector2(transform.position.x, transform.position.y);

        if (curpos == endPosition && onHit == false) //���� ��ġ�� ������ Ÿ������ ���� -> Ÿ���� ��ġ�� �缳��
        {
            endPosition = basic_endPosition;
        }

        if (targetSet == true)
        {
            if (targetEnemy != null)
            {
                targetSet = false; //���� ���߿� �������� -> Ÿ�ټ����� �������� �ΰ� ��ǥ��ġ�� �⺻��ǥ��ġ��
            }
        }

        if (!targetSet)  //Ÿ���� �������� �ʾ����� 
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length != 0) //���� �ٽ� ������ ���
            {
                endPosition = enemies[Random.Range(0, enemies.Length)].transform.position;
                random = Random.Range(0, enemies.Length);
                targetSet = true;
            }
            else
            {
                endPosition = basic_endPosition;
            }
        }

        

        t += Time.deltaTime * speed;
        if (t > 1.0f)
            t = 1.0f;

            // ������ ����� ���� ��ġ ���
        Vector2 newPosition = BezierCurve(startPosition, handlePosition, endPosition, t);

            // ������Ʈ �̵�
        transform.position = newPosition;
                                
    }

    // ������ ��� ����ϴ� �Լ�
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
            collision.GetComponent<Enemy>().Enemydamaged(damage, gameObject);
            onHit = true;
            Destroy(gameObject);
        }
        
    }
}

