using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Cinemachine.Utility;
using UnityEditor.Experimental.GraphView;
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
    public float waitSeconds, stunTime;
    public float speed, rotationSpeed;
    private Vector3 direction, origPos;
    private Quaternion lookRotation, origRot;
    public bool Patroling;
    public EnemyAttack attack;
    

    private void Awake()
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
    }

    public void StartPatrol()
    {
        onPatrol = true;
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
        running = true;
        agent.destination = destinations[currentDest].position;
        while (onPatrol)
        {
            if (CheckDest(.05f, destinations[currentDest].position))
            {
                yield return new WaitForSeconds(waitSeconds);
                currentDest++;
                if (currentDest >= destinations.Count)
                    currentDest = 0;
                //Debug.Log("Swap Dest, Current Dest: " + currentDest);
            }

            agent.destination = destinations[currentDest].position;
            yield return new WaitForFixedUpdate();
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
        //Debug.Log("Return to Position");
        running = true;
        while (!CheckDest(.05f, origPos))
        {
            agent.destination = origPos;
            yield return new WaitForFixedUpdate();
        }

        while (!CheckRot(.05f, origRot.eulerAngles))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, origRot, Time.deltaTime * rotationSpeed);
            yield return new WaitForFixedUpdate();
        }

        running = false;
    }

    private IEnumerator RotateTowards(Transform target)
    {
        //Debug.Log("Rotate Towards");
        running = true;
        direction = (target.position - transform.position).normalized;
        while (!CheckRot(.05f, Quaternion.LookRotation(direction).eulerAngles))
        {
            direction = (target.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            yield return new WaitForFixedUpdate();
        }
        transform.rotation = lookRotation;
        yield return new WaitForSeconds(waitSeconds);
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
        //Debug.Log("Go to Brick");
        if(Patroling)
            StopPatrol();
        StopAllCoroutines();
        StartCoroutine(LookAtBrick(brickDest));
    }

    public void GoToDest(Vector3 newDest)
    {
        //Debug.Log("Go To Destination");
        if(Patroling)
            StopPatrol();
        StopAllCoroutines();
        StartCoroutine(GoTowards(newDest));
    }

    private IEnumerator LookAtBrick(Transform target)
    {
        //Debug.Log("Look at Brick");
        running = true;
        while (!CheckDest(.05f, target.position))
        {
            agent.SetDestination(target.position);
            yield return new WaitForFixedUpdate();
        }
        attack.distracted = true;
        yield return new WaitForSeconds(waitSeconds);
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
        running = true;
        while (!CheckDest(.05f, target))
        {
            agent.SetDestination(target);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(waitSeconds);
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
        
    }

    public void LookUp()
    {
        
    }

    public void Stun()
    {
        agent.speed = 0;
        StartCoroutine(StunTimer());
    }

    public IEnumerator StunTimer()
    {
        running = true;
        //Debug.Log("Stunned");
        attack.stunned = true;
        yield return new WaitForSeconds(stunTime);
        //Debug.Log("UnStunned");
        attack.stunned = false;
        agent.speed = speed;
        running = false;
    }
    
}
