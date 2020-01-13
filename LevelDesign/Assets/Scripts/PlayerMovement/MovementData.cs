using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Movement/Data")]
public class MovementData : ScriptableObject
{
    public FloatData ForwardSpeed, SideSpeed, UpSpeed;
}
