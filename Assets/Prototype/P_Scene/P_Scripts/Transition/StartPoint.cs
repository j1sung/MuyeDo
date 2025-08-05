using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    [SerializeField]
    private string previousSceneName; // �̵��� ���� �̸��� ��ġ�ϴ��� Ȯ��

    void Start()
    {

        if (SceneManager.GetActiveScene().name != "P_Overworld") // �������� �ƴϸ� Ȱ��ȭ
        {
            // �ʵ忡�� �ʵ� ī�޶�, ĳ���� Ȱ��ȭ
            Player_instance_JS.instance.gameObject.SetActive(true);
            CameraMove_JS.instance.gameObject.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "P_Overworld") // ��������� ��Ȱ��ȭ
        {
            // �ʵ忡�� �ʵ� ī�޶�, ĳ���� Ȱ��ȭ
            Player_instance_JS.instance.gameObject.SetActive(false);
            CameraMove_JS.instance.gameObject.SetActive(false);
        }

        if (previousSceneName == Player_instance_JS.instance.saveSceneName)
        {   
            // ĳ������Ʈ�ѷ��� Ȱ��ȭ �Ǹ� ��ġ�̵��� �ȵǼ� ��Ȱ��ȭ��
            CharacterController cc = Player_instance_JS.instance.GetComponent<CharacterController>(); 
            cc.enabled = false; // �ӽ÷� ��Ȱ��ȭ
            Player_instance_JS.instance.transform.position = transform.position; // ĳ���� �̵�
            CameraMove_JS.instance.SnapToTarget();                             // ī�޶� �̵�
            cc.enabled = true;  // �ٽ� Ȱ��ȭ
        }
    }
}
