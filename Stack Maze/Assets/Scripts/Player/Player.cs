using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private void Awake()
    {

    }

    public void ChangePosition(Vector3 playerNewPosition)
    {
        transform.position = playerNewPosition;
    }
}
