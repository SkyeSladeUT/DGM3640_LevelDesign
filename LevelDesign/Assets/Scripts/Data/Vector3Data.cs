using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Vector3")]
public class Vector3Data : ScriptableObject
{
    public Vector3 vector;

    public void SetVector(Vector3 newVec)
    {
        vector = newVec;
    }

    public void SetPosition(Transform transform)
    {
        vector = transform.position;
    }
}
