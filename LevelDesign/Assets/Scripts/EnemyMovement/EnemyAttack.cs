using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public EnemyPatrol patrol;
    public bool heardplayer;
    public bool distracted, stunned;
    
    public void Attack()
    {
        if (!distracted && !stunned)
        {
            //Put Animations and timer
            Time.timeScale = 0;
            menu.SetActive(true);
        }
    }

    public void HearPlayer()
    {
        if (!stunned && !heardplayer)
        {
            if (!player.GetComponent<PlayerMovement>().isCrouched && player.GetComponent<PlayerMovement>().moving)
            {
                Debug.Log("heard player");
                distracted = false;
                heardplayer = true;
                patrol.GoToDest(player.transform.position);
            }
        }
    }

    public void HearBrick(Transform brick)
    {
        if (!stunned)
        {
            Debug.Log("Go To Brick");
            patrol.GoToBrick(brick);
        }
    }

    public void Rotate()
    {
        patrol.TurnTowards(player.transform);
    }
    
}
