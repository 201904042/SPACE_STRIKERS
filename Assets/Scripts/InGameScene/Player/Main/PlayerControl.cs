using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private const float playerMoveSpeedBase = 3f;
    private PlayerStat pStat => PlayerMain.pStat;
    private SpriteRenderer playerSprite;

    private PlayerInput playerInput;
    private Vector2 inputVector;

    public bool isTopCollide;
    public bool isBottomCollide;
    public bool isLeftCollide;
    public bool isRightCollide;

    public bool canMove
    {
        get => pStat.CanMove;
        set => pStat.CanMove = value;
    }

    private float finalSpeed;

    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
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
        PlayerSkillOn();
    }

    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    private void Init()
    {
        finalSpeed = playerMoveSpeedBase + (pStat.moveSpeed / 5);
    }

    private void Update()
    {
        finalSpeed = playerMoveSpeedBase + (pStat.moveSpeed / 5);

        if (canMove)
        {
            PlayerMove();
        }

        KeepPlayerInViewport();
    }

    private void PlayerMove()
    {
        float h = inputVector.x;
        float v = inputVector.y;

        // 충돌에 따른 이동 제한 처리
        if ((isRightCollide && h > 0) || (isLeftCollide && h < 0)) h = 0;
        if ((isTopCollide && v > 0) || (isBottomCollide && v < 0)) v = 0;

        Vector3 moveDirection = new Vector3(h, v, 0) * finalSpeed * Time.deltaTime;
        transform.position += moveDirection;
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

    public void PlayerSkillOn()
    {
        if (pStat.specialCount > 0 && pStat.powerLevel != 0) //todo => 스페셜이 작동중이면 불가
        {
            pStat.specialCount--;
            //SpecialFire(playerId);
        }
        else
        {
            Debug.Log("cant do specialattack");
        }
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
