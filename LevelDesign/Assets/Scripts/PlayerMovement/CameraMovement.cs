using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool canMove = true;
    private Vector3 movement, rotation;
    private Quaternion quat;
    public Transform followObj;
    private float x, y;
    public float CamSpeed;
    public float sensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(CamMove());
    }

    private IEnumerator CamMove()
    {
        while (canMove)
        {
            movement = followObj.position;
            movement.y = transform.position.y;
            transform.position = movement;
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
            rotation = transform.rotation.eulerAngles;
            rotation.x -= y * CamSpeed;
            rotation.y += x * CamSpeed;
            rotation.z = 0;
            quat = Quaternion.Euler(rotation);
            //transform.rotation = quat;
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, sensitivity*Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
