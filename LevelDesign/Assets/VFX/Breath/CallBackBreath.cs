using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBackBreath : MonoBehaviour
{
    public bool running;
    public float minwaittime, maxwaittime;

    public IEnumerator OnParticleSystemStopped()
    {
        if (running)
        {
            yield return new WaitForSeconds(Random.Range(minwaittime, maxwaittime));
            GetComponent<ParticleSystem>().Play();
        }
    }
}
