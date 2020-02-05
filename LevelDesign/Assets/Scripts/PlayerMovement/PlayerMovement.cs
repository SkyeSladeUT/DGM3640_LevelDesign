﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    private bool canMove = true, canCrouch = true, isCrouched = false;
    public MovementData moveData;
    private Vector3 movement, scale;
    private float gravity;
    public float gravitySpeed;
    private bool onGround;
    private float origHeight, shrunkHeight;
    public float CrouchDecrease;

    private void Start()
    {
        onGround = false;
        cc = GetComponent<CharacterController>();
        StartCoroutine(Move());
        StartCoroutine(Crouch());
    }

    public void SwapData(MovementData newData)
    {
        moveData = newData;
    }

    private IEnumerator Move()
    {
        gravity = 1;
        movement = transform.forward*moveData.ForwardSpeed.GetFloat()*Time.deltaTime
                   + transform.right*moveData.SideSpeed.GetFloat()*Time.deltaTime
                   + transform.up*moveData.UpSpeed.GetFloat()*Time.deltaTime;
        movement.y -= gravity;
        cc.Move(movement); 
        gravity = 0;
        yield return  new WaitForFixedUpdate();
        while (canMove)
        {
            movement = transform.forward*moveData.ForwardSpeed.GetFloat()*Time.deltaTime
                + transform.right*moveData.SideSpeed.GetFloat()*Time.deltaTime
                + transform.up*moveData.UpSpeed.GetFloat()*Time.deltaTime; 
            if (isCrouched)
            {
                movement *= CrouchDecrease;
            }
            movement.y -= gravity;
            if (!cc.isGrounded)
            {
                Debug.Log("OffGround");
                if(gravity < 1)
                    gravity += Time.deltaTime * gravitySpeed;
            }
            else
            {
                gravity = 0;
            }
            cc.Move(movement);   
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator Crouch()
    {
        origHeight = transform.localScale.y;
        shrunkHeight = origHeight * .65f;
        scale = transform.localScale;
        while (canCrouch)
        {
            if (Input.GetButton("Crouch"))
            {
                isCrouched = true;
                scale.y = shrunkHeight;
            }

            else
            {
                isCrouched = false;
                scale.y = origHeight;
            }
            transform.localScale = scale;
            yield return new WaitForFixedUpdate();
        }
    }
}



