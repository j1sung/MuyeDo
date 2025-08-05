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
        Debug.Log("Enemy ���� �õ�"); // �߰�
        
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            canAttack = false;
            playerController move = player.GetComponent<playerController>();
            if (move != null)
            {
                move.OnHit(attackDamage); // Move���� ü�� ó�� �� �ǰ� Ʈ����
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
