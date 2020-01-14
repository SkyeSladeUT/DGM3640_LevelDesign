using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName="Data/Bool")]
public class BoolData : ScriptableObject
{
    public bool value;

    public void Swap()
    {
        value = !value;
    }

    public void Set(bool val)
    {
        value = val;
    }
}
