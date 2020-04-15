using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PaperMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float minForce, maxForce;
    private Vector3 randDirection, speed;
    private float time;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        randDirection = Vector3.zero;
        speed = Vector3.zero;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            randDirection.x = Random.Range(0, 2);
            randDirection.z = Random.Range(0, 2);
            yield return new WaitUntil(CheckVelocity);
        }
    }

    private bool CheckVelocity()
    {
        if (rb.velocity.x <= 1f && rb.velocity.x >= -1f && rb.velocity.z <= 1f && rb.velocity.z >= -1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
