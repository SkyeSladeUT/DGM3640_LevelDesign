using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    [HideInInspector]public bool canMove = true, canCrouch = true;
    [HideInInspector] public bool isCrouched = false, moving = false;
    public MovementData moveData;
    private Vector3 movement, scale, brickOrig, brickShrunk;
    private float gravity;
    public float gravitySpeed;
    private float origHeight, shrunkHeight;
    public float CrouchDecrease;
    private WaitForFixedUpdate _fixedUpdate;
    public Transform brickHold;

    private void Awake()
    {
        _fixedUpdate = new WaitForFixedUpdate();
    }

    public void Initialize()
    {
        canMove = true;
        cc = GetComponent<CharacterController>();
        StartCoroutine(Move());
        StartCoroutine(Crouch());
    }

    public void Stop()
    {
        canMove = false;
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
        yield return _fixedUpdate;
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
                if(gravity < 1)
                    gravity += Time.deltaTime * gravitySpeed;
            }
            else
            {
                gravity = 0;
            }
            if (movement.x!=0 || movement.z!=0)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }

            cc.Move(movement);   
            yield return _fixedUpdate;
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
            yield return _fixedUpdate;
        }
    }
}



