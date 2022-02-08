using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleObject : MonoBehaviour
{
    public void DisableObject()
    {
        this.gameObject.SetActive(false);
    }
}
