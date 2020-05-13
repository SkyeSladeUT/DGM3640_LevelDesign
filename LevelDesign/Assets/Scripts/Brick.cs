using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    //[HideInInspector] 
    public bool thrown = false;
    //[HideInInspector]
    public bool inHand = true;
    public AudioSource brickhit;

    private void OnCollisionEnter(Collision other)
    {
        if (!inHand && !other.collider.CompareTag("ignoreTag") )
        {
            //Debug.Log("Thrown");
            thrown = true;
            brickhit.Play();
        }
    }
}
