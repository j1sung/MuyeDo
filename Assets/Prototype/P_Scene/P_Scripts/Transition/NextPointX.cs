using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextPointX : MonoBehaviour
{
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = Player_instance_JS.instance.GetComponent<CharacterController>();
            cc.enabled = false; // �ӽ÷� ��Ȱ��ȭ
            Vector3 newPos = new Vector3(target.transform.position.x, target.transform.position.y, other.transform.position.z);
            Player_instance_JS.instance.transform.position = newPos;
            CameraMove_JS.instance.SnapToTarget();
            cc.enabled = true;  // �ٽ� Ȱ��ȭ
        }
    }
}
