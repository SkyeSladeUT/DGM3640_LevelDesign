using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingCloseRange : MonoBehaviour
{
    public EnemySitting enemy;
    public GameObject eyeline;
    public GameObject player;
    private RaycastHit hit;
    public float sightDistance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")  && !enemy.stunned && Physics.Raycast(eyeline.transform.position, (player.transform.position - eyeline.transform.position), out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                enemy.Rotate();
            }
        }
    }
}
