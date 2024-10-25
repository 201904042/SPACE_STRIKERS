using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool isAbleToMove; // �����̱Ⱑ ������ ����
    public bool isInvincibleState; // ���� ����
    public bool isAttackable; // �߻簡 ������ ����
    public bool isHitted; // �������� ���� ����
    private bool isKnockbackRun;
    private float invincibleDuration = 3f;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStat>();
        playerSprite = GetComponent<SpriteRenderer>();

        playerObjSpeed = basicSpeed + (playerStat.moveSpeed / 5);

        isAttackable = true;
        isHitted = false;
        isInvincibleState = false;
        isAbleToMove = true;
        isKnockbackRun = false;
        // PlayerInput �ν��Ͻ��� Awake���� �� ���� ����
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnPlayerMove;
        playerInput.Player.Skill.performed += OnPlayerSkill;
    }

    private void OnDisable()
    {
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
        KeepPlayerInViewport();
    }

    private void PlayerMove()
    {
        float h = moveInput.x;
        float v = moveInput.y;

        // �浹�� ���� �̵� ���� ó��
        if ((isRightCollide && h > 0) || (isLeftCollide && h < 0)) h = 0;
        if ((isTopCollide && v > 0) || (isBottomCollide && v < 0)) v = 0;

        Vector3 moveDirection = new Vector3(h, v, 0) * playerObjSpeed * Time.deltaTime;
        transform.position += moveDirection;
    }

    private void MakePlayerTranslucent()
    {
        playerSprite.color = isInvincibleState
            ? new Color(1, 1, 1, 0.5f)
            : new Color(1, 1, 1, 1f);
    }

    private void KeepPlayerInViewport()
    {
        Vector3 playerPos = transform.position;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(playerPos);

        float uiPaddingX = 0.0f;
        float headerViewportHeight = 50f / Screen.height;
        float footerViewportHeight = 75f / Screen.height;

        viewportPos.x = Mathf.Clamp(viewportPos.x, uiPaddingX, 1 - uiPaddingX);
        viewportPos.y = Mathf.Clamp(viewportPos.y, footerViewportHeight, 1 - headerViewportHeight);

        transform.position = Camera.main.ViewportToWorldPoint(viewportPos);
    }

    public void PlayerDamagedAction(GameObject attack_obj)
    {
        if (isInvincibleState) return;

        Collider2D attackingCollider = attack_obj.GetComponent<Collider2D>();
        if (attackingCollider != null)
        {
            isInvincibleState = true;
            StartCoroutine(ActiveInvincible(invincibleDuration));
            PlayerKnockBack(attackingCollider);
        }
    }

    public void PlayerKnockBack(Collider2D collision)
    {
        Vector2 curPosition = transform.position;
        Vector2 targetPosition = CalculateTargetPos(curPosition, collision);

        // �ڷ�ƾ�� ���� ���� �ƴϸ� ����
        if (!isKnockbackRun)
        {
            StartCoroutine(PushbackLerp(curPosition, targetPosition));
        }
    }


    private Vector2 CalculateTargetPos(Vector3 curpos, Collider2D col)
    {
        Vector2 pushDirection = (curpos - col.transform.position).normalized;
        return curpos - new Vector3(-pushDirection.x, -pushDirection.y, 0) * 2;
    }

    private IEnumerator PushbackLerp(Vector3 startPos, Vector3 endPos)
    {
        isKnockbackRun = true; // �ڷ�ƾ ���� ���� ����
        float startTime = Time.time;
        float elapseTime = 0f;

        while (elapseTime < 0.1f)
        {
            // �浹 ���¸� Ȯ��
            if (isTopCollide || isBottomCollide || isLeftCollide || isRightCollide)
            {
                break;
            }

            float t = Mathf.Clamp01(elapseTime / 0.1f);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapseTime = Time.time - startTime;

            yield return null;
        }

        isKnockbackRun = false; // �ڷ�ƾ ���� ���� ����
    }

    private IEnumerator ActiveInvincible(float invincible_time)
    {
        isInvincibleState = true;
        yield return new WaitForSeconds(invincible_time);
        isInvincibleState = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.name)
            {
                case "Top":
                    isTopCollide = true; break;
                case "Bottom":
                    isBottomCollide = true; break;
                case "Right":
                    isRightCollide = true; break;
                case "Left":
                    isLeftCollide = false; break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerStat.PlayerDamaged(collision.GetComponent<EnemyObject>().enemyStat.damage / 2, collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            switch (collision.name)
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
