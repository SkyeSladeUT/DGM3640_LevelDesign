using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPile : MonoBehaviour
{
    public GameObject weaponPrefab;
    public Transform weaponSpawnPoint;
    private GameObject weaponInstance;
    public PlayerData player;
    public Transform parent;
    private bool inTrigger;
    public UnityEvent OnGrab;

    public void GetWeapon()
    {
        weaponInstance = Instantiate(weaponPrefab);
        weaponInstance.transform.SetParent(parent);
        weaponInstance.transform.position = weaponSpawnPoint.transform.position;
        weaponInstance.transform.rotation = weaponSpawnPoint.transform.rotation;
        player.grabBrick(weaponInstance);
        OnGrab.Invoke();
    }

    private IEnumerator GetBrick()
    {
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
