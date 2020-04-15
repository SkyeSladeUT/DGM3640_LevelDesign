using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;
    public Transform WindObj;
    public float Speed;
    private Vector3 rotation;
    private ParticleSystem.RotationBySpeedModule rotSpeed;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(Rotate());
    }


    private IEnumerator Rotate()
    {
        while (true)
        {
            WindObj.rotation = Quaternion.Euler(new Vector3(0,Random.Range(0,360), 0));
            rotSpeed = ps.rotationBySpeed;
            rotSpeed.xMultiplier = WindObj.forward.z * Speed;
            rotSpeed.zMultiplier = WindObj.forward.x * Speed;
            yield return new WaitForSeconds(5);
        }
    }
}
