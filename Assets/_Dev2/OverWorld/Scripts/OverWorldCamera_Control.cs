using UnityEngine;
using Cinemachine;

public class OverWorldCamera_Control : MonoBehaviour
{
    public Transform player;
    public CinemachineVirtualCamera virtualCamera; 
    public float cameraMoveSpeed = 10f; 
    public float zoomSpeed = 5f; 
    public float minZoom = 30f; 
    public float maxZoom = 50f; 

    private bool isCameraMode = false; // 카메라 조작 모드 여부
    private Vector3 savedCameraPosition; // 원래 카메라 위치 저장
    private Quaternion savedCameraRotation; // 원래 카메라 회전 저장
    private MonoBehaviour playerController; 
    private Transform mainCamera; // 메인 카메라 (Cinemachine 대신 직접 이동할 카메라)

    void Start()
    {
        playerController = player.GetComponent<MonoBehaviour>(); // 캐릭터 컨트롤러 가져오기
        mainCamera = Camera.main.transform; // 실제 메인 카메라 가져오기
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleCameraMode();
        }

        if (isCameraMode)
        {
            HandleCameraMovement();
        }
    }

    void ToggleCameraMode()
    {
        if (isCameraMode)
        {
            // ▶ 캐릭터 조작 모드로 복귀
            isCameraMode = false;

            // Cinemachine Virtual Camera 다시 활성화
            virtualCamera.enabled = true;

            // 저장된 위치로 카메라 복귀
            mainCamera.position = savedCameraPosition;
            mainCamera.rotation = savedCameraRotation;

            // 캐릭터 조작 다시 활성화
            if (playerController != null)
                playerController.enabled = true;
        }
        else
        {
            // ▶ 카메라 조작 모드로 전환
            isCameraMode = true;

            // 현재 카메라 위치 저장
            savedCameraPosition = mainCamera.position;
            savedCameraRotation = mainCamera.rotation;

            // Cinemachine Virtual Camera 비활성화 (직접 조작하기 위해)
            virtualCamera.enabled = false;

            // 캐릭터 조작 비활성화
            if (playerController != null)
                playerController.enabled = false;
        }
    }

void HandleCameraMovement()
{
    float moveX = Input.GetAxis("Horizontal"); 
    float moveZ = Input.GetAxis("Vertical");  

    Vector3 moveDirection = new Vector3(moveX, 0, moveZ) * cameraMoveSpeed * Time.deltaTime;
    mainCamera.position += moveDirection;

    // 마우스 휠로 FOV 줌 인/아웃
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0)
    {
        float newFOV = Camera.main.fieldOfView - scroll * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
    }
}
}