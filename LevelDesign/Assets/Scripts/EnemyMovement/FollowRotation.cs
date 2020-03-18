using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform followRot;
    private Vector3 rotationChange;

    private void FixedUpdate()
    {
        rotationChange = transform.rotation.eulerAngles;
        rotationChange.y = followRot.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(rotationChange);
    }
}
