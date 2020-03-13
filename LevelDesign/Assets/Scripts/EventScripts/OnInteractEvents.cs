using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnInteractEvents : MonoBehaviour
{
    public List<string> TagsToCompare;
    private bool _inRange, _isRunning;
    public UnityEvent OnInteract;

    private void OnTriggerEnter(Collider other)
    {
        if (TagsToCompare.Contains(other.tag))
        {
            _inRange = true;
            if (!_isRunning)
            {
                _isRunning = true;
                StartCoroutine(CheckInteract());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagsToCompare.Contains(other.tag))
        {
            _inRange = false;
            StopCoroutine(CheckInteract());
        }
    }

    IEnumerator CheckInteract()
    {
        while (_inRange)
        {
            if (Input.GetButtonDown("Interact"))
            {
                OnInteract.Invoke();
            }
            yield return new WaitForFixedUpdate();
        }

        _isRunning = false;
    }
}
