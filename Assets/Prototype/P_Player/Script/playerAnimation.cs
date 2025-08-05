using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    public GameObject Player_Sprite;
    private Animator animator;
    
    //private Move moveScript;
    private playerController playerController;
    
    private bool wasHurt = false;

    void Start()
    {
        animator = Player_Sprite.GetComponent<Animator>();
        //moveScript = GetComponent<Move>(); // Move 스크립트 참조
        playerController = GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            animator.SetBool("IsRunning", true);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (horizontal > 0 ? 1 : -1);
            transform.localScale = scale;
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        //animator.SetBool("IsJump", moveScript.IsJumping);
        
        animator.SetBool("IsJump", playerController.IsJumping());
        

        // 피격 트리거는 isHurt 상태가 처음 true로 바뀌었을 때만 실행
        /*if (moveScript.IsHurt && !wasHurt)
        {
            animator.SetTrigger("IsHurt");
            wasHurt = true;
        }
        else if (!moveScript.IsHurt)
        {
            wasHurt = false;
        }*/
        if ( playerController.IsHurt())
        {
            if (!wasHurt)
            {
                Debug.Log("Hurt Animation");
                animator.SetTrigger("IsHurt");
                wasHurt = true;
            }
        }
        else
        {
            wasHurt = false;
        }


        //animator.SetBool("IsDashing", playerController.IsDashing());


    }
}
