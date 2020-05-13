using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{
    Light testLight;
        public float minWaitTime;
        public float maxWaitTime;
    
        void OnTriggerEnter(Collider Other)
        {
            testLight = GetComponent<Light>();
            StartCoroutine(Flash());
        }
    
        IEnumerator Flash()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                testLight.enabled = !testLight.enabled;
    
            }
        }
}
