using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light light;
    private float MinIntensity, MaxIntensity;
    public float dimAmount;
    public float minTimeBright, minTimeDark, maxTimeBright, maxTimeDark;
    public ParticleSystem ps;
    private ParticleSystem.ColorOverLifetimeModule color;
    private Color initColorMin, initColorMax;
    public Color clearColorMin, clearColorMax;
    private ParticleSystem.MinMaxGradient initcolorgrad, clearcolorgrad;

    private void Start()
    {
        light = GetComponent<Light>();
        MaxIntensity = light.intensity;
        MinIntensity = MaxIntensity - dimAmount;
        if (ps != null)
        {
            color = ps.colorOverLifetime;
            initcolorgrad = color.color;
            clearcolorgrad = new ParticleSystem.MinMaxGradient(clearColorMin, clearColorMax);
        }
        
        StartCoroutine(flicker());
    }

    private IEnumerator flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBright, maxTimeBright));
            if (ps != null)
            {
                color.color = clearcolorgrad;
            }

            light.intensity = MinIntensity;
            yield return new WaitForSeconds(Random.Range(minTimeDark, maxTimeDark));
            if (ps != null)
            {
                color.color = initcolorgrad;
            }

            light.intensity = MaxIntensity;
        }
    }
}
