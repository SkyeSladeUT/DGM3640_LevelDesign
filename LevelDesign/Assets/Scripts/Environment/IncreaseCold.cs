using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class IncreaseCold : MonoBehaviour
{
    public float IncreaseAmount;
    private float total;
    public PostProcessVolume volume;
    private Vignette vignette;

    private void Start()
    {
        total = 0;
        volume.profile.TryGetSettings(out vignette);
    }

    IEnumerator ColdChange()
    {
        yield return new WaitForFixedUpdate();
    }
    
     
}
