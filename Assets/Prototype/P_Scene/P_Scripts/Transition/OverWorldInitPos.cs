using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldInitPos : MonoBehaviour
{
    [SerializeField]
    private Transform initPos;

    void Start()
    {
        transform.position = initPos.transform.position;
    }
}
