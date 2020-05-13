using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuObj;
    public List<Text> menuOptions;
    public List<UnityEvent> menuEvents;
    public Color initColor, selectedColor;
    private int currentSelection;
    private bool open;
    public MusicPause music;

    private void Awake()
    {
        open = false;
    }

    public void Open()
    {
        Time.timeScale = 0;
        currentSelection = 0;
        music.PauseAudio();
        menuOptions[0].color = selectedColor;
        if (menuOptions.Count > 1)
        {
            for (int i = 1; i < menuOptions.Count; i++)
            {
                menuOptions[i].color = initColor;
            }
        }
        open = true;
        menuObj.SetActive(true);
    }

    private void Update()
    {
        if (open)
        {
            if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
            {
                if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") > 0)
                {
                    menuOptions[currentSelection].color = initColor;
                    currentSelection++;
                    if (currentSelection >= menuOptions.Count)
                    {
                        currentSelection = 0;
                    }

                    menuOptions[currentSelection].color = selectedColor;
                }
                else if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") < 0)
                {
                    menuOptions[currentSelection].color = initColor;
                    currentSelection--;
                    if (currentSelection < 0)
                    {
                        currentSelection = menuOptions.Count - 1;
                    }

                    menuOptions[currentSelection].color = selectedColor;
                }
            }
            if (Input.GetButtonDown("Interact"))
            {
                menuEvents[currentSelection].Invoke();
            }

            if (Input.GetButtonDown("Pause"))
            {
                Close();
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause"))
            {
                Open();
            }
        }
    }

    public void Close()
    {
        Debug.Log("Run Close");
        open = false;
        music.ResumeAudio();
        menuObj.SetActive(false);
        Time.timeScale = 1;
    }
}
