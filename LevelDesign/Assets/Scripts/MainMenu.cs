using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menuObj;
    public List<Text> menuOptions;
    public List<UnityEvent> menuEvents;
    public Color initColor, selectedColor;
    private int currentSelection;
    private WaitForFixedUpdate waitforfixed;
    private WaitUntil waituntil;
    private bool open;
    public UnityEvent onPause, onUnpause;

    private void Awake()
    {
        Screen.lockCursor = true;
        Open();
    }

    public void Open()
    {
        waitforfixed = new WaitForFixedUpdate();
        currentSelection = 0;
        menuOptions[0].color = selectedColor;
        if (menuOptions.Count > 1)
        {
            for (int i = 1; i < menuOptions.Count; i++)
            {
                menuOptions[i].color = initColor;
            }
        }
        waituntil = new WaitUntil(()=>Input.anyKeyDown);
        open = true;
        menuObj.SetActive(true);
        onPause.Invoke();
        StartCoroutine(SelectIndex());
    }

    private IEnumerator SelectIndex()
    {
        while (open)
        {
            yield return waituntil;
            if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                menuOptions[currentSelection].color = initColor;
                currentSelection++;
                if (currentSelection >= menuOptions.Count)
                {
                    currentSelection = 0;
                }
                menuOptions[currentSelection].color = selectedColor;
            }
            else if(Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                menuOptions[currentSelection].color = initColor;
                currentSelection--;
                if (currentSelection < 0)
                {
                    currentSelection = menuOptions.Count-1;
                }
                menuOptions[currentSelection].color = selectedColor;
            }
            else if (Input.GetButtonDown("Interact"))
            {
                menuEvents[currentSelection].Invoke();
            }

            yield return waitforfixed;
        }
    }

    public void Close()
    {
        onUnpause.Invoke();
        open = false;
        menuObj.SetActive(false);
    }
}
