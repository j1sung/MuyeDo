using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    public GameObject player;
    public float attackRange;    
    public float attackDamage;
    public float attackCooldown;
    private bool canAttack = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange && canAttack)
        {
            animator.SetBool("IsJump", true);
            Attack();
        }
    }
    
    public void Attack()
    {
        Debug.Log("Enemy 공격 시도"); // 추가
        
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            canAttack = false;
            playerController move = player.GetComponent<playerController>();
            if (move != null)
            {
                move.OnHit(attackDamage); // Move에서 체력 처리 및 피격 트리거
            }
            StartCoroutine(AttackCooldown());
        }
    }
    private System.Collections.IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
