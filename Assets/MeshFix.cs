using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFix : MonoBehaviour
{
    public MeshFilter m;


    public MeshFilter Mesh;

    
    private void OnEnable()
    {
        Mesh = m;
    }
}
