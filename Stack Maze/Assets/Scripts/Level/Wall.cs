using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private BoxCollider col;

    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    public void ActiveWall()
    {
        col.isTrigger = false;
    }
}
