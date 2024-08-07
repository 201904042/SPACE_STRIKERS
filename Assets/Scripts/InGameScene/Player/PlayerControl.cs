using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private PlayerStat playerStat;
    private SpriteRenderer playerSprite;

    private float speed;

    private bool isTopCollide;
    private bool isBottomCollide;
    private bool isLeftCollide;
    private bool isRightCollide;

    private float basicSpeed = 3f;

    public bool canMove;
    public bool isInvincible;

    private float invincibleDuration = 3f;

    // Update is called once per frame
    private void Awake()
    {
        playerStat = transform.GetComponent<PlayerStat>();
        playerSprite = transform.GetComponent<SpriteRenderer>();

        speed = basicSpeed + (playerStat.moveSpeed/5);

        isInvincible = false;
        canMove = true;
    }

    private void Update()
    {
        speed = basicSpeed + (playerStat.moveSpeed / 5);

        if (canMove)
        {
            PlayerMove();
        }

        InvincibleSprite();
    }

    //컨트롤 뉴인풋시스템 적용 예정
    private void PlayerMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isRightCollide && h == 1) || (isLeftCollide && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTopCollide && v == 1) || (isBottomCollide && v == -1))
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

    public void PlayerDamagedAction(GameObject attack_obj)
    {
        if (isInvincible == true) { return; }
        
        Collider2D attackingCollider = attack_obj.GetComponent<Collider2D>();
        if (attackingCollider != null)
        {
            isInvincible = true;
            StartCoroutine(invinciblePlayer(invincibleDuration));
            PlayerKnockBack(attackingCollider);
        }
    }
    
    public void PlayerKnockBack(Collider2D collision)
    {
        Vector2 curPosition = transform.position;
        Vector2 targetPosition = CalculateTargetPos(curPosition, collision);

        StartCoroutine(PushbackLerp(curPosition, targetPosition));
    }

    public Vector2 CalculateTargetPos(Vector3 curpos,Collider2D col) //뒤로 밀리는 값계산
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
            if (isTopCollide || isBottomCollide || isLeftCollide || isRightCollide)
            {
                break;
            }
            float t = Mathf.Clamp01(elapseTime / 0.1f); // 시간의 경과 비율
            transform.position = Vector3.Lerp(startPos, endPos, t); // 선형 보간을 사용하여 플레이어 위치 업데이트
            elapseTime = Time.time - startTime;
            yield return null;
        }
    }

    /// <summary>
    /// 플레이어의 무적시간 코루틴
    /// </summary>
    private IEnumerator invinciblePlayer(float invincible_time)
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincible_time);
        
        isInvincible = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border") //보더에 부딫힐시 해당 보더에 부딫히고 있다는것 전달
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTopCollide = true; break;
                case "Bottom":
                    isBottomCollide = true; break;
                case "Right":
                    isRightCollide = true; break;
                case "Left":
                    isLeftCollide = true; break;
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
                    isTopCollide = false; break;
                case "Bottom":
                    isBottomCollide = false; break;
                case "Right":
                    isRightCollide = false; break;
                case "Left":
                    isLeftCollide = false; break;
            }
        }
    }
}
