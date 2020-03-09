using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [HideInInspector] public bool thrown = false;

    private void OnCollisionEnter(Collision other)
    {
        thrown = true;
    }
}
