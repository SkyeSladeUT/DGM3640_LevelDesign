using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/FloatInput")]
public class FloatDataInput : FloatData
{
    public String AxisName;

    public override float GetFloat()
    {
        return Input.GetAxis(AxisName) * value;
    }
}
