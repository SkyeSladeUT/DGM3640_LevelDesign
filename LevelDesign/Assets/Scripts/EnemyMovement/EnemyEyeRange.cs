using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEyeRange : MonoBehaviour
{
    public GameObject player;
    private RaycastHit hit;
    public float sightDistance;
    public EnemyAttack enemy;
    public GameObject eyeline;
    private Vector3 origscale, newscale;

    private void Start()
    {
        origscale = transform.localScale;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !enemy.distracted)
        {  
            //Debug.Log("Seen Player");
            //Debug.DrawLine(eyeline.transform.position, (player.transform.position - eyeline.transform.position)*sightDistance);
            if (Physics.Raycast(eyeline.transform.position, (player.transform.position - eyeline.transform.position), out hit, sightDistance))
            {
                //Debug.Log("Hit");
                if (hit.collider.CompareTag("Player"))
                {
                    //Debug.Log("Hit Player");
                    enemy.Attack();
                }
            }
        }
    }
}
