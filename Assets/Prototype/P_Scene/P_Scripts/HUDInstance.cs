using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDInstance : MonoBehaviour
{
    private static HUDInstance _instance;

    public static HUDInstance instance
    {
        get
        {
            if (_instance == null)
            {
                // �� ���� �̹� �����ϴ� Move_JS�� ã��
                _instance = FindObjectOfType<HUDInstance>();
            }
            return _instance;
        }
    }
    void Awake()
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
