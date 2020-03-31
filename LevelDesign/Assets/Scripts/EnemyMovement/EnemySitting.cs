using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySitting : MonoBehaviour
{
    public GameObject menu;
    public GameObject player, playerCam;
    public bool heardplayer;
    public bool looking, stunned;
    public Animator anim;
    public float deathTime;
    private PlayerMovement pm;
    private bool running, end;
    public float lookTime, stunTime;
    public GameObject sightObj;
    private WaitForSeconds stunTimeWait, lookTimeWait, deathTimeWait, timeWait01;
    private WaitForFixedUpdate fixedUpdateWait;
    public CameraAnimation camAnim;
    private Vector3 direction;
    private Quaternion lookRotation;

    private void Awake()
    {
        stunTimeWait = new WaitForSeconds(stunTime);
        lookTimeWait = new WaitForSeconds(lookTime);
        deathTimeWait = new WaitForSeconds(deathTime);
        timeWait01 = new WaitForSeconds(.5f);
        fixedUpdateWait = new WaitForFixedUpdate();
    }

    public void Initialize()
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
        yield return stunTimeWait;
        ResetTriggers();
        anim.SetTrigger("EndHit");
        yield return timeWait01;
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
        yield return timeWait01;
        ResetTriggers();
        anim.SetTrigger("Gun");
        end = false;
        StartCoroutine(RotatePlayer());
        yield return deathTimeWait;
        end = true;
        camAnim.Death();
        yield return deathTimeWait;
        Time.timeScale = 0;
        menu.SetActive(true);
        running = false;
    }
    
    private IEnumerator RotatePlayer()
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        running = true;
        direction = (transform.position - playerCam.transform.position).normalized;
        while (!CheckRot(.05f, Quaternion.LookRotation(direction).eulerAngles) && !end)
        {
            direction = (transform.position - playerCam.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, lookRotation, Time.deltaTime * 5);
            yield return fixedUpdateWait;
        }
        playerCam.transform.rotation = lookRotation;
    }
    
    private bool CheckRot(float offset, Vector3 euler)
    {
        if (((transform.rotation.eulerAngles.y > euler.y - offset) && transform.rotation.eulerAngles.y < euler.y + offset))
            return true;
        else
            return false;
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
        yield return lookTimeWait;
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
