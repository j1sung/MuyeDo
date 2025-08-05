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
        //moveScript = GetComponent<Move>(); // Move ��ũ��Ʈ ����
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
        

        // �ǰ� Ʈ���Ŵ� isHurt ���°� ó�� true�� �ٲ���� ���� ����
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
