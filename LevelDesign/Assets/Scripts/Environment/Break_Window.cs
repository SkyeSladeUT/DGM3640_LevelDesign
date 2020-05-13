﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Break_Window : MonoBehaviour
{
    public GameObject UnBrokenWindow, BrokenWindow;
    private bool broken;
    public AudioSource breakWindow;

    private void Start()
    {
        broken = false;
        UnBrokenWindow.SetActive(true);
        BrokenWindow.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit");
        if (!broken && other.CompareTag("Brick"))
        {
            //Debug.Log("Break");
            breakWindow.Play();
            broken = true;
            UnBrokenWindow.SetActive(false);
            BrokenWindow.SetActive(true);
        }
    }
}
