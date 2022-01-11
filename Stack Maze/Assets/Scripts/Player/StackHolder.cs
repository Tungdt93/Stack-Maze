using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackHolder : MonoBehaviour
{
    public void ChangePosition(Vector3 playerNewPosition)
    {
        transform.position = playerNewPosition;
    }
}
