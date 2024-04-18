using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    EnemyStat enemy_stat;
    public int Enemy_ID;
    public string Enemy_Name;
    public string Enemy_Grade;
    public bool Enemy_MoveAttack;
    public bool Enemy_IsAiming;
    public float Enemy_MaxHP;
    public float Enemy_CurHP;
    public float Enemy_Damage;
    public float Enemy_MoveSpeed;
    public float Enemy_AttackSpeed;
    public float exp_amount;
    public float score_amount;
    public bool Attack_Type; //1 : ���� , 0 : ����
    public bool Can_Attack;
    public bool is_Slow;
    
    public GameObject exp;
    public GameObject e_hpBar;
    
    private GameObject canvas;
    private GameObject hpBar_instance;
    private Slider hpSlider;
    private RectTransform hpBar;

    public GameObject e_DamageRate;
    private GameObject DamageRate_instance;
    private TextMeshProUGUI TMPro;



    private void Awake()
    {
        enemy_stat = GetComponent<EnemyStat>();
        EnemyStat_set();
        hpBar_set();
    }
    void EnemyStat_set()
    {
        Enemy_ID = enemy_stat.cur_EnemyID;
        Enemy_Name = enemy_stat.Name;
        gameObject.name = Enemy_Name;
        Enemy_Grade = enemy_stat.Grade;
        Enemy_MoveAttack = enemy_stat.MoveAttack;
        Enemy_IsAiming = enemy_stat.is_Aiming;
        Enemy_MaxHP = enemy_stat.MaxHP;
        Enemy_CurHP = Enemy_MaxHP;
        Enemy_Damage = enemy_stat.Damage;
        Enemy_MoveSpeed = enemy_stat.MoveSpeed;
        Enemy_AttackSpeed = enemy_stat.AttackSpeed;
        exp_amount = enemy_stat.exp_amount;
        score_amount = enemy_stat.score_amount;
        Can_Attack = true;
        is_Slow = false;
    }

    private void hpBar_set()
    {
        canvas = GameObject.Find("Canvas");
        hpBar_instance = Instantiate(e_hpBar, canvas.transform);
        hpBar = hpBar_instance.GetComponent<RectTransform>();
        hpSlider = hpBar_instance.GetComponent<Slider>();
        hpBar.sizeDelta = new Vector2(transform.localScale.x * 200, hpBar.sizeDelta.y);

        if(gameObject.name == "sandBag")
        {
            DamageRate_instance = Instantiate(e_DamageRate, canvas.transform);
            TMPro = DamageRate_instance.GetComponent<TextMeshProUGUI>();

            TMPro.text = Enemy_CurHP.ToString();
        }
    }

    private void Update()
    {
        hpBar_update();
        

        if (Enemy_CurHP <= 0)
        {
            EnemyDeath();
        }
    }
    
    void hpBar_update() //hp���� ��ġ�� ������Ʈ
    {
        Vector3 hpBar_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y-(transform.localScale.y), 0));
        hpBar.position = hpBar_pos;

        if(DamageRate_instance != null)
        {
            Vector3 DamageRate_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y) * 2, 0));
            DamageRate_instance.transform.position = DamageRate_pos;
            TMPro.text = Enemy_CurHP.ToString();
        }

        hpSlider.value = Enemy_CurHP/Enemy_MaxHP;
        

    }
    public void EnemyEliminate()
    {
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
        if (DamageRate_instance != null)
        {
            Destroy(DamageRate_instance.gameObject);
        }
    }
    public void EnemyDeath() //�� �����
    {
        for(int i=0; i<exp_amount; i++)
        {
            Instantiate(exp, transform.position, transform.rotation); //�����Ǵ� ��ġ�� ���ݾ� ���̰� ������ �ٲ㺸��
        }
        
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
        if(DamageRate_instance != null)
        {
            Destroy(DamageRate_instance.gameObject);
        }
        
        //���ھ� ���� �ڵ� �߰��Ұ�
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerStat>().Player_damaged(Enemy_Damage / 2, gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "BulletBorder")
        {
            EnemyEliminate();
        }
    }

    public void Enemydamaged(float damage, GameObject attackObj)
    {
        Enemy_CurHP -= damage;
        Debug.Log(attackObj + " �� ���� " + damage + " �� �������� ����");
    }
}
