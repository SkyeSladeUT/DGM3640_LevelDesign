using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    private bool canMove = true, returnpos;
    private Vector3 movement, rotation, offset;
    private Quaternion quat;
    public Transform followObj;
    private float x, y;
    public float CamSpeed;
    public float sensitivity;
    private WaitForFixedUpdate _fixedUpdate;
    private float ShakeAmount=0, ShakeTime=0;
    private float time;
    private float changeAmountX, changeAmounty;

    private void Awake()
    {
        Screen.lockCursor = true;
        _fixedUpdate = new WaitForFixedUpdate();
    }

    public void SetShake(float amount, float time)
    {
        ShakeAmount = amount;
        ShakeTime = time;
    }

    public void Initialize()
    {
        offset = transform.position - followObj.position;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(CamMove());
    }

    private IEnumerator CamMove()
    {
        time = 0;
        changeAmountX = Random.RandomRange(-ShakeAmount, ShakeAmount);
        changeAmounty = Random.RandomRange(-ShakeAmount, ShakeAmount);
        while (canMove)
        {
            movement = followObj.position + offset;
            if (time > ShakeTime && !returnpos)
            {
                changeAmountX = Random.RandomRange(-ShakeAmount, ShakeAmount);
                changeAmounty = Random.RandomRange(-ShakeAmount, ShakeAmount);
                time = 0;
                returnpos = true;
            }
            if (returnpos && time < ShakeTime)
            {
                movement.x += Mathf.Lerp(changeAmountX, 0, time / ShakeTime);
                movement.y += Mathf.Lerp(changeAmounty, 0, time / ShakeTime);
            }
            if (returnpos && time > ShakeTime)
            {
                returnpos = false;
                time = 0;
            }
            if (ShakeTime > 0 && ShakeAmount > 0 && !returnpos)
            {
                movement.x += Mathf.Lerp(0, changeAmountX, time/ShakeTime);
                movement.y += Mathf.Lerp(0, changeAmounty, time/ShakeTime);
            }
            transform.position = movement;
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
            rotation = transform.rotation.eulerAngles;
            rotation.x -= y * CamSpeed;
            rotation.y += x * CamSpeed;
            rotation.z = 0;
            quat = Quaternion.Euler(rotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, sensitivity*Time.deltaTime);
            time += Time.deltaTime;
            yield return _fixedUpdate;
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
