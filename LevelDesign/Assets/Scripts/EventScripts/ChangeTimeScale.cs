using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ChangeTimeScale : MonoBehaviour
{
    public void SetScale(float scale)
    {
        Time.timeScale = scale;
    }
}
