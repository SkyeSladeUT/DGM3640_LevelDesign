using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartEvents : MonoBehaviour
{
    public UnityEvent OnStart;

    private void Start()
    {
        Time.timeScale = 1;
        OnStart.Invoke();
    }
}
