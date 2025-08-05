using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuard_JS : MonoBehaviour
{
    private PlayerState playerStats;
    private bool isGuarding;
    Renderer childRenderer;
    private Color originalColor;


    private void OnEnable()
    {
        // ������Ʈ Ȱ��ȭ �� ���� �ʱ�ȭ
        isGuarding = false;
        
    }
    void Start()
    {
        playerStats = GetComponent<PlayerState>(); // player ���� ����
        childRenderer = GetComponentInChildren<Renderer>();
        originalColor = childRenderer.material.color;
    }

    void Update()
    {
        // Input.GetAxis("RT") > 0.1f
        if (Input.GetKey(KeyCode.T))
        {
            isGuarding = true;  // ���� ��
            //Debug.Log("���� ��");
        }
        else
        {
            isGuarding = false; // ���� �ƴ�
            //Debug.Log("���� ����");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (isGuarding)
            {
                StartCoroutine(GuardSuccess()); // ���� ���� Ȯ��
            }
            else
            {
                StartCoroutine(GuardFailed()); // ���� ���� Ȯ��
                playerStats.TakeDamage(10f);
            }
        }
        
    }

    private IEnumerator GuardSuccess() // ���� �����ߴ��� �ð��� üũ - ���� 2�ʵ��� �Ͼ���
    {
        childRenderer.material.color = Color.yellow; // ��� ����
        
        yield return new WaitForSeconds(1f);

        childRenderer.material.color = originalColor; // ���� �� ����
    }

    private IEnumerator GuardFailed() // ���� ���ߴ��� �ð��� üũ - ����/������ �ݺ�
    {
        int repeatCount = 3; // �ݺ� Ƚ��
        float transparentAlpha = 0.5f; // ���� ��
        float effectDuration = 0.1f; // ���� �ð�


        for (int i = 0; i < repeatCount; i++) // 3�� �ݺ�
        {
            Color tempColor = originalColor; // ���� ������ �÷� �ӽ� ����
            tempColor.a = transparentAlpha;
            childRenderer.material.color = tempColor; // ������ ������ ��������

            yield return new WaitForSeconds(effectDuration);

            childRenderer.material.color = originalColor; // ���� ����

            yield return new WaitForSeconds(effectDuration);
        }
    }
}
