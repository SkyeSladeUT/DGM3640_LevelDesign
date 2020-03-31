﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItem : MonoBehaviour
{
    public PlayerData player;
    private bool forceSet, increase;
    private float forceAmount;
    public Transform throwDirection;
    public float forceMin, forceMax, changeSpeed;
    private Vector3 forceDirection;
    public Image forceImage;
    private WaitForSeconds delayTime;
    private Brick brick;
    private WaitForFixedUpdate _fixedUpdate;

    private void Start()
    {
        delayTime = new WaitForSeconds(.1f);
        _fixedUpdate = new WaitForFixedUpdate();
        forceSet = false;
    }

    public void StartThrow()
    {
        StartCoroutine(Throw());
    }

    private IEnumerator Throw()
    {
        brick = player.brickRB.gameObject.GetComponent<Brick>();
        while (player.hasBrick)
        {
            brick.thrown = false;
            if (Input.GetMouseButtonDown(0))
            {
                forceSet = true;
                //StartCoroutine(SetForce());
                forceAmount = forceMin;
                increase = true;
                while (forceSet)
                {
                    if (Input.GetMouseButtonUp(0) && forceSet)
                    {
                        forceSet = false;
                        forceDirection = throwDirection.forward * forceAmount;
                        player.brickRB.transform.parent = null;
                        player.brickRB.constraints = RigidbodyConstraints.None;
                        player.brickRB.AddForce(forceDirection, ForceMode.Impulse);
                        yield return delayTime;
                        brick.inHand = false;
                        player.throwBrick();
                        forceImage.fillAmount = 0;
                    }
                    else
                    {
                        //set force
                        if (forceAmount >= forceMax)
                            increase = false;

                        else if (forceAmount <= forceMin)
                            increase = true;

                        if (increase)
                            forceAmount += Time.deltaTime * changeSpeed;

                        else
                            forceAmount -= Time.deltaTime * changeSpeed;

                        forceImage.fillAmount = (forceAmount - forceMin) / (forceMax - forceMin);
                    }

                    yield return _fixedUpdate;
                    
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                player.brickRB.transform.parent = null;
                player.brickRB.constraints = RigidbodyConstraints.None;
                player.brickRB.gameObject.GetComponent<Brick>().inHand = false;
                player.throwBrick();
            }
            yield return _fixedUpdate;
        }
    }

    private IEnumerator SetForce()
    {
        forceAmount = forceMin;
        increase = true;
        while (forceSet)
        {
            //set force
            if (forceAmount >= forceMax)
                increase = false;
 
            else if (forceAmount <= forceMin)
                increase = true;

            if (increase)
                forceAmount += Time.deltaTime * changeSpeed;

            else
                forceAmount -= Time.deltaTime*changeSpeed;

            forceImage.fillAmount = (forceAmount-forceMin) / (forceMax-forceMin);
            
            yield return _fixedUpdate;
        }
    }
    
    
}
