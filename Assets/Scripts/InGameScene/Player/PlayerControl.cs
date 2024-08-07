using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private PlayerStat playerStat;
    private PlayerInput playerInput;
    private SpriteRenderer playerSprite;

    private Vector2 moveInput;
    private float playerObjSpeed;

    private bool isTopCollide;
    private bool isBottomCollide;
    private bool isLeftCollide;
    private bool isRightCollide;

    private float basicSpeed = 3f;

    public bool isAbleToMove; //�����̱Ⱑ �����ѻ���
    public bool isInvincibleState; //��������
    public bool isShootable; //�߻簡 ������ ����
    public bool isHitted; //�������� ���� ����

    private float invincibleDuration = 3f;

    private void Awake()
    {
        playerStat = transform.GetComponent<PlayerStat>();
        playerSprite = transform.GetComponent<SpriteRenderer>();

        playerObjSpeed = basicSpeed + (playerStat.moveSpeed/5);

        isShootable = true;
        isHitted = false;
        isInvincibleState = false;
        isAbleToMove = true;
    }

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnPlayerMove;
        playerInput.Player.Skill.performed += OnPlayerSkill;
    }

    private void OnDisable()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Move.performed -= OnPlayerMove;
        playerInput.Player.Skill.performed -= OnPlayerSkill;
        playerInput.Player.Disable();
    }

    private void OnPlayerSkill(InputAction.CallbackContext context)
    {
        GetComponent<PlayerSpecialSkill>().PlayerSkillOn();
    }

    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    private void Update()
    {
        playerObjSpeed = basicSpeed + (playerStat.moveSpeed / 5);

        if (isAbleToMove)
        {
            PlayerMove();
        }

        MakePlayerTranslucent();
    }

    private void PlayerMove()
    {
        float h = moveInput.x;
        if ((isRightCollide && h == 1) || (isLeftCollide && h == -1)) h = 0;

        float v = moveInput.y;
        if ((isTopCollide && v == 1) || (isBottomCollide && v == -1)) v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * playerObjSpeed * Time.deltaTime;

        transform.position = curPos + nextPos;
    }

    private void MakePlayerTranslucent()
    {
        if (isInvincibleState)
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
        if (isInvincibleState == true) { return; }
        
        Collider2D attackingCollider = attack_obj.GetComponent<Collider2D>();
        if (attackingCollider != null)
        {
            isInvincibleState = true;
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

    private Vector2 CalculateTargetPos(Vector3 curpos,Collider2D col) //�ڷ� �и��� �����
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
        isInvincibleState = true;

        yield return new WaitForSeconds(invincible_time);
        
        isInvincibleState = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border") 
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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //���̳� �� �߻�ü�� ��� ��� �ִ�
        if (collision.gameObject.tag == "Enemy")
        {
            playerStat.PlayerDamaged(collision.GetComponent<EnemyObject>().enemyStat.enemyDamage / 2, collision.gameObject);
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
