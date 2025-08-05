using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionZone_JS : MonoBehaviour
{
    public GameObject interactionUI;   // 필드 UI 창창
    public GameObject pressE;      // 캐릭터 머리 위 "PRESS - E" UI
    private bool isPlayerNearby = false;

    private void Start()
    {
        interactionUI.SetActive(false);
        pressE.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            pressE.SetActive(true); // 캐릭터 머리 위 UI 활성화
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            pressE.SetActive(false); // 캐릭터 머리 위 UI 숨김
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            OpenUI();
        }
    }

    void OpenUI()
    {
        interactionUI.SetActive(true);    // UI 창 열기
        pressE.SetActive(false);      // PRESS E 숨기기
        Time.timeScale = 0f;              // 게임 멈춤
    }

    public void CloseUI()
    {
        interactionUI.SetActive(false);   // UI 창 닫기
        Time.timeScale = 1f;              // 게임 재시작
    }

    public void EnterScene()
    {
        Time.timeScale = 1f;              // 씬 전환 전 시간 복구
        Player_instance_JS.instance.saveSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("P_Field_Village"); // 씬 전환환
    }
}