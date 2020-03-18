using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingSightRange : MonoBehaviour
{
    public GameObject player;
    private RaycastHit hit;
    public float sightDistance;
    public EnemySitting enemy;
    public GameObject eyeline;
    private Vector3  newscale;
    private bool inRange;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (inRange && enemy.looking)
        {  
            if (Physics.Raycast(eyeline.transform.position, (player.transform.position - eyeline.transform.position), out hit, sightDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemy.Attack();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
