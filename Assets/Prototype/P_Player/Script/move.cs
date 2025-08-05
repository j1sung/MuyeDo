using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float hurtDuration; // 피격 시 무적 시간, 멈춤 시간

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpButtonGracePeriod;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    private CharacterController characterController;
    private float ySpeed; // 현재 적용받는 y 속도값
    private float originalStepOffset; // CharacterController의 StepOffset 설정값
    private float? lastGroundedTime; //마지막 땅 접지 시간, float? (Nullable<float>를 사용하여 null 할당)
    private float? jumpButtonPressedTime; //점프 버튼 누른 시간
    private bool isDashing = false;
    private bool canDash = true;

    private PlayerState playerStats;
    private bool isHurt = false;
    private Player_Combo pct;
    private bool onRT = false;

    public bool IsJumping => ySpeed > 0.1f && !characterController.isGrounded;
    public bool IsHurt => isHurt;

    

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerState>();
        pct = GetComponent<Player_Combo>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        if (isHurt) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed; //정규화 전 대각선 문제 해결을 위한 크기 제한
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime; //캐릭터 점프 시 중력에 의한 속도 감소

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonPressedTime)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.8f;
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;

                lastGroundedTime = null;
                jumpButtonPressedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        // 약진 (순간 회피)
        if (Input.GetAxis("LT") > 0.1f && characterController.isGrounded && !isDashing && canDash)
        {
            if (playerStats != null && playerStats.UseDash()) //약진 게이지 1칸 차감 시도
            {
                StartCoroutine(Dash(movementDirection));
            }
        }

        if (!isDashing)
        {
            Vector3 velocity = movementDirection * magnitude;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);
        }

        //가드
        if (Input.GetAxis("RT") > 0)
        {
            //잡기(던지기)
            if (Input.GetButtonDown("B"))
            {
                onRT = false;
                pct.ActionThrow();
            }
            //잡기(위치바꾸기)
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
            //공격
            if (Input.GetButtonDown("B"))
            {
                pct.ActionAttack();
            }

            //세
            if (Input.GetButtonDown("X"))
            {
                pct.ActionForm();
            }

            //궁극기
            if (Input.GetButtonDown("Y"))
            {
                pct.ActionUltimate();
            }

        }
    }
    private IEnumerator Dash(Vector3 dashDirection)
    {
        isDashing = true;
        canDash = false;

        float dashEndTime = Time.time + dashDuration;

        while (Time.time < dashEndTime)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown); // 약진 후 쿨타임 적용
        canDash = true;
    }
    public void TakeHit()
    {
        if (!isHurt)
        {
            StartCoroutine(HurtRoutine());
        }
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;
        canDash = false;
        
        yield return new WaitForSeconds(hurtDuration); // 피격 시 잠깐 멈추고 넉백 or 무력화

        isHurt = false;
        canDash = true;
    }

    public void OnHit(float damage)
    {
        Debug.Log("플레이어 OnHit 호출됨! 피해량: " + damage);
        PlayerState stats = GetComponent<PlayerState>();

        if (stats != null)
        {
            stats.TakeDamage(damage);
        }

        TakeHit();
    }

}
