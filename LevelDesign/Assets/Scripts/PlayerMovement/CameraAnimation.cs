using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    public Animator anim;
    public float floory, speed;
    private Vector3 movement;

    private void Start()
    {
        anim.enabled = false;
    }

    public void Death()
    {
        anim.enabled = true;
        movement = transform.position;
        movement.y = floory;
        transform.position = movement;
    }

}
