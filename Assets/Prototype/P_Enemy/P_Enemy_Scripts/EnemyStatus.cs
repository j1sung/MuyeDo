using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    private float maxHP = 100f;
    private float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public float GetHP() 
    {
        return currentHP;
    }

    public void SetHP(float Hp)
    {
        currentHP = Hp;
    }
}
