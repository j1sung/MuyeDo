using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLayer : MonoBehaviour
{
    void Update()
    {
        int orderLayer = -(int)(transform.position.z * 100);
        GetComponent<SpriteRenderer>().sortingOrder = orderLayer;
    }
}
