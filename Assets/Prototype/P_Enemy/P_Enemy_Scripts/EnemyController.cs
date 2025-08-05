using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyStatus enemyStatus; // �� �������ͽ� ����
    private float currentHP;

    [SerializeField]
    private float damageValue; // �޴� ������ ��ġ
    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    private void GetDamage()
    {
        currentHP = enemyStatus.GetHP();
        currentHP -= damageValue;
        enemyStatus.SetHP(currentHP);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            GetDamage();
        }
    }
}
