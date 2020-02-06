using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Vector3List")]
public class VectorListData : ScriptableObject
{
    public List<Vector3> vectors = new List<Vector3>();

    public void SetList(List<Vector3> newVectors)
    {
        vectors = newVectors;
    }

    public void ClearList()
    {
        vectors.Clear();
    }

    public void AddList(Vector3 vector)
    {
        vectors.Add(vector);
    }

    public void AddList(Transform transform)
    {
        vectors.Add(transform.position);
    }

    public void SetList(List<Transform> transforms)
    {
        vectors.Clear();
        foreach (var t in transforms)
        {
            vectors.Add(t.position);
        }
    }
}
