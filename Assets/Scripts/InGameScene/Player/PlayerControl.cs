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

    public bool isAbleToMove; //움직이기가 가능한상태
    public bool isInvincibleState; //무적상태
    public bool isShootable; //발사가 가능한 상태
    public bool isHitted; //데미지를 받은 상태

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
        KeepPlayerInViewport();
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

    private void KeepPlayerInViewport()
    {
        // 플레이어의 현재 월드 좌표를 가져옵니다.
        Vector3 playerPos = transform.position;

        // 현재 플레이어의 월드 좌표를 뷰포트 좌표로 변환합니다.
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(playerPos);

        // UI가 차지하는 뷰포트 영역을 고려하여 제한을 설정합니다.
        float uiPaddingX = 0.0f; // UI에 가로 패딩이 없으면 0
        float uiPaddingY = 0.0f; // 세로 패딩, 이 예제에서는 사용하지 않음
        float headerViewportHeight = 50f / Screen.height; // 헤더 UI의 뷰포트 높이
        float footerViewportHeight = 75f / Screen.height; // 푸터 UI의 뷰포트 높이

        // 뷰포트 좌표를 제한
        viewportPos.x = Mathf.Clamp(viewportPos.x, uiPaddingX, 1 - uiPaddingX);
        viewportPos.y = Mathf.Clamp(viewportPos.y, footerViewportHeight, 1 - headerViewportHeight);

        // 제한된 뷰포트 좌표를 다시 월드 좌표로 변환합니다.
        playerPos = Camera.main.ViewportToWorldPoint(viewportPos);

        // 변환된 월드 좌표를 플레이어의 위치로 설정합니다.
        transform.position = playerPos;
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

    private Vector2 CalculateTargetPos(Vector3 curpos,Collider2D col) //뒤로 밀리는 값계산
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
        //적이나 적 발사체에 계속 닿고 있다
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
