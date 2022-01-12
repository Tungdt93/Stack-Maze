using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdgePart : MonoBehaviour
{
    [SerializeField] private GameObject stackPrefab;

    public void GenerateStack()
    {
        GameObject newStack = Instantiate(stackPrefab, transform.position, Quaternion.identity, transform.parent);
        newStack.transform.localScale = Vector3.one;
    }

    public void DisableBirdgePart() 
    {
        this.gameObject.SetActive(false);
    }
}
