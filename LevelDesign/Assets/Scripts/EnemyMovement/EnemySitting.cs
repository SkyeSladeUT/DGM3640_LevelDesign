using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySitting : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public bool heardplayer;
    public bool looking, stunned;
    public Animator anim;
    public float deathTime;
    private PlayerMovement pm;
    private bool running;
    public float lookTime, stunTime;
    public GameObject sightObj;

    private void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
        running = false;
        looking = false;
        stunned = false;
        ResetTriggers();
        sightObj.SetActive(false);
    }
    
    public void Stun()
    {
        ResetTriggers();
        anim.SetTrigger("Hit");
        if(!running)
            StartCoroutine(StunTimer());
    }
    
    private IEnumerator StunTimer()
    {
        running = true;
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        ResetTriggers();
        anim.SetTrigger("EndHit");
        yield return new WaitForSeconds(.5f);
        stunned = false;
        StartCoroutine(LookAt());
    }

    public void Attack()
    {
        if (looking && !stunned)
        {
            Debug.Log("Attack");
            pm.canMove = false;
            if(!running)
                StartCoroutine(AttackFun());
        }
    }

    private IEnumerator AttackFun()
    {
        running = true;
        ResetTriggers();
        anim.SetTrigger("LookUp");
        yield return new WaitForSeconds(.5f);
        ResetTriggers();
        anim.SetTrigger("Gun");
        yield return new WaitForSeconds(deathTime);
        Time.timeScale = 0;
        menu.SetActive(true);
        running = false;
    }

    public void HearPlayer()
    {
        if (!stunned && !heardplayer)
        {
            if (!pm.isCrouched && player.GetComponent<PlayerMovement>().moving)
            {
                heardplayer = true;
                if(!looking)
                    StartCoroutine(LookAt());       
            }
        }
    }

    public void HearBrick(Transform brick)
    {
        if (!stunned)
        {
            if(!looking)
                StartCoroutine(LookAt());
        }
    }

    public void Rotate()
    {
        StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        sightObj.SetActive(true);
        ResetTriggers();
        anim.SetTrigger("LookUp");
        looking = true;
        yield return new WaitForSeconds(lookTime);
        looking = false;
        anim.SetTrigger("Idle");
        sightObj.SetActive(false);
    }

    private void ResetTriggers()
    {
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Gun");
        anim.ResetTrigger("Hit");
        anim.ResetTrigger("EndHit");
        anim.ResetTrigger("LookUp");
    }
}
