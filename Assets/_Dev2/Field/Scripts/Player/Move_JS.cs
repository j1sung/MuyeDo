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
    private float ySpeed; // ���� ����޴� y �ӵ���
    private float originalStepOffset; // CharacterController�� StepOffset ������
    private float? lastGroundedTime; //������ �� ���� �ð�, float? (Nullable<float>�� ����Ͽ� null �Ҵ�)
    private float? jumpButtonPressedTime; //���� ��ư ���� �ð�
    private bool isDashing = false;
    private bool canDash = true;

    private PlayerState playerStats;

    private void OnEnable()
    {
        // ������Ʈ Ȱ��ȭ �� ���� �ʱ�ȭ
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
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed; //����ȭ �� �밢�� ���� �ذ��� ���� ũ�� ����
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime; //ĳ���� ���� �� �߷¿� ���� �ӵ� ����

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

        // ���� (���� ȸ��)
        if (Input.GetAxis("LT") > 0.1f && characterController.isGrounded && !isDashing && canDash) // playerStats.currentDash != 0 (�̰Ÿ� getter�� �о���� �ɵ�)
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

        yield return new WaitForSeconds(dashCooldown); // ���� �� ��Ÿ�� ����
        canDash = true;
    }
}
