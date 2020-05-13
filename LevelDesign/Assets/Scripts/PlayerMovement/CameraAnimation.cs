using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    public Animator anim;
    public float floory, speed;
    private Vector3 movement;
    public CameraMovement cammove;

    private void Start()
    {
        anim.enabled = false;
    }

    public void Death()
    {
        cammove.SetCanMove(false);
        anim.enabled = true;
        movement = transform.localPosition;
        movement.y = floory;
        transform.localPosition = movement;
    }

}
