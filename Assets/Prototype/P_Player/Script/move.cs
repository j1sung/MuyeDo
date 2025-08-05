using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float hurtDuration; // �ǰ� �� ���� �ð�, ���� �ð�

    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpButtonGracePeriod;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    private CharacterController characterController;
    private float ySpeed; // ���� ����޴� y �ӵ���
    private float originalStepOffset; // CharacterController�� StepOffset ������
    private float? lastGroundedTime; //������ �� ���� �ð�, float? (Nullable<float>�� ����Ͽ� null �Ҵ�)
    private float? jumpButtonPressedTime; //���� ��ư ���� �ð�
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
        if (Input.GetAxis("LT") > 0.1f && characterController.isGrounded && !isDashing && canDash)
        {
            if (playerStats != null && playerStats.UseDash()) //���� ������ 1ĭ ���� �õ�
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

        //����
        if (Input.GetAxis("RT") > 0)
        {
            //���(������)
            if (Input.GetButtonDown("B"))
            {
                onRT = false;
                pct.ActionThrow();
            }
            //���(��ġ�ٲٱ�)
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
            //����
            if (Input.GetButtonDown("B"))
            {
                pct.ActionAttack();
            }

            //��
            if (Input.GetButtonDown("X"))
            {
                pct.ActionForm();
            }

            //�ñر�
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

        yield return new WaitForSeconds(dashCooldown); // ���� �� ��Ÿ�� ����
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
        
        yield return new WaitForSeconds(hurtDuration); // �ǰ� �� ��� ���߰� �˹� or ����ȭ

        isHurt = false;
        canDash = true;
    }

    public void OnHit(float damage)
    {
        Debug.Log("�÷��̾� OnHit ȣ���! ���ط�: " + damage);
        PlayerState stats = GetComponent<PlayerState>();

        if (stats != null)
        {
            stats.TakeDamage(damage);
        }

        TakeHit();
    }

}
