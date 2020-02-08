using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloseRange : MonoBehaviour
{
    public EnemyAttack enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")  && !enemy.stunned)
        {
            enemy.Attack();
        }
    }
}
