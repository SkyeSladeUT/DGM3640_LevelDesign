using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject menu;
    public GameObject player, playerCam;
    public EnemyPatrol patrol;
    public bool heardplayer;
    public bool distracted, stunned;
    public Animator anim;
    public float deathTime;
    private PlayerMovement pm;
    private bool running, end;
    private Vector3 direction;
    private Quaternion lookRotation;
    private WaitForFixedUpdate fixedUpdateWait;
    private WaitForSeconds deathTimeWait;
    public CameraAnimation camAnim;
    public AudioSource SurpriseGrunt, Gun, footSteps;
    

    private void Start()
    {
        fixedUpdateWait = new WaitForFixedUpdate();
        deathTimeWait = new WaitForSeconds(deathTime);
        pm = player.GetComponent<PlayerMovement>();
        running = false;
    }

    public void Attack()
    {
        if (!distracted && !stunned)
        {
            ResetTriggers();
            anim.SetTrigger("Gun");
            patrol.Freeze();
            pm.canMove = false;
            if(!running)
                StartCoroutine(RotateTowards(player.transform));
        }
    }

    private IEnumerator AttackFun()
    {
        end = false;
        StartCoroutine(RotatePlayer());
        end = true;
        yield return deathTimeWait;
        Gun.Play();
        camAnim.Death();
        yield return deathTimeWait;
        Time.timeScale = 0;
        menu.SetActive(true);
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

    public void HearPlayer()
    {
        if (!stunned && !heardplayer && !distracted)
        {
            if (!pm.isCrouched && player.GetComponent<PlayerMovement>().moving)
            {
                Debug.Log("heard player");
                SurpriseGrunt.Play();
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
            SurpriseGrunt.Play();
            patrol.GoToBrick(brick);
        }
    }

    public void Rotate()
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        if (!footSteps.isPlaying)
        {
            footSteps.Play();
        }
        patrol.TurnTowards(player.transform);
    }
    
    private void ResetTriggers()
    {
        anim.ResetTrigger("Walk");
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("LookAround");
        anim.ResetTrigger("Gun");
        anim.ResetTrigger("Hit");
        anim.ResetTrigger("StandUp");
    }
    
    private IEnumerator RotateTowards(Transform target)
    {
        running = true;
        ResetTriggers();
        anim.SetTrigger("Walk");
        if (!footSteps.isPlaying)
        {
            footSteps.Play();
        }
        direction = (target.position - transform.position).normalized;
        while (!CheckRot(.1f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (target.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,  1);
            yield return fixedUpdateWait;
        }
        transform.rotation = lookRotation;
        footSteps.Stop();
        StartCoroutine(AttackFun());

    }
    private bool CheckRot(float offset, Vector3 euler)
    {
        if (((transform.rotation.eulerAngles.y > euler.y - offset) && transform.rotation.eulerAngles.y < euler.y + offset))
            return true;
        else
            return false;
    }
    
    
}
