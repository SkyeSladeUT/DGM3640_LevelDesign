using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatedSightRange : MonoBehaviour
{
    public GameObject player;
    private RaycastHit hit;
    public float sightDistance;
    public EnemySittingAttack enemy;
    public GameObject eyeline;
    private Vector3  newscale;



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !enemy.distracted)
        {  
            if (Physics.Raycast(eyeline.transform.position, (player.transform.position - eyeline.transform.position), out hit, sightDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Saw Player");
                    enemy.Attack();
                }
            }
        }
    }
}
