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
            brick = other.gameObject;
            if (Physics.Raycast(eyeline.transform.position, (brick.transform.position - eyeline.transform.position), out hit,
                hearingDistance))
            {
                if(hit.collider.CompareTag("Brick"))
                    if (!enemy.heardplayer)
                        enemy.HearBrick(brick.transform);
            }
        }

        if (other.CompareTag("Player"))
        {
            playerinRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
            if (playerinRange && Physics.Raycast(eyeline.transform.position, (player.transform.position - eyeline.transform.position), out hit,
                hearingDistance))
            {
                if (hit.collider.CompareTag("Player") && !enemy.heardplayer)
                {
                    enemy.HearPlayer();
                }
            } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = false;
        }
    }
}
