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

    //��Ʈ�� ����ǲ�ý��� ���� ����
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

    private void InvincibleSprite() //�÷��̾ ���� ��忩�ο� ���� ���� ����
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

    public Vector2 CalculateTargetPos(Vector3 curpos,Collider2D col) //�ڷ� �и��� �����
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
            float t = Mathf.Clamp01(elapseTime / 0.1f); // �ð��� ��� ����
            transform.position = Vector3.Lerp(startPos, endPos, t); // ���� ������ ����Ͽ� �÷��̾� ��ġ ������Ʈ
            elapseTime = Time.time - startTime;
            yield return null;
        }
    }

    /// <summary>
    /// �÷��̾��� �����ð� �ڷ�ƾ
    /// </summary>
    private IEnumerator invinciblePlayer(float invincible_time)
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincible_time);
        
        isInvincible = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border") //������ �΋H���� �ش� ������ �΋H���� �ִٴ°� ����
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

        //���̳� �� �߻�ü�� ��� ��� �ִ�
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
