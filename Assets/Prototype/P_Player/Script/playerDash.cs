using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDash : MonoBehaviour
{
    private CharacterController controller;
    private float dashSpeed, dashDuration, dashCooldown;
    private bool isDashing, canDash = true;

    public bool IsDashing => isDashing;
    public bool CanDash => canDash;

    /*public playerDash( CharacterController controller, float dashSpeed, float dashDuration, float dashCooldown)
    {
        Debug.Log("dashSpeed: " + dashSpeed);
        this.controller = controller;
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
    }*/
    public void Init(CharacterController controller, float dashspeed, float dashDuration, float dashCooldown)
    {

        this.controller = controller;
        this.dashSpeed = dashspeed;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
    }

    public void TryDash(Vector3 direction)
    {
        if (!isDashing && canDash )
        {
            StartCoroutine(DashRoutine(direction));
        }
    }

    private IEnumerator DashRoutine(Vector3 direction)
    {
        isDashing = true;
        canDash = false;
        float endTime = Time.time + dashDuration;

        while (Time.time < endTime)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void SetCanDash(bool value)
    {
        Debug.Log("SetCanDash ½ÇÇà : " + value);
        canDash = value;
    }
}
