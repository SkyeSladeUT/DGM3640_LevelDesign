using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlMenu : MonoBehaviour
{
    public GameObject ControlsObj;
    public UnityEvent onClose;
    private bool open;

    private void Awake()
    {
        ControlsObj.SetActive(false);
        open = false;
    }

    public void StartCheck()
    {
        open = true;
        Time.timeScale = 0;
        ControlsObj.SetActive(true);
    }

    private void Update()
    {
        if (open)
        {
            if (Input.anyKeyDown)
            {
                open = false;
                ControlsObj.SetActive(false);
                onClose.Invoke();
            }
        }
        else
        {
            if (Input.GetButtonUp("Interact"))
            {
                open = true;
            }
        }
    }
}
