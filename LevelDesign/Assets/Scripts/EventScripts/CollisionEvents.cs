using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour
{
    public List<string> tags;
    public UnityEvent CollisionEnter, CollisionStay, CollisionExit;

    private void OnCollisionEnter(Collision other)
    {
        if(tags.Contains(other.gameObject.tag))
            CollisionEnter.Invoke();
    }

    private void OnCollisionExit(Collision other)
    {
        if(tags.Contains(other.gameObject.tag))
            CollisionExit.Invoke();
    }

    private void OnCollisionStay(Collision other)
    {
        if(tags.Contains(other.gameObject.tag))
            CollisionStay.Invoke();
    }
}
