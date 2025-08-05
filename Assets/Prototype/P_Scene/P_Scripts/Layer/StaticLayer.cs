using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLayer : MonoBehaviour
{
    void Start()
    {
        int orderLayer = -(int)(transform.position.z * 100);
        GetComponent<SpriteRenderer>().sortingOrder = orderLayer;
    }
}
