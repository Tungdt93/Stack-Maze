using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetertor : MonoBehaviour
{
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private GameObject stackHolder;

    private Rigidbody rb;
    private Vector3 moveAmount;
    private float yPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yPosition = stackHolder.transform.position.y;
    }
    private void Update()
    {
 
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + moveAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stack"))
        {
            if (collision.gameObject.TryGetComponent<Stack>(out Stack stack))
            {
                stack.DisableObject();
                Vector3 newStackPosition = new Vector3(stackHolder.transform.position.x,
                    yPosition,
                   stackHolder.transform.position.z);

                GameObject newStack = Instantiate(stackPrefab, newStackPosition, Quaternion.identity, stackHolder.transform);
                newStack.transform.localScale *= 2f;
            }
        }
    }
}
