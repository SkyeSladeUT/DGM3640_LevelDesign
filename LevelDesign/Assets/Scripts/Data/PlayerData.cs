using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName="Data/Player")]
public class PlayerData : ScriptableObject
{
    public bool hasBrick;
    public Rigidbody brickRB;

    public void grabBrick(GameObject brick)
    {
        brickRB = brick.GetComponent<Rigidbody>();
        hasBrick = true;
    }

    public void throwBrick()
    {
        brickRB = null;
        hasBrick = false;
    }
    
}
