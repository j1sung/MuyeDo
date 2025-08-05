using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player_Anime : MonoBehaviour
{

    private Animator animator;
    private Transform transform;
    private bool onRT = false;
    private Player_Combo pct;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        pct = GetComponent<Player_Combo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            animator.SetBool("IsRunning", true);
            transform.localScale = new Vector3(1, 1, 1);

        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            animator.SetBool("IsRunning", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetButtonDown("A"))
        {
            animator.SetTrigger("IsJump");
        }

        //����
        if (Input.GetAxis("RT") > 0)
        {
            //���(������)
            if (Input.GetButtonDown("B"))
            {
                Debug.Log("RT+B");
                onRT = false;
                pct.ActionThrow();
            }
            //���(��ġ�ٲٱ�)
            else if (Input.GetButtonDown("X"))
            {
                Debug.Log("RT+X");
                onRT = false;
                pct.ActionChangePosition();
            }
            else
            {
                onRT = true;
                Debug.Log("push RT");
                pct.ActionOnGuard();
            }
        }
        else if (Input.GetAxis("RT") == 0 && onRT)
        {
            Debug.Log("Release RT");
            onRT = false;
            pct.ActionReleaseGuard();
        }
        else
        {
            //����
            if (Input.GetButtonDown("B"))
            {
                Debug.Log("B");
                pct.ActionAttack();
            }

            //��
            if (Input.GetButtonDown("X"))
            {
                Debug.Log("X");
                pct.ActionForm();
            }

            //�ñر�
            if (Input.GetButtonDown("Y"))
            {
                Debug.Log("Y");
                pct.ActionUltimate();
            }
        }
    }
}
