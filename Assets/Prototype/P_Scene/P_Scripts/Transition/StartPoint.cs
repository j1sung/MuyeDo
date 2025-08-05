using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    [SerializeField]
    private string previousSceneName; // 이동한 맵의 이름과 일치하는지 확인

    void Start()
    {

        if (SceneManager.GetActiveScene().name != "P_Overworld") // 오버월드 아니면 활성화
        {
            // 필드에선 필드 카메라, 캐릭터 활성화
            Player_instance_JS.instance.gameObject.SetActive(true);
            CameraMove_JS.instance.gameObject.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "P_Overworld") // 오버월드면 비활성화
        {
            // 필드에선 필드 카메라, 캐릭터 활성화
            Player_instance_JS.instance.gameObject.SetActive(false);
            CameraMove_JS.instance.gameObject.SetActive(false);
        }

        if (previousSceneName == Player_instance_JS.instance.saveSceneName)
        {   
            // 캐릭터컨트롤러가 활성화 되면 위치이동이 안되서 비활성화함
            CharacterController cc = Player_instance_JS.instance.GetComponent<CharacterController>(); 
            cc.enabled = false; // 임시로 비활성화
            Player_instance_JS.instance.transform.position = transform.position; // 캐릭터 이동
            CameraMove_JS.instance.SnapToTarget();                             // 카메라 이동
            cc.enabled = true;  // 다시 활성화
        }
    }
}
