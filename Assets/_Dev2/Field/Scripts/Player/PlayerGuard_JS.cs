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
        // 오브젝트 활성화 시 상태 초기화
        isGuarding = false;
        
    }
    void Start()
    {
        playerStats = GetComponent<PlayerState>(); // player 스탯 연결
        childRenderer = GetComponentInChildren<Renderer>();
        originalColor = childRenderer.material.color;
    }

    void Update()
    {
        // Input.GetAxis("RT") > 0.1f
        if (Input.GetKey(KeyCode.T))
        {
            isGuarding = true;  // 가드 중
            //Debug.Log("가드 중");
        }
        else
        {
            isGuarding = false; // 가드 아님
            //Debug.Log("가드 해제");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (isGuarding)
            {
                StartCoroutine(GuardSuccess()); // 가드 성공 확인
            }
            else
            {
                StartCoroutine(GuardFailed()); // 가드 실패 확인
                playerStats.TakeDamage(10f);
            }
        }
        
    }

    private IEnumerator GuardSuccess() // 가드 성공했는지 시각적 체크 - 몸이 2초동안 하얘짐
    {
        childRenderer.material.color = Color.yellow; // 흰색 지정
        
        yield return new WaitForSeconds(1f);

        childRenderer.material.color = originalColor; // 원래 색 지정
    }

    private IEnumerator GuardFailed() // 공격 당했는지 시각적 체크 - 투명/불투명 반복
    {
        int repeatCount = 3; // 반복 횟수
        float transparentAlpha = 0.5f; // 투명도 값
        float effectDuration = 0.1f; // 지속 시간


        for (int i = 0; i < repeatCount; i++) // 3번 반복
        {
            Color tempColor = originalColor; // 투명 적용할 컬러 임시 저장
            tempColor.a = transparentAlpha;
            childRenderer.material.color = tempColor; // 지정한 값으로 투명해짐

            yield return new WaitForSeconds(effectDuration);

            childRenderer.material.color = originalColor; // 원래 투명도

            yield return new WaitForSeconds(effectDuration);
        }
    }
}
