using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Float")]
public class FloatData : ScriptableObject
{
    public float value;

    public void increase(float amount)
    {
        value += amount;
    }

    public void decrease(float amount)
    {
        value -= amount;
    }

    public virtual float GetFloat()
    {
        return value;
    }
}
