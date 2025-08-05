using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Move_JS : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    private CharacterController characterController;
    private float ySpeed; // 현재 적용받는 y 속도값
    private float originalStepOffset; // CharacterController의 StepOffset 설정값
    private float? lastGroundedTime; //마지막 땅 접지 시간, float? (Nullable<float>를 사용하여 null 할당)
    private float? jumpButtonPressedTime; //점프 버튼 누른 시간
    private bool isDashing = false;
    private bool canDash = true;

    private PlayerState playerStats;

    private void OnEnable()
    {
        // 오브젝트 활성화 시 상태 초기화
        isDashing = false;
        canDash = true;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        playerStats = GetComponent<PlayerState>();
    }

    void Update()
    {
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
        if (Input.GetAxis("LT") > 0.1f && characterController.isGrounded && !isDashing && canDash) // playerStats.currentDash != 0 (이거만 getter로 읽어오면 될듯)
        {
            playerStats.UseDash();
            StartCoroutine(Dash(movementDirection));
        }

        if (!isDashing)
        {
            Vector3 velocity = movementDirection * magnitude;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);
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
}
