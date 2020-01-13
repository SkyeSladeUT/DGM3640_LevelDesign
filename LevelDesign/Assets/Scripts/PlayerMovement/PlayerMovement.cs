using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    private bool canMove = true;
    public MovementData moveData;
    private Vector3 movement;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        StartCoroutine(Move());
    }

    public void SwapData(MovementData newData)
    {
        moveData = newData;
    }

    private IEnumerator Move()
    {
        while (canMove)
        {
            movement = transform.forward*moveData.ForwardSpeed.GetFloat()*Time.deltaTime
                + transform.right*moveData.SideSpeed.GetFloat()*Time.deltaTime
                + transform.up*moveData.UpSpeed.GetFloat()*Time.deltaTime;
            cc.Move(movement);          
            yield return new WaitForFixedUpdate();
        }
    }
}
