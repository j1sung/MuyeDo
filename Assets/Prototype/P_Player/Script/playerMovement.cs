using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private CharacterController controller;
    private float speed, jumpSpeed, ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime, jumpButtonPressedTime;
    private float jumpGracePeriod;

    public bool IsJumping => ySpeed > 0.1f && !controller.isGrounded;

    /*public playerMovement(CharacterController controller, float speed, float jumpSpeed, float gracePeriod)
    {
        Debug.Log("JumpSpeed: " + jumpSpeed);  // 여기!

        this.controller = controller;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.jumpGracePeriod = gracePeriod;
        this.originalStepOffset = controller.stepOffset;
    }*/
    public void Init(CharacterController controller, float speed, float jumpSpeed, float gracePeriod)
    {
        Debug.Log("JumpSpeed: " + jumpSpeed);

        this.controller = controller;
        this.speed = speed;
        this.jumpSpeed = jumpSpeed;
        this.jumpGracePeriod = gracePeriod;
        this.originalStepOffset = controller.stepOffset;
    }

    public void HandleJumpInput()
    {
        jumpButtonPressedTime = Time.time;
    }

    public Vector3 UpdateMovement(Vector3 input)
    {
        input.Normalize();
        float magnitude = Mathf.Clamp01(input.magnitude) * speed;

        // 중력 적용 - 유니티 기본 중력 사용
        ySpeed += Physics.gravity.y * Time.deltaTime;

        // 점프 조건 체크
        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;

            if (jumpButtonPressedTime.HasValue &&
                Time.time - jumpButtonPressedTime <= jumpGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
            else if (ySpeed < 0)
            {
                ySpeed = -0.8f; // 바닥에 붙도록 약간 눌러줌
            }
        }

        Vector3 velocity = input * magnitude;
        velocity.y = ySpeed;

        return velocity;
    }

    public void ApplyStepOffset()
    {
        controller.stepOffset = originalStepOffset;
    }

    public void DisableStepOffset()
    {
        controller.stepOffset = 0;
    }
}
