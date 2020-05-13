using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door_Rotate : MonoBehaviour
{
    public GameObject Hinge;
    public float RotateAmount, Speed;
    private bool open, inRange, running;
    private float rotateAmount, totalRotateAmount, partialRotate;
    private WaitForFixedUpdate _fixedUpdate;
    public AudioSource DoorSound;

    private void Start()
    {
        _fixedUpdate = new WaitForFixedUpdate();
        open = false;
        running = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if (!running)
            {
                running = true;
                StartCoroutine(openDoor());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    IEnumerator openDoor()
    {
        while (inRange)
        {
            //Debug.Log("In Range");
            if (Input.GetButtonDown("Interact"))
            {
                if (open)
                {
                    //Debug.Log("Open");
                    rotateAmount = RotateAmount;
                    open = false;
                }
                else
                {
                    rotateAmount = -RotateAmount;
                    open = true;
                }
                DoorSound.Play();
                totalRotateAmount = 0;
                while (totalRotateAmount >= rotateAmount+1f || totalRotateAmount <= rotateAmount - 1f)
                {
                    partialRotate = rotateAmount * Speed * Time.deltaTime;
                    transform.RotateAround(Hinge.transform.position, Vector3.up, partialRotate);
                    totalRotateAmount += partialRotate;
                    yield return _fixedUpdate;
                }
            }
            yield return _fixedUpdate;
        }
        //Debug.Log("Finish");
        running = false;
    }
}
