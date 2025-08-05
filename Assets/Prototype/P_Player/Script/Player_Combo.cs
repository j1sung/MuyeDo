using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Combo : MonoBehaviour
{
    volatile bool atkInputEnabled = false;
    volatile bool atkInputNow = false;
    private Animator animator;

    //Animation State
    public readonly static int ANISTS_Idle = Animator.StringToHash("Base Layer.IDLE");
    public readonly static int ANISTS_Run = Animator.StringToHash("Base Layer.Run");
    public readonly static int ANISTS_Form = Animator.StringToHash("Base Layer.Combo_System.Form");
    public readonly static int ANISTS_Attack = Animator.StringToHash("Base Layer.Combo_System.Attack");
    public readonly static int ANISTS_Power_Attack = Animator.StringToHash("Base Layer.Combo_System.Power_Attack");
    public readonly static int ANISTS_OnGuard = Animator.StringToHash("Base Layer.Combo_System.OnGuard");
    public readonly static int ANISTS_Guarding = Animator.StringToHash("Base Layer.Combo_System.Guarding");

    //콤보 공격 카운트
    [SerializeField] private int Attack_cnt;
    [SerializeField] private int Power_Attack_cnt;
    //선택한 세
    [SerializeField] private int Form_num;

    //현재 동작 중인 콤보 여부
    [SerializeField] private bool onAttackCombo;
    [SerializeField] private bool onPower_AttackCombo;
    //세 여부
    [SerializeField] private bool Formed;
    //방어 여부
    [SerializeField] private bool Guarded;

    //ActionEvent
    public event Action ActionEvent = null;
    public void Start()
    {
        animator = GetComponent<Animator>();
        Attack_cnt = 0;
        Power_Attack_cnt = 0;
        onAttackCombo = false;
        onPower_AttackCombo = false;
        Formed = false;
        Guarded = false;

        Form_num = 0;
    }

    //애니메이션용 이벤트 코드
    public void EnableAttackInput() {
        atkInputEnabled = true;
    }
    public void NextAction() {
        if (ActionEvent != null)
        {
            Debug.Log("Play Combo Animation");
            ActionEvent.Invoke();
            ActionEvent = null;
        }
        else
        {
            Debug.Log("reset state");
            EndAttackCombo();
        }
    }

    //세 전용 애니메이션 이벤트
    public void DisableForm()
    {
        Formed = false;
        Debug.Log("off form!");
    }

    public void PlayAnimation(string parameter, int actNum) 
    {
        animator.SetFloat(parameter, actNum);
    }

    //======기본 액션

    //공격
    public void ActionAttack() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == ANISTS_Idle   ||
            stateInfo.fullPathHash == ANISTS_Run    ||
            stateInfo.fullPathHash == ANISTS_Form)
        {
            if (ActionEvent == null)
            {
                if (Formed == true)
                {
                    ActionEvent += playPower_Attack;
                }
                else
                {
                    playAttack();
                }
            }
        }
        else
        {
            if (atkInputEnabled) 
            {
                atkInputEnabled = false;
                if(ActionEvent == null)
                {
                    if(onAttackCombo)
                    {
                        if (Attack_cnt < 4)
                        {
                            ActionEvent += playAttack;
                        }
                        else
                        {
                            ActionEvent += EndAttackCombo;
                        }
                    }

                    if (onPower_AttackCombo) 
                    {
                        if(Power_Attack_cnt < 3)
                        {
                            ActionEvent += playPower_Attack;
                        }
                        else
                        {
                            ActionEvent += EndAttackCombo;
                        }
                    }
                }    
            }
        }
    }

    //일반 공격
    private void playAttack()
    {
        animator.SetTrigger("Attack");
        onAttackCombo = true;
        if (Formed && Attack_cnt == 3)
        {
            Formed = false;
            Attack_cnt = 4;
        }
        PlayAnimation("Attack_Blend", Attack_cnt);
        Attack_cnt++;
    }

    //강 공격
    private void playPower_Attack()
    {
        if(Formed)
        {
            Debug.Log(Power_Attack_cnt);
            animator.SetTrigger("Power_Attack");
            onPower_AttackCombo = true;
            PlayAnimation("Power_Attack_Blend", Power_Attack_cnt);
            Power_Attack_cnt++;
        }
    }

    private void EndAttackCombo()
    {
        Attack_cnt = 0;
        Power_Attack_cnt = 0;
        onAttackCombo = false;
        onPower_AttackCombo = false;
        Formed = false;

    }

    //세
    public void ActionForm() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == ANISTS_Idle   ||
            stateInfo.fullPathHash == ANISTS_Run    ||
            stateInfo.fullPathHash == ANISTS_Attack ||
            stateInfo.fullPathHash == ANISTS_Power_Attack)
        {
            Formed = true;

            if (onAttackCombo)
            {
                if (Attack_cnt <= 2)
                {
                    playForm();
                    EndAttackCombo();
                }
            }

            if(onPower_AttackCombo)
            {
                if(Power_Attack_cnt <= 1)
                {
                    playForm();
                    EndAttackCombo();
                }
            }

            if(!onAttackCombo && !onPower_AttackCombo)
            {
                playForm();
            }
        }
    }

    private void playForm()
    {
        animator.SetTrigger("Form");
        PlayAnimation("Form_Blend", Form_num);
    }

    //궁극기
    public void ActionUltimate() 
    {
        if (Formed) 
        {
            if (ActionEvent == null)
            {
                ActionEvent += playUltimate;
            }
        }
    }

    private void playUltimate()
    {
        animator.SetTrigger("Ultimate");
    }
    //방어 하기
    public void ActionOnGuard() 
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Run)
        {
            playOnGuard();
        }
    }

    private void playOnGuard()
    {
        Guarded = true;
        animator.SetBool("Guard", Guarded);
    }

    //방어 풀기
    public void ActionReleaseGuard() 
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(stateInfo.fullPathHash);
        Debug.Log(ANISTS_OnGuard);
        Debug.Log(ANISTS_Guarding);
        if ((stateInfo.fullPathHash == ANISTS_OnGuard ||
            stateInfo.fullPathHash == ANISTS_Guarding )&& Guarded)
        {
            Debug.Log("Release Guard");
            playReleaseGuard();
        }
    }

    private void playReleaseGuard()
    {
        Guarded = false;
        animator.SetBool("Guard", Guarded);
    }

    //잡기(던지기)
    public void ActionThrow() 
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo.fullPathHash == ANISTS_OnGuard    ||
            stateInfo.fullPathHash == ANISTS_Guarding)   && Guarded)
        {
            playThrow();
        }
    }

    private void playThrow()
    {
        playReleaseGuard();
        animator.SetTrigger("Throw");
    }

    //잡기(위치바꾸기)
    public void ActionChangePosition() 
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo.fullPathHash == ANISTS_OnGuard ||
            stateInfo.fullPathHash == ANISTS_Guarding )&& Guarded)
        {
            playChangePosition();
        }
    }

    private void playChangePosition()
    {
        playReleaseGuard();
        animator.SetTrigger("ChangePosition");
    }
}
