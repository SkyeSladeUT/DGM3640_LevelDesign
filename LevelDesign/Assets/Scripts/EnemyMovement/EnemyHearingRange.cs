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
    private Brick brickScript;
    private Transform brickTrans;
    private int layermask;

    private void Start()
    {
        layermask = LayerMask.GetMask("Door");
        layermask = ~layermask;
        brick = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Debug.Log("Brick Enter");
            brick = other.gameObject;
            brickScript = brick.transform.GetComponent<Brick>();
            brickTrans = brick.transform;
            if (brickScript == null)
            {
                brickScript = brick.transform.parent.GetComponent<Brick>();
                brickTrans = brick.transform.parent.transform;
            }
            if (!enemy.heardplayer && !brickScript.thrown)
            {
                Debug.Log("Brick Go To");
                enemy.HearBrick(brickTrans);
            }
        }

        if (other.CompareTag("Player"))
        {
            playerinRange = true;
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (playerinRange && !enemy.heardplayer && !enemy.distracted)
        {
            if (HearThroughWalls)
                enemy.HearPlayer();
            else
            {
                if (Physics.Raycast(eyeline.transform.position,
                    (player.transform.position - eyeline.transform.position), out hit, hearingDistance, layermask))
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
