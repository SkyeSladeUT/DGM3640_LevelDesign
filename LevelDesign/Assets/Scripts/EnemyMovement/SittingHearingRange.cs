using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingHearingRange : MonoBehaviour
{
    public GameObject player;
    private RaycastHit hit;
    public float hearingDistance;
    public EnemySitting enemy;
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
            if (!enemy.heardplayer && !brick.transform.parent.GetComponent<Brick>().thrown)
            {
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
        if (playerinRange)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = false;
            enemy.heardplayer = false;
        }
    }
}
