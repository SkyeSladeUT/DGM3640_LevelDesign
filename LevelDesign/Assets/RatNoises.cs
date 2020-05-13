using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatNoises : MonoBehaviour
{
    public AudioSource Squeak, Scratch;
    private bool called;

    private void Start()
    {
        called = false;
    }

    public void Call()
    {
        if (!called)
        {
            called = true;
            Scratch.Play();
            Squeak.Play();        }
    }

}
