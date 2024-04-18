using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    private PlayerStat player_stat;
    private SpriteRenderer player_sprite;


    private float speed;
    private bool isTop;
    private bool isBottom;
    private bool isLeft;
    private bool isRight;

    private Vector3 targetPosition;
    private Vector3 curPosition;
    private float basic_speed;

    public bool is_invincible;
    public bool is_moveable;

    private float invincibleDuration = 3f;

    // Update is called once per frame
    private void Awake()
    {
        player_stat = transform.GetComponent<PlayerStat>();
        player_sprite = transform.GetComponent<SpriteRenderer>();
        basic_speed = 3;
        speed = basic_speed + (player_stat.move_speed/5);

        is_invincible = false;
        is_moveable = true;
    }
    void Update()
    {
        
        speed = basic_speed + (player_stat.move_speed / 5);
        if (is_moveable)
        {
            Move();
        }
        invincible_sprite();
    }

    private void Move()
    {
        
        float h = Input.GetAxisRaw("Horizontal");
        if ((isRight && h == 1) || (isLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTop && v == 1) || (isBottom && v == -1))
            v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    private void invincible_sprite() //�÷��̾ ���� ��忩�ο� ���� ���� ����
    {
        if (is_invincible)
        {
            player_sprite.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            player_sprite.color = new Color(1, 1, 1, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border") //������ �΋H���� �ش� ������ �΋H���� �ִٴ°� ����
        {
            switch(collision.gameObject.name) {
                case "Top":
                    isTop = true; break;
                case "Bottom": 
                    isBottom = true; break;
                case "Right":
                    isRight = true; break;
                case "Left":
                    isLeft = true; break;
            }
        }

        //���̳� �� �߻�ü�� ��� ��� �ִ�
        if(collision.gameObject.tag == "Enemy") 
        {
            player_stat.Player_damaged(collision.GetComponent<Enemy>().Enemy_Damage, collision.gameObject);
        }
    }
    public void player_Attacked(GameObject attack_obj)
    {
        Collider2D e_obj_collider = attack_obj.GetComponent<Collider2D>();
        if (is_invincible == false)
        {
            if (e_obj_collider != null)
            {
                is_invincible = true;
                StartCoroutine(player_invincible(invincibleDuration));
                player_push(e_obj_collider);
            }
        }
    }
    
    public void player_push(Collider2D collision)
    {
        curPosition = transform.position;
        targetPosition = cal_targetposition(curPosition, collision);

        StartCoroutine(pushbackLerp(curPosition, targetPosition));
    }

    public Vector3 cal_targetposition(Vector3 curpos,Collider2D col) //�ڷ� �и��� �����
    {
        Vector2 pushDirection = (curpos - col.transform.position).normalized;
        return curpos - new Vector3(-pushDirection.x, -pushDirection.y, 0) * 2;
    }

    IEnumerator pushbackLerp(Vector3 startPos, Vector3 endPos)
    {
        float startTime = Time.time;
        float elapseTime = 0f;

        while(elapseTime < 0.1f)
        {
            if (isTop || isBottom || isLeft || isRight)
            {
                break;
            }
            float t = Mathf.Clamp01(elapseTime / 0.1f); // �ð��� ��� ����
            transform.position = Vector3.Lerp(startPos, endPos, t); // ���� ������ ����Ͽ� �÷��̾� ��ġ ������Ʈ
            elapseTime = Time.time - startTime;
            yield return null;
        }
    }

    IEnumerator player_invincible(float invincible_time)
    {
        if(is_invincible == false)
        {
            yield break;
        }

        float inv_time = invincible_time;
        
        while (inv_time > 0)
        {
            inv_time-= Time.deltaTime;

            yield return null;
        }
        is_invincible = false;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTop = false; break;
                case "Bottom":
                    isBottom = false; break;
                case "Right":
                    isRight = false; break;
                case "Left":
                    isLeft = false; break;
            }
        }
    }
}
