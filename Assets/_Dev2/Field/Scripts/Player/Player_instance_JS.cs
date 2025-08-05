using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_instance_JS : MonoBehaviour
{
    public string saveSceneName; // ExitPoint 스크립트에 있는 saveSceneName 변수의 값 저장

    private static Player_instance_JS _instance;
    public static Player_instance_JS instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬 내에 이미 존재하는 Player_instance_JS 찾음
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
