using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private int numberOfParts;

    public int NumberOfParts { get => numberOfParts; set => numberOfParts = value; }

    public void GetAllParts()
    {
        foreach (Transform transform in transform)
        {
            numberOfParts++;
        }
    }
}
