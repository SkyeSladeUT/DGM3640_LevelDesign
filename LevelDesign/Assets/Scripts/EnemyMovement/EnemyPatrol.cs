using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public List<Transform> destinations;
    private bool onPatrol;
    private int currentDest;
    private float distance;
    public float waitSeconds;
    public float speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
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
        StartCoroutine(Patrol());
    }

    private IEnumerator Patrol()
    {
        agent.destination = destinations[currentDest].position;
        while (onPatrol)
        {
            if (CheckDest(.05f, destinations[currentDest].position))
            {
                yield return new WaitForSeconds(waitSeconds);
                currentDest++;
                if (currentDest >= destinations.Count)
                    currentDest = 0;
            }

            agent.destination = destinations[currentDest].position;
            yield return new WaitForFixedUpdate();
        }
    }

    private bool CheckDest(float offset, Vector3 dest)
    {
        if (((transform.position.x > dest.x - offset) && transform.position.x < dest.x + offset)
            && ((transform.position.z > dest.z - offset) && transform.position.z < dest.z + offset))
            return true;
        else
            return false;
    }
    
    public void StopPatrol()
    {
        onPatrol = false;
        StopCoroutine(Patrol());
    }
}
