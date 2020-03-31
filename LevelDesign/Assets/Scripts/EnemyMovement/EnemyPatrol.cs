using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> destinations;
    private bool onPatrol, running;
    private int currentDest;
    private float distance;
    public float waitSeconds, stunTime, bricktime;
    public float speed, rotationSpeed;
    private Vector3 direction, origPos;
    private Quaternion lookRotation, origRot;
    public bool Patroling;
    public EnemyAttack attack;
    public Animator anim;
    private WaitForFixedUpdate fixedUpdateWait;
    private WaitForSeconds lookTimeWait, stunTimeWait, delayWait, bricktimewait;


    private void Awake()
    {
        fixedUpdateWait = new WaitForFixedUpdate();
        lookTimeWait = new WaitForSeconds(waitSeconds);
        stunTimeWait = new WaitForSeconds(stunTime);
        delayWait = new WaitForSeconds(.5f);
        bricktimewait = new WaitForSeconds(bricktime);
    }

    public void Initialize()
    {
        running = false;
        origPos = transform.position;
        origRot = transform.rotation;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        if (Patroling)
        {
            StartPatrol();
        }
        else
        {
            ResetTriggers();
            anim.SetTrigger("LookAround");
        }
    }

    public void Freeze()
    {
        agent.speed = 0;
    }
    
    public void StartPatrol()
    {
        onPatrol = true;
        attack.distracted = false;
        if (destinations.Count > 1)
        {
            distance = Mathf.Sqrt(Mathf.Pow(transform.position.x + destinations[0].position.x, 2)
                                  + Mathf.Pow(transform.position.z + destinations[0].position.z, 2));
            currentDest = 0;
            for (int i = 1; i < destinations.Count; i++)
            {
                if (Mathf.Sqrt(Mathf.Pow(transform.position.x + destinations[i].position.x, 2)
                               + Mathf.Pow(transform.position.z + destinations[i].position.z, 2)) < distance)
                {
                    distance = Mathf.Sqrt(Mathf.Pow(transform.position.x + destinations[i].position.x, 2)
                                          + Mathf.Pow(transform.position.z + destinations[i].position.z, 2));
                    currentDest = i;
                }
            }
        }
        if(!running)
            StartCoroutine(Patrol());
        else
        {
            StopAllCoroutines();
            StartCoroutine(Patrol());
        }
    }

    private IEnumerator Patrol()
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        running = true;
        attack.distracted = false;
        agent.destination = destinations[currentDest].position;
        while (onPatrol)
        {
            if (CheckDest(.05f, destinations[currentDest].position))
            {
                ResetTriggers();
                anim.SetTrigger("LookAround");
                yield return lookTimeWait;
                ResetTriggers();
                anim.SetTrigger("Walk");
                currentDest++;
                if (currentDest >= destinations.Count)
                    currentDest = 0;
            }

            agent.destination = destinations[currentDest].position;
            yield return fixedUpdateWait;
        }

        running = false;
    }

    private bool CheckDest(float offset, Vector3 dest)
    {
        if (((transform.position.x > dest.x - offset) && transform.position.x < dest.x + offset)
            && ((transform.position.z > dest.z - offset) && transform.position.z < dest.z + offset))
            return true;
        else
            return false;
    }

    private bool CheckRot(float offset, Vector3 euler)
    {
        if (((transform.rotation.eulerAngles.y > euler.y - offset) && transform.rotation.eulerAngles.y < euler.y + offset))
            return true;
        else
            return false;
    }
    
    public void StopPatrol()
    {
        //Debug.Log("Stop Patrol");
        running = false;
        onPatrol = false;
        StopCoroutine(Patrol());
    }

    public void TurnTowards(Transform obj)
    {
        //Debug.Log("Turn Towards");
        StopPatrol();
        agent.SetDestination(agent.transform.position);
        if(!running)
            StartCoroutine(RotateTowards(obj));
        else
        {
            StopAllCoroutines();
            StartCoroutine(RotateTowards(obj));
        }
    }

    private IEnumerator ReturnPos()
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        running = true;
        while (!CheckDest(.05f, origPos))
        {
            agent.destination = origPos;
            yield return fixedUpdateWait;
        }

        while (!CheckRot(.05f, origRot.eulerAngles))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, origRot, Time.deltaTime * rotationSpeed);
            yield return fixedUpdateWait;
        }
        ResetTriggers();
        anim.SetTrigger("LookAround");
        running = false;
    }

    private IEnumerator RotateTowards(Transform target)
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        running = true;
        direction = (target.position - transform.position).normalized;
        while (!CheckRot(.05f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (target.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return fixedUpdateWait;
        }
        transform.rotation = lookRotation;
        yield return lookTimeWait;
        if (Patroling)
        {
            running = false;
            StartPatrol();
        }
        else
        {
            running = false;
            StartCoroutine(ReturnPos());
        }

    }

    public void GoToBrick(Transform brickDest)
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        if(Patroling)
            StopPatrol();
        StopAllCoroutines();
        StartCoroutine(LookAtBrick(brickDest));
    }

    public void GoToDest(Vector3 newDest)
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        if(Patroling)
            StopPatrol();
        StopAllCoroutines();
        StartCoroutine(GoTowards(newDest));
    }

    private IEnumerator LookAtBrick(Transform target)
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        running = true;
        while (!CheckDest(.05f, target.position))
        {
            agent.SetDestination(target.position);
            yield return fixedUpdateWait;
        }
        attack.distracted = true;
        ResetTriggers();
        anim.SetTrigger("Idle");
        yield return bricktimewait;
        attack.distracted = false;
        if (Patroling)
        {
            running = false;
            StartPatrol();
        }
        else
        {
            running = false;
            StartCoroutine(ReturnPos());
        }
    }

    private IEnumerator GoTowards(Vector3 target)
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        attack.distracted = false;
        running = true;
        while (!CheckDest(.05f, target))
        {
            agent.SetDestination(target);
            yield return fixedUpdateWait;
        }
        ResetTriggers();
        anim.SetTrigger("LookAround");
        yield return lookTimeWait;
        attack.heardplayer = false;
        if (Patroling)
        {
            running = false;
            StartPatrol();
        }
        else
        {
            running = false;
            StartCoroutine(ReturnPos());
        }
    }

    public void LookAround()
    {
        ResetTriggers();
        anim.SetTrigger("LookAround");
    }

    public void Stun()
    {
        ResetTriggers();
        anim.SetTrigger("Hit");
        Debug.Log("Stun");
        agent.speed = 0;
        StartCoroutine(StunTimer());
    }

    public IEnumerator StunTimer()
    {
        running = true;
        attack.stunned = true;
        yield return stunTimeWait;
        ResetTriggers();
        anim.SetTrigger("StandUp");
        yield return delayWait;
        attack.stunned = false;
        agent.speed = speed;
        running = false;
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
    
}
