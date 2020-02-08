using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    public string levelName;


    private void Update()
    {
        if (Time.timeScale <= 0)
        {
            if (Input.GetButtonDown("Restart"))
            {
                SceneManager.LoadScene(levelName);
            }
        }   
    }
}
