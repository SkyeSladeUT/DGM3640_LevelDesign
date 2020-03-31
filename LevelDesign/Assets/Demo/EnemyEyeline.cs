using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEyeline : MonoBehaviour
{
    public GameObject player;
    private RaycastHit hit;
    public float sightDistance;
    public GameObject eyeline;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {  
            if (Physics.Raycast(eyeline.transform.position, (player.transform.position - eyeline.transform.position), out hit, sightDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player Seen");
                    //Attack Function
                }
            }
        }
    }
}
