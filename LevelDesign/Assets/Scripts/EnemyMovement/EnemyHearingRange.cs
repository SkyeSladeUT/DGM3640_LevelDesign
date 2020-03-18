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
    public bool HearThroughWalls = true;

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
            if (HearThroughWalls)
                enemy.HearPlayer();
            else
            {
                if (Physics.Raycast(eyeline.transform.position,
                    (player.transform.position - eyeline.transform.position), out hit, hearingDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        enemy.HearPlayer();
                    }
                }
            }
        }
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
