using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private PlayerStat playerStat;
    private SpriteRenderer playerSprite;

    private float speed;
    private bool isTop;
    private bool isBottom;
    private bool isLeft;
    private bool isRight;

    private Vector3 targetPosition;
    private Vector3 curPosition;
    private float basicSpeed = 3f;

    public bool isMoveable;
    public bool isInvincible;

    private float invincibleDuration = 3f;

    // Update is called once per frame
    private void Awake()
    {
        playerStat = transform.GetComponent<PlayerStat>();
        playerSprite = transform.GetComponent<SpriteRenderer>();

        speed = basicSpeed + (playerStat.moveSpeed/5);

        isInvincible = false;
        isMoveable = true;
    }
    private void Update()
    {
        speed = basicSpeed + (playerStat.moveSpeed / 5);
        if (isMoveable)
        {
            Move();
        }
        InvincibleSprite();
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

    private void InvincibleSprite() //플레이어가 무적 모드여부에 따른 투명도 적용
    {
        if (isInvincible)
        {
            playerSprite.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            playerSprite.color = new Color(1, 1, 1, 1);
        }
    }

    public void PlayerAttacked(GameObject attack_obj)
    {
        Collider2D e_obj_collider = attack_obj.GetComponent<Collider2D>();
        if (isInvincible == false)
        {
            if (e_obj_collider != null)
            {
                isInvincible = true;
                StartCoroutine(playerInvincible(invincibleDuration));
                player_push(e_obj_collider);
            }
        }
    }
    
    public void player_push(Collider2D collision)
    {
        curPosition = transform.position;
        targetPosition = CalTargetposition(curPosition, collision);

        StartCoroutine(PushbackLerp(curPosition, targetPosition));
    }

    public Vector3 CalTargetposition(Vector3 curpos,Collider2D col) //뒤로 밀리는 값계산
    {
        Vector2 pushDirection = (curpos - col.transform.position).normalized;
        return curpos - new Vector3(-pushDirection.x, -pushDirection.y, 0) * 2;
    }

    private IEnumerator PushbackLerp(Vector3 startPos, Vector3 endPos)
    {
        float startTime = Time.time;
        float elapseTime = 0f;

        while(elapseTime < 0.1f)
        {
            if (isTop || isBottom || isLeft || isRight)
            {
                break;
            }
            float t = Mathf.Clamp01(elapseTime / 0.1f); // 시간의 경과 비율
            transform.position = Vector3.Lerp(startPos, endPos, t); // 선형 보간을 사용하여 플레이어 위치 업데이트
            elapseTime = Time.time - startTime;
            yield return null;
        }
    }

    private IEnumerator playerInvincible(float invincible_time)
    {
        if(isInvincible == false)
        {
            yield break;
        }

        float inv_time = invincible_time;
        
        while (inv_time > 0)
        {
            inv_time-= Time.deltaTime;

            yield return null;
        }
        isInvincible = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border") //보더에 부딫힐시 해당 보더에 부딫히고 있다는것 전달
        {
            switch (collision.gameObject.name)
            {
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

        //적이나 적 발사체에 계속 닿고 있다
        if (collision.gameObject.tag == "Enemy")
        {
            playerStat.PlayerDamaged(collision.GetComponent<EnemyObject>().enemyStat.enemyDamage/2, collision.gameObject);
        }
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
