using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private Transform theStack;
    [SerializeField] private List<GameObject> stacks = new List<GameObject>();

    public List<GameObject> Stacks { get => stacks; set => stacks = value; }

    void Awake()
    {
        GetNonStaticStacks();
    }

    private void GetNonStaticStacks()
    {
        foreach (Transform child in theStack)
        {
            if (child.CompareTag("Stack"))
            {
                child.gameObject.AddComponent<Stack>();
                child.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                child.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.95f, 1f, 0.95f);
                stacks.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        
    }
}
