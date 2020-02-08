using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent TriggerEnter, TriggerStay, TriggerExit;
    public List<string> tags;

    private void OnTriggerEnter(Collider other)
    {
        if(tags.Contains(other.tag))
            TriggerEnter.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if(tags.Contains(other.tag))
            TriggerStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if(tags.Contains(other.tag))
            TriggerExit.Invoke();
    }
}
