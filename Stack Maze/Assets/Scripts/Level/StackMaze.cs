using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMaze : MonoBehaviour
{
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private float numberOfRows;
    [SerializeField] private float numberOfColumns;
    [SerializeField] private float spacing;

    private float newXPosition;
    private float yPosition;
    private float newZPosition;


    void Start()
    {
        GenerateStacks();  
    }

    private void GenerateStacks()
    {
        newXPosition = transform.position.x;
        newZPosition = transform.position.z;
        yPosition = transform.position.y;

        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                Vector3 newPosition = new Vector3(newXPosition, yPosition + 0.1f, newZPosition);
                GameObject newStack = Instantiate(stackPrefab, newPosition, Quaternion.identity);
                newZPosition += spacing;
            }
            newZPosition = transform.position.z;
            newXPosition += spacing;
        }
    }

    void Update()
    {
        
    }
}
