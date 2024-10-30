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

    public void Init()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerInput = new PlayerInput();
    }

    public void PlayerInputOn()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnPlayerMove;
        playerInput.Player.Skill.performed += OnPlayerSkill;
    }

    public void playerInputOff()
    {
        playerInput.Player.Move.performed -= OnPlayerMove;
        playerInput.Player.Skill.performed -= OnPlayerSkill;
        playerInput.Player.Disable();
    }

    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    private void OnPlayerSkill(InputAction.CallbackContext context)
    {
        PlayerSkillOn();
    }


    public void PlayerMove()
    {
        float h = inputVector.x;
        float v = inputVector.y;

        // 충돌에 따른 이동 제한 처리
        if ((isRightCollide && h > 0) || (isLeftCollide && h < 0)) h = 0;
        if ((isTopCollide && v > 0) || (isBottomCollide && v < 0)) v = 0;

        Vector3 moveDirection = new Vector3(h, v, 0) * (playerMoveSpeedBase + (pStat.IG_MSpd / 5)) * Time.deltaTime;
        transform.position += moveDirection;
    }

    public void KeepPlayerInViewport()
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
        if(pStat.IG_curPowerLevel <= 0)
        {
            Debug.Log("파워레벨이 충분하지 않음");
        }
        else if(pStat.USkillCount <= 0)
        {
            Debug.Log("사용 가능 횟수가 부족");
        }
        //else if(현재 스킬 사용중일 경우)

        

        //PlayerMain.pSpecial.isSkillActivating = true;
        PlayerMain.pSpecial.SpecialFire(pStat.curPlayerID, pStat.IG_curPowerLevel);

        pStat.USkillCount--;
        pStat.IG_curPowerLevel = 0;
        pStat.AddPower = 0;
    }

    
}
