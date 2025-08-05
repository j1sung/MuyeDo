using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit");
    }
}
