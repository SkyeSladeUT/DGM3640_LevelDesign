using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public EnemyPatrol patrol;
    [HideInInspector] public bool heardplayer;
    [HideInInspector] public bool distracted, stunned;
    
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
            if (!player.GetComponent<PlayerMovement>().isCrouched)
            {
                heardplayer = true;
                patrol.GoToDest(player.transform.position);
            }

            heardplayer = false;
        }
    }

    public void HearBrick(Transform brick)
    {
        if(!stunned)
            patrol.GoToBrick(brick);
    }
    
}
