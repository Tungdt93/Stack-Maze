using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetertor : MonoBehaviour
{
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private GameObject stackHolder;

    private StackHolder stacks;

    private Rigidbody rb;

    private Vector3 playerInput;
    private Vector3 moveAmount;

    private float startYPosition;
    private float yPosition;
    private bool firstStack = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stacks = stackHolder.GetComponent<StackHolder>();
        startYPosition = transform.position.y;
        yPosition = stackHolder.transform.position.y;
    }
    private void Update()
    {
        playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
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
                stack.DisableStack();
                Vector3 newStackPosition = new Vector3(stackHolder.transform.position.x,
                    yPosition,
                   stackHolder.transform.position.z);

                GameObject newStack = Instantiate(stackPrefab, newStackPosition, Quaternion.identity, stackHolder.transform);
                newStack.transform.localScale *= 2f;
            }

        }
    }
}
