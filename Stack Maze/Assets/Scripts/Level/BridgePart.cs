using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgePart : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Left,
        Right,   
        Down
    };

    [SerializeField] private Direction direction;
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private bool turnRight;
    [SerializeField] private bool turnLeft;
    [SerializeField] private bool marked = false;

    [SerializeField] private bool passed = false;
    [SerializeField] private bool notGenerated = true;
   
    public bool Passed { get => passed; set => passed = value; }
    public bool NotGenerated { get => notGenerated; set => notGenerated = value; }
    public bool Marked { get => marked; set => marked = value; }

    public Vector3 GetDirection()
    {
        if (passed)
        {
            if (turnRight)
            {
                return direction switch
                {
                    Direction.Up => Vector3.left,
                    Direction.Left => Vector3.back,
                    Direction.Right => Vector3.back,
                    Direction.Down => Vector3.right,
                    _ => Vector3.forward,
                };
            }
            else if (turnLeft)
            {
                return direction switch
                {
                    Direction.Up => Vector3.right,
                    Direction.Left => Vector3.back,
                    Direction.Right => Vector3.back,
                    Direction.Down => Vector3.left,
                    _ => Vector3.forward,
                };
            }
            else
            {
                return direction switch
                {
                    Direction.Up => Vector3.back,
                    Direction.Left => Vector3.right,
                    Direction.Right => Vector3.left,
                    Direction.Down => Vector3.forward,
                    _ => Vector3.forward,
                };
            }
            
        }
        else
        {
            return direction switch
            {
                Direction.Up => Vector3.forward,
                Direction.Left => Vector3.left,
                Direction.Right => Vector3.right,
                Direction.Down => Vector3.back,
                _ => Vector3.forward,
            };
        }    
    }

    public void GenerateStack()
    {
        if (notGenerated)
        {
            GameObject newStack = Instantiate(stackPrefab, transform.position, Quaternion.identity, transform.parent);
            newStack.transform.localScale = new Vector3(0.98f, 0.2f, 0.98f);
            notGenerated = false;
        }     
    }

    public void DisableBridgePart() 
    {
        this.gameObject.SetActive(false);
    }
}
