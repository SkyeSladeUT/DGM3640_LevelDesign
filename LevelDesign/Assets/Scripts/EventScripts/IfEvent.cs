using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IfEvent : MonoBehaviour
{
    public UnityEvent IfTrue, IfFalse;

    public void CheckBool(BoolData b)
    {
        if (b.value)
        {
            IfTrue.Invoke();
        }
        else
        {
            IfFalse.Invoke();
        }
    }
}
