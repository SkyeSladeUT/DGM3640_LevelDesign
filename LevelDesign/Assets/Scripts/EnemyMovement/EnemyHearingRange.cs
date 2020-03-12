using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearingRange : MonoBehaviour
{
    public GameObject player;
    private RaycastHit hit;
    public float hearingDistance;
    public EnemyAttack enemy;
    private GameObject brick;
    private bool playerinRange;
    public GameObject eyeline;

    private void Start()
    {
        brick = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Debug.Log("Brick Enter");
            brick = other.gameObject;
            if (!enemy.heardplayer && !brick.transform.parent.GetComponent<Brick>().thrown)
            {
                Debug.Log("Brick Go To");
                enemy.HearBrick(brick.transform.parent.transform);
            }
        }

        if (other.CompareTag("Player"))
        {
            playerinRange = true;
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
            if (playerinRange && !enemy.heardplayer)
            {
               enemy.HearPlayer();
            } 
        /*if (other.CompareTag("Brick"))
        {
            brick = other.gameObject;
            Debug.DrawRay(eyeline.transform.position, (brick.transform.parent.transform.position - eyeline.transform.position)*hit.distance);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = false;
            enemy.heardplayer = false;
        }
    }
}
