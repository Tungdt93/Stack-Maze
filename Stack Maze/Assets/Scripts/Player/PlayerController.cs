using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnReachedEndTrigger = delegate {};

    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject stackHolder;

    private Rigidbody rb;
    [SerializeField] private List<GameObject> playerStacks = new List<GameObject>();

    private Vector3 playerInput;
    private Vector3 moveAmount;

    private int totalStacks;
    private float addition = 0f;
    private bool firstStack = true;
    private bool isControlling = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isControlling)
        {
            playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveAmount = playerInput.normalized * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + moveAmount);
        }
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stack"))
        {
            totalStacks += 1;
            if (other.gameObject.TryGetComponent<Stack>(out Stack stack))
            {
                stack.DisableStack();
                if (firstStack)
                {
                    GameObject newStack = Instantiate(stackPrefab, stackHolder.transform.position, Quaternion.identity, stackHolder.transform);                                      
                }
                else
                {
                    Vector3 newStackPosition = new Vector3(stackHolder.transform.position.x,
                                       stackHolder.transform.position.y + addition,
                                       stackHolder.transform.position.z);
                    GameObject newStack = Instantiate(stackPrefab, newStackPosition, Quaternion.identity, stackHolder.transform);

                    Vector3 newVisualPosition = new Vector3(visual.transform.position.x,
                                       visual.transform.position.y + (stackPrefab.transform.localScale.y),
                                       visual.transform.position.z);
                    visual.transform.position = newVisualPosition;
                }
                firstStack = false;
                addition += stackPrefab.transform.localScale.y;
            }
        }

        if (other.gameObject.CompareTag("Bridge"))
        {
            if (other.gameObject.TryGetComponent(out BirdgePart birdgePart))
            {
                int lastStackIndex = stackHolder.transform.childCount - 1;
                if (lastStackIndex >= 0)
                {
                    Destroy(stackHolder.transform.GetChild(lastStackIndex).gameObject);

                    Vector3 bridgePartPostition = other.gameObject.transform.position;
                    transform.position = new Vector3(bridgePartPostition.x,
                        bridgePartPostition.y + (stackPrefab.transform.localScale.y / 2f),
                        bridgePartPostition.z);

                    Vector3 newVisualPosition = new Vector3(visual.transform.position.x,
                                visual.transform.position.y - (stackPrefab.transform.localScale.y),
                                visual.transform.position.z);
                    visual.transform.position = newVisualPosition;
                    
                    birdgePart.GenerateStack(); 
                    birdgePart.DisableBirdgePart();
                }         
            }         
        }
        if (other.gameObject.CompareTag("EndTrigger"))
        {
            rb.isKinematic = true;
        }
    }
}
