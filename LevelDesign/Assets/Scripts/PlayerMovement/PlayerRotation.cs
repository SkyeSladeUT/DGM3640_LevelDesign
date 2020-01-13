using System.Collections;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public Transform rotFollow;
    private Vector3 rotation;
    private Quaternion quat;
    private bool canRotate = true;

    private void Start()
    {
        rotation = transform.rotation.eulerAngles;
        rotation.y = rotFollow.rotation.eulerAngles.y;
        quat = Quaternion.Euler(rotation);
        transform.rotation = quat;
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (canRotate)
        {
            rotation = transform.rotation.eulerAngles;
            rotation.y = rotFollow.rotation.eulerAngles.y;
            quat = Quaternion.Euler(rotation);
            transform.rotation = quat;
            yield return new WaitForFixedUpdate();
        }
    }
}
