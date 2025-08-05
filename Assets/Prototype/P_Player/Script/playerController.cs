using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class playerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpButtonGracePeriod;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    [Header("Hurt")]
    [SerializeField] private float hurtDuration; // 피격 시 무적 시간, 멈춤 시간

    
    
    private CharacterController characterController;
    private playerMovement movement;
    private playerDash dash;
    private playerHurtHandler hurtHandler;
    private Player_Combo pct;
    private PlayerState playerStats;
    private bool onRT = false;

    public GameObject Player_Sprite;
    public GameObject Player_State;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //movement = new playerMovement(characterController, speed, jumpSpeed, jumpButtonGracePeriod);
        //dash = new playerDash(this, characterController, dashSpeed, dashDuration, dashCooldown);
        //hurtHandler = new playerHurtHandler(this, hurtDuration);
        movement = GetComponent<playerMovement>();
        dash = GetComponent<playerDash>();
        hurtHandler = GetComponent<playerHurtHandler>();
        playerStats = Player_State.GetComponent<PlayerState>();
        pct = Player_Sprite.GetComponent<Player_Combo>();

        movement.Init(characterController, speed, jumpSpeed, jumpButtonGracePeriod);
        dash.Init(characterController, dashSpeed, dashDuration, dashCooldown);
        hurtHandler.Init(hurtDuration);
    }

    void Update()
    {
        if (hurtHandler.IsHurt) return;

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            movement.HandleJumpInput();
        }

        if (characterController.isGrounded)
        {
            movement.ApplyStepOffset();
        }
        else
        {
            movement.DisableStepOffset();
        }

        if (Input.GetAxis("LT") > 0.1f && dash.CanDash && characterController.isGrounded)
        {
            if (playerStats.UseDash())
            {
                dash.TryDash(input);
            }
        }

        if (!dash.IsDashing)
        {
            Vector3 velocity = movement.UpdateMovement(input);
            characterController.Move(velocity * Time.deltaTime);
        }

        HandleComboInput();
    }
    private void HandleComboInput()
    {
        if (Input.GetAxis("RT") > 0)
        {
            if (Input.GetButtonDown("B"))
            {
                onRT = false;
                pct.ActionThrow();
            }
            else if (Input.GetButtonDown("X"))
            {
                onRT = false;
                pct.ActionChangePosition();
            }
            else
            {
                onRT = true;
                pct.ActionOnGuard();
            }
        }
        else if (Input.GetAxis("RT") == 0 && onRT)
        {
            onRT = false;
            pct.ActionReleaseGuard();
        }
        else
        {
            if (Input.GetButtonDown("B")) pct.ActionAttack();
            if (Input.GetButtonDown("X")) pct.ActionForm();
            if (Input.GetButtonDown("Y")) pct.ActionUltimate();
        }
    }

    public void OnHit(float damage)
    {
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }

        hurtHandler.TakeHit(() => dash.SetCanDash(true));
    }

    public bool IsJumping()
    {
        return movement.IsJumping;
    }

    public bool IsHurt()
    {
        return hurtHandler.IsHurt;
    }

    public bool IsDashing()
    {
        return dash != null && dash.IsDashing;
    }

}
