using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySittingAttack : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;
    public bool heardplayer;
    public bool distracted, stunned;
    public Animator anim;
    public float deathTime;
    private PlayerMovement pm;
    private bool running;
    private Vector3 direction;
    private Quaternion lookRotation;
    public GameObject HeadRotateJoint;
    public float lookTime, rotationSpeed, stunTime;
    private Quaternion origRot;
    

    private void Start()
    {
        origRot = HeadRotateJoint.transform.rotation;
        pm = player.GetComponent<PlayerMovement>();
        running = false;
        distracted = false;
        stunned = false;
    }
    
    public void Stun()
    {
        ResetTriggers();
        anim.SetTrigger("Hit");
        Debug.Log("Stun");
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
        running = false;
    }

    public void Attack()
    {
        if (!distracted && !stunned)
        {
            Debug.Log("Attack");
            pm.canMove = false;
            StartCoroutine(AttackFun());
        }
    }

    private IEnumerator AttackFun()
    {
        ResetTriggers();
        anim.SetTrigger("Gun");
        yield return new WaitForSeconds(deathTime);
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void HearPlayer()
    {
        if (!stunned && !heardplayer)
        {
            if (!pm.isCrouched && player.GetComponent<PlayerMovement>().moving)
            {
                Debug.Log("heard player");
                if(!running)
                    StartCoroutine(LookAt(player.transform));
                distracted = false;
                heardplayer = true;
            }
        }
    }

    public void HearBrick(Transform brick)
    {
        if (!stunned)
        {
            Debug.Log("Look At Brick");
            if(!running)
                StartCoroutine(LookAtBrick(brick));
        }
    }

    public void Rotate()
    {
        ResetTriggers();
        anim.SetTrigger("Idle");
        StartCoroutine(LookAt(player.transform));
    }

    private IEnumerator LookAt(Transform lookObj)
    {
        //HeadRotateJoint.transform.rotation = origRot;
        anim.enabled = false;
        ResetTriggers();
        anim.SetTrigger("Idle");
        anim.speed = 0;
        running = true;
        direction = (lookObj.position - HeadRotateJoint.transform.position).normalized;
        while (!CheckRot(.5f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (lookObj.position - HeadRotateJoint.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            HeadRotateJoint.transform.rotation = Quaternion.Slerp(HeadRotateJoint.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
        HeadRotateJoint.transform.rotation = lookRotation;
        yield return new WaitForSeconds(lookTime);
        anim.enabled = true;
        anim.speed = 1;
        running = false;
    }
    
    private IEnumerator LookAtBrick(Transform lookObj)
    {
        //HeadRotateJoint.transform.rotation = origRot;
        anim.enabled = false;
        distracted = true;
        Debug.Log("Start Look At");
        ResetTriggers();
        anim.SetTrigger("Idle");
        anim.speed = 0;
        running = true;
        direction = (lookObj.position - HeadRotateJoint.transform.position).normalized;
        while (!CheckRot(.5f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (lookObj.position - HeadRotateJoint.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            HeadRotateJoint.transform.rotation = Quaternion.Slerp(HeadRotateJoint.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
        HeadRotateJoint.transform.rotation = lookRotation;
        yield return new WaitForSeconds(lookTime);
        Debug.Log("End Look At");
        distracted = false;
        anim.enabled = true;
        anim.speed = 1;
        running = false;
    }
    
    private bool CheckRot(float offset, Vector3 euler)
    {
        if (((HeadRotateJoint.transform.rotation.eulerAngles.y > euler.y - offset) && HeadRotateJoint.transform.rotation.eulerAngles.y < euler.y + offset))
            return true;
        else
            return false;
    }
    
    private void ResetTriggers()
    {
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("Gun");
        anim.ResetTrigger("Hit");
        anim.ResetTrigger("EndHit");
    }
}
