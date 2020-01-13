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
            quat = Quaternion.Euler(rotation);
            transform.rotation = quat;
            //transform.Rotate(y*CamSpeed, x*CamSpeed, 0);
            /*float mouseY = (Input.mousePosition.y / Screen.height) - 0.5f;
            float mouseX = (Input.mousePosition.x / Screen.width) - .5f;
            transform.localRotation =
                Quaternion.Euler(new Vector4(-1f * (mouseY * 100f), mouseX * 180f, transform.localRotation.z));
            */yield return new WaitForFixedUpdate();
        }
    }
}
