using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> destinations;
    private bool onPatrol, running;
    private int currentDest;
    private float distance;
    public float waitSeconds, stunTime, bricktime, scale;
    public float speed, rotationSpeed;
    private Vector3 direction, origPos;
    private Quaternion lookRotation, origRot;
    public bool Patroling;
    public EnemyAttack attack;
    public Animator anim;
    private WaitForFixedUpdate fixedUpdateWait;
    private WaitForSeconds lookTimeWait, stunTimeWait, delayWait, bricktimewait;
    public AudioSource footsteps, hitGrunt;


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
            footsteps.Stop();

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
        agent.updateRotation = false;
        direction = (destinations[currentDest].position - transform.position).normalized;
        if (GetRotateDirection(transform.rotation.eulerAngles, destinations[currentDest].position - transform.position))
        {
            anim.SetTrigger("TurnLeft");
        }
        else
        {
            anim.SetTrigger("TurnRight");
        }
        scale = 1;
        while (!CheckRot(1f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (destinations[currentDest].position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * scale);
            scale += Time.deltaTime;
            yield return fixedUpdateWait;
        }        
        ResetTriggers();
        anim.SetTrigger("Walk");
        footsteps.Play();
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
                currentDest++;
                if (currentDest >= destinations.Count)
                    currentDest = 0;
                 ResetTriggers();
                agent.updateRotation = false;
                direction = (destinations[currentDest].position - transform.position).normalized;
                scale = 1;
                footsteps.Stop();
                if (GetRotateDirection(transform.rotation.eulerAngles, destinations[currentDest].position - transform.position))
                {
                    //Debug.Log("Left");
                    anim.SetTrigger("TurnLeft");
                }
                else
                {
                    //Debug.Log("Right");
                    anim.SetTrigger("TurnRight");
                }
                while (!CheckRot(1f, Quaternion.LookRotation(direction).eulerAngles))
                {
                    direction = (destinations[currentDest].position - transform.position).normalized;
                    lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * scale);
                    scale += Time.deltaTime*2;
                    yield return fixedUpdateWait;
                }
                agent.updateRotation = true;
                ResetTriggers();
                anim.SetTrigger("Walk");
                footsteps.Play();

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
        running = true;
        direction = (origPos - transform.position).normalized;
        footsteps.Stop();
        if (GetRotateDirection(transform.rotation.eulerAngles, origPos - transform.position))
        {
            //Debug.Log("Left");
            anim.SetTrigger("TurnLeft");
        }
        else
        {
            //Debug.Log("Right");
            anim.SetTrigger("TurnRight");
        }
        agent.updateRotation = false;
        while (!CheckRot(.2f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (origPos - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return fixedUpdateWait;
        }

        agent.updateRotation = true;
        ResetTriggers();
        anim.SetTrigger("Walk");
        footsteps.Play();
        while (!CheckDest(.05f, origPos))
        {
            agent.destination = origPos;
            yield return fixedUpdateWait;
        }

        agent.updateRotation = false;
        while (!CheckRot(.05f, origRot.eulerAngles))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, origRot, Time.deltaTime * rotationSpeed);
            yield return fixedUpdateWait;
        }
        agent.updateRotation = true;
        ResetTriggers();
        anim.SetTrigger("LookAround");
        footsteps.Stop();
        running = false;
    }

    private IEnumerator RotateTowards(Transform target)
    {
        ResetTriggers();
        running = true;
        direction = (target.position - transform.position).normalized;
        footsteps.Stop();
        if (GetRotateDirection(transform.rotation.eulerAngles, destinations[currentDest].position - transform.position))
        {
            //Debug.Log("Left");
            anim.SetTrigger("TurnLeft");
        }
        else
        {
            //Debug.Log("Right");
            anim.SetTrigger("TurnRight");
        }

        agent.updateRotation = false;
        while (!CheckRot(.1f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (target.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return fixedUpdateWait;
        }
        transform.rotation = lookRotation;
        agent.updateRotation = true;
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
        footsteps.Play();
        if(Patroling)
            StopPatrol();
        StopAllCoroutines();
        StartCoroutine(LookAtBrick(brickDest));
    }

    public void GoToDest(Vector3 newDest)
    {
        ResetTriggers();
        anim.SetTrigger("Walk");
        footsteps.Play();
        if(Patroling)
            StopPatrol();
        StopAllCoroutines();
        StartCoroutine(GoTowards(newDest));
    }

    private IEnumerator LookAtBrick(Transform target)
    {
        ResetTriggers();
        agent.updateRotation = false;
        direction = (target.position - transform.position).normalized;
        footsteps.Stop();
        if (GetRotateDirection(transform.rotation.eulerAngles, target.position - transform.position))
        {
            anim.SetTrigger("TurnLeft");
        }
        else
        {
            anim.SetTrigger("TurnRight");
        }
        scale = 1;
        while (!CheckRot(2f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (target.position- transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * scale);
            scale += Time.deltaTime;
            yield return fixedUpdateWait;
        } 
        ResetTriggers();
        anim.SetTrigger("Walk");
        anim.speed *= 1.25f;
        agent.speed *= 3;
        footsteps.Play();
        running = true;
        while (!CheckDest(.05f, target.position))
        {
            agent.SetDestination(target.position);
            yield return fixedUpdateWait;
        }
        attack.distracted = true;
        ResetTriggers();
        anim.SetTrigger("Idle");
        footsteps.Stop();
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
        agent.updateRotation = false;
        direction = (target - transform.position).normalized;
        footsteps.Stop();
        if (GetRotateDirection(transform.rotation.eulerAngles, target - transform.position))
        {
            anim.SetTrigger("TurnLeft");
        }
        else
        {
            anim.SetTrigger("TurnRight");
        }
        scale = 1;
        while (!CheckRot(2f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (target- transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed * scale);
            scale += Time.deltaTime;
            yield return fixedUpdateWait;
        } 
        ResetTriggers();
        anim.SetTrigger("Walk");
        anim.speed = 1.25f;
        agent.speed *= 3;
        footsteps.Play();
        attack.distracted = false;
        running = true;
        while (!CheckDest(.1f, target))
        {
            agent.SetDestination(target);
            yield return fixedUpdateWait;
        }
        ResetTriggers();
        anim.SetTrigger("LookAround");
        footsteps.Stop();
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
        footsteps.Stop();
    }

    public void Stun()
    {
        ResetTriggers();
        footsteps.Stop();
        hitGrunt.Play();
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
        anim.speed = 1;
        agent.speed = speed;
        anim.ResetTrigger("Walk");
        anim.ResetTrigger("Idle");
        anim.ResetTrigger("LookAround");
        anim.ResetTrigger("Gun");
        anim.ResetTrigger("Hit");
        anim.ResetTrigger("StandUp");
        anim.ResetTrigger("TurnLeft");
        anim.ResetTrigger("TurnRight");
    }
    
    bool GetRotateDirection(Vector3 from, Vector3 to)
    {
        float clockWise = 0f;
        float counterClockWise = 0f;
 
        if (from.y <= to.y)
        {
            clockWise = to.y-from.y;
            counterClockWise = from.y + (360-to.y);
        }
        else
        {
            clockWise = (360-from.y) + to.y;
            counterClockWise = from.y-to.y;
        }
        return (clockWise <= counterClockWise);
    }
}
