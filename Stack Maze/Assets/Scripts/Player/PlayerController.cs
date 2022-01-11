using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject stackHolder;

    private Rigidbody rb;

    private Vector3 playerInput;
    private Vector3 moveAmount;

    private float addition = 0f;
    private bool firstStack = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveAmount = playerInput.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + moveAmount);

    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stack"))
        {
            if (other.gameObject.TryGetComponent<Stack>(out Stack stack))
            {
                stack.DisableStack();
                if (firstStack)
                {
                    GameObject newStack = Instantiate(stackPrefab, stackHolder.transform.position, Quaternion.identity, stackHolder.transform);                    
                }
                else
                {
                    Debug.Log(addition / 2);
                    Vector3 newStackPosition = new Vector3(stackHolder.transform.position.x,
                                       stackHolder.transform.position.y + addition,
                                       stackHolder.transform.position.z);
                    GameObject newStack = Instantiate(stackPrefab, newStackPosition, Quaternion.identity, stackHolder.transform);
                    Debug.Log(visual.transform.position);
                    Vector3 newVisualPosition = new Vector3(visual.transform.position.x,
                                       visual.transform.position.y + (stackPrefab.transform.localScale.y),
                                       visual.transform.position.z);
                    visual.transform.position = newVisualPosition;

                    Debug.Log(visual.transform.position);
                }
                firstStack = false;
                addition += stackPrefab.transform.localScale.y;
            }
        }
    }
}
