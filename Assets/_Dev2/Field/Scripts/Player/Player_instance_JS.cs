using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_instance_JS : MonoBehaviour
{
    public string saveSceneName; // ExitPoint ��ũ��Ʈ�� �ִ� saveSceneName ������ �� ����

    private static Player_instance_JS _instance;
    public static Player_instance_JS instance
    {
        get
        {
            if (_instance == null)
            {
                // �� ���� �̹� �����ϴ� Player_instance_JS ã��
                _instance = FindObjectOfType<Player_instance_JS>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
