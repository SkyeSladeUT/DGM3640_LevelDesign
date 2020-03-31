using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    //[HideInInspector] 
    public bool thrown = false;
    //[HideInInspector]
    public bool inHand = true;

    private void OnCollisionEnter(Collision other)
    {
        if (!inHand)
        {
            //Debug.Log("Thrown");
            thrown = true;
        }
    }
}
