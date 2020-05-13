using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class IncreaseCold : MonoBehaviour
{
    public float IncreaseSpeed;
    public float TotalAmount;
    public float StartChangeAmount;
    public Image FrostImage;
    public Vector3 origScale, finalScale;//, currentscale;
    public Color origColor, finalColor;
    private float total, percent, shakepercent;
    private bool increase, canDecrease;
    private WaitForSeconds waittime, deathTimeWait;
    public CameraAnimation camAnim;
    public GameObject menu;
    public CameraMovement camShake;
    public float startShakeAmount;
    public float MaxCamShake;

    private void Awake()
    {
        waittime = new WaitForSeconds(.1f);
        deathTimeWait = new WaitForSeconds(1f);
    }

    public void Initialize()
    {
        total = 0;
        FrostImage.rectTransform.localScale = origScale;
        FrostImage.color = origColor;
        increase = true;
        canDecrease = true;
        StartCoroutine(ColdChange());
    }

    public void SetDecrease(bool value)
    {
        canDecrease = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            increase = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            increase = true;
        }
    }

    IEnumerator ColdChange()
    {
        while (canDecrease)
        {
            yield return waittime;
            if (increase)
            {
                total += IncreaseSpeed * .1f;
            }
            else
            {
                total -= IncreaseSpeed * .1f*10;
                if (total <= 0)
                {
                    total = 0;
                }
            }
            if (total >= StartChangeAmount)
            {
                percent = (total - StartChangeAmount) / (TotalAmount - StartChangeAmount);
                FrostImage.color = Color.Lerp(origColor, finalColor, percent);
                FrostImage.rectTransform.localScale = Vector3.Lerp(origScale, finalScale, percent);
                //currentscale = Vector3.Lerp(origScale, finalScale, percent);
            }

            if (total >= startShakeAmount)
            {
                shakepercent = (total - startShakeAmount) / (TotalAmount - startShakeAmount);
                camShake.SetShake(shakepercent*MaxCamShake, .05f);
            }
            if (total >= TotalAmount)
            {
                camAnim.Death();
                yield return deathTimeWait;
                Time.timeScale = 0;
                menu.SetActive(true);
            }
        }
    }
}
