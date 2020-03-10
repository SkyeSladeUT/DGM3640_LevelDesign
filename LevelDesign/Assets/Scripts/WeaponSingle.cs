using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WeaponSingle : MonoBehaviour
{
    public GameObject weaponobj;
    public Transform weaponSpawnPoint;
    public PlayerData player;
    public Transform parent;
    private bool inTrigger;
    public UnityEvent OnGrab;

    private void Start()
    {
        player.hasBrick = false;
        player.brickRB = null;
    }

    public void GetWeapon()
    {
        weaponobj.transform.SetParent(parent);
        weaponobj.transform.position = weaponSpawnPoint.transform.position;
        weaponobj.transform.rotation = weaponSpawnPoint.transform.rotation;
        player.grabBrick(weaponobj.transform.gameObject);
        OnGrab.Invoke();
    }

    private IEnumerator GetBrick()
    {
        Debug.Log("get Brick");
        while (inTrigger)
        {
            if (!player.hasBrick && Input.GetMouseButtonUp(0))
            {
                GetWeapon();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            StartCoroutine(GetBrick());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            StopCoroutine(GetBrick());
        }
    }
}
