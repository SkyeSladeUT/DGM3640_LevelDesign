using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Obj : MonoBehaviour
{
    public Transform followObj;
    private Vector3 pos, offset, rotation;
    private WaitForFixedUpdate _fixedUpdate;
    private bool following;

    private void Start()
    {
        offset = transform.position - followObj.position;
        _fixedUpdate = new WaitForFixedUpdate();
        following = true;
        StartCoroutine(Follow());
    }

    private IEnumerator Follow()
    {
        while (following)
        {
            pos = followObj.position + offset;
            transform.position = pos;
            rotation = transform.rotation.eulerAngles;
            rotation.y = followObj.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(rotation);
            yield return _fixedUpdate;
        }
    }
}
