using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<Transform> OnPlatform = delegate { };
    public event Action<Transform> OnBridge = delegate { };

    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject stackHolder;
    [SerializeField] private GameObject bulletHolder;
    [SerializeField] private float defaultSpeed;
 

    private Rigidbody rb;
    private Vector3 direction;
    private float moveSpeed = 0f;
    private float addition = 0f;
    //private float bulletHolderXPosition = 0f;
    private float bulletHolderYPosition = 0f;
    //private float bulletHolderZPosition = 0f;
    private float yStackHolderStartPosition;
    private int score = -3;
    private int bullet;
    private bool disabledDirections = false;
    private bool isControlling = true;
    private bool isMoving = true;
    private bool goOut = false;
    private bool goBack = true;
    private bool upOneStack = true;

    public GameObject Visual { get => visual; set => visual = value; }
    public Vector3 Direction { get => direction; set => direction = value; }
    public bool DisabledDirections { get => disabledDirections; set => disabledDirections = value; }
    public int Score { get => score; set => score = value; }


    void Start()
    {
        yStackHolderStartPosition = stackHolder.transform.position.y - (stackPrefab.transform.localScale.y / 2f);
        direction = Vector3.zero;        
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (moveSpeed > 0)
        {
            isControlling = false;
        }
        else
        {
            isControlling = true;   
        }
        CheckCollider();
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        if (isControlling)
        {
            isMoving = true;
            moveSpeed = defaultSpeed;
            transform.forward = newDirection;
            direction = newDirection;
        }
    }

    private void CheckCollider()
    {
        if (direction != Vector3.zero)
        {
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            {
                if (!hit.transform.gameObject.CompareTag("Player"))
                {
                    if (hit.transform.CompareTag("StaticStack") || hit.transform.CompareTag("Wall"))
                    {
                        if (isMoving)
                        {
                            isMoving = false;
                            moveSpeed = 0f;
                        }
                    }
                }

                if (hit.transform.CompareTag("BridgePart"))
                {
                    if (stackHolder.transform.childCount == 0)
                    {
                        if (direction == Vector3.forward)
                        {
                            moveSpeed = 0;
                        }
                    }
                }

                if (hit.transform.CompareTag("EndTrigger"))
                {
                    if (stackHolder.transform.childCount == 0)
                    {
                        if (direction == Vector3.forward)
                        {
                            moveSpeed = 0;
                        }
                    }
                }
            }
        }       
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        #region Platform
        if (other.gameObject.CompareTag("Platform"))
        {
            OnPlatform?.Invoke(other.transform);
            if (other.TryGetComponent(out PlatformController platformController))
            {
                platformController.Wall.SetActive(true);
            }
            if (stackHolder.transform.childCount == 0)
            {
                if (goOut)
                {
                    stackHolder.transform.position = new Vector3(stackHolder.transform.position.x,
                        yStackHolderStartPosition,
                        stackHolder.transform.position.z); ;
                    visual.transform.position = stackHolder.transform.position;
                    //goOut = false;
                }
            }
        }
        #endregion

        //#region Wall
        //if (other.gameObject.CompareTag("Wall"))
        //{
        //    if (direction == Vector3.back)
        //    {
        //        Debug.Log("haha");
        //        moveSpeed = 0f;
        //    }

        //    if (direction != Vector3.back)
        //    {
        //        Debug.Log("hihi");
        //        moveSpeed = defaultSpeed;
        //    }
        //}
        //#endregion

        #region Stack
        if (other.gameObject.CompareTag("Stack"))
        {
            if (other.gameObject.TryGetComponent<Stack>(out Stack stack))
            {
                score += 3;
                stack.DisableObject();

                if (stackHolder.transform.childCount == 0)
                {
                    if (!goOut)
                    {
                        Vector3 newVisualPosition = new Vector3(visual.transform.position.x,
                                          visual.transform.position.y + stackPrefab.transform.localScale.y,
                                          visual.transform.position.z);
                        visual.transform.position = newVisualPosition;
                    }
                    else
                    {
                        Vector3 newStackPosition = new Vector3(stackHolder.transform.position.x,
                                     stackHolder.transform.position.y + (stackPrefab.transform.localScale.y / 2f),
                                     stackHolder.transform.position.z);
                        stackHolder.transform.position = newStackPosition;

                        Vector3 newVisualPosition = new Vector3(visual.transform.position.x,
                                          visual.transform.position.y + stackPrefab.transform.localScale.y,
                                          visual.transform.position.z);
                        visual.transform.position = newVisualPosition;
                    }

                    Instantiate(stackPrefab, stackHolder.transform.position, Quaternion.identity, stackHolder.transform);

                }
                else if (stackHolder.transform.childCount > 0)
                {
                    Vector3 newStackPosition = new Vector3(stackHolder.transform.position.x,
                                       stackHolder.transform.position.y + addition,
                                       stackHolder.transform.position.z);
                    Instantiate(stackPrefab, newStackPosition, Quaternion.identity, stackHolder.transform);

                    Vector3 newVisualPosition = new Vector3(visual.transform.position.x,
                                       visual.transform.position.y + (stackPrefab.transform.localScale.y),
                                       visual.transform.position.z);
                    visual.transform.position = newVisualPosition;
                }
                addition += stackPrefab.transform.localScale.y;
            }
        }
        #endregion

        #region Bullet
        if(other.gameObject.CompareTag("Bullet"))
        {
            if (other.TryGetComponent(out Bullet bullet))
            {
                this.bullet++;
                Vector3 newBulletPosition = new Vector3(bulletHolder.transform.position.x,
                    bulletHolder.transform.position.y + bulletHolderYPosition,
                    bulletHolder.transform.position.z);
                Instantiate(bulletPrefab, newBulletPosition, Quaternion.identity, bulletHolder.transform);
                bullet.DisableObject();
            }
            bulletHolderYPosition += 0.1f;
        }
        #endregion

        #region Bridge    
        if (other.gameObject.CompareTag("BridgePart"))
        {
            if (other.gameObject.TryGetComponent(out BridgePart bridgePart))
            {
                if (stackHolder.transform.childCount == 0)
                {
                    if (direction != Vector3.forward)
                    {
                        Vector3 bridgePartPostition = other.gameObject.transform.position;
                        transform.position = new Vector3(bridgePartPostition.x,
                            bridgePartPostition.y + (stackPrefab.transform.localScale.y / 2f),
                            bridgePartPostition.z);
                        upOneStack = true;                      
                    }
                    if (direction == Vector3.forward)
                    {
                        moveSpeed = 0f;
                        if (!goBack)
                        {
                            visual.transform.position = new Vector3(visual.transform.position.x,
                                visual.transform.position.y - stackPrefab.transform.localScale.y,
                                visual.transform.position.z);
                        }
                    }
                }
                else if (stackHolder.transform.childCount > 0)
                { 
                    OnBridge?.Invoke(visual.transform);
                    if (bridgePart.NotGenerated)
                    {
                        int lastStackIndex = stackHolder.transform.childCount - 1;

                        if (lastStackIndex >= 0)
                        {
                            if (stackHolder.transform.childCount == 1 || stackHolder.transform.childCount == 0)
                            {
                                Destroy(stackHolder.transform.GetChild(lastStackIndex).gameObject);

                                Vector3 bridgePartPostition = other.gameObject.transform.position;
                                transform.position = new Vector3(bridgePartPostition.x,
                                    bridgePartPostition.y + (stackPrefab.transform.localScale.y / 2f),
                                    bridgePartPostition.z);
                            }
                            else if (stackHolder.transform.childCount > 1)
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
                            }
                        }
                        if (addition < 0)
                        {
                            addition = 0f;
                        }
                        else
                        {
                            addition -= stackPrefab.transform.localScale.y;
                        }
                        bridgePart.GenerateStack();
                        goOut = true;
                    }   
                    else
                    {
                        Vector3 bridgePartPostition = other.gameObject.transform.position;
                        transform.position = new Vector3(bridgePartPostition.x,
                            bridgePartPostition.y + (stackPrefab.transform.localScale.y / 2f),
                            bridgePartPostition.z);

                        if (upOneStack)
                        {
                            stackHolder.transform.position = new Vector3(stackHolder.transform.position.x,
                                stackHolder.transform.position.y + stackPrefab.transform.localScale.y,
                                stackHolder.transform.position.z);

                            visual.transform.position = new Vector3(visual.transform.position.x,
                                visual.transform.position.y + stackPrefab.transform.localScale.y,
                                visual.transform.position.z);
                            upOneStack = false;
                            goBack = false;
                        }
                    }
                }
                direction = bridgePart.GetDirection();
                transform.forward = bridgePart.GetDirection();
            }            
        }       
        #endregion

        #region CheckPoint
        if (other.gameObject.CompareTag("CheckPoint"))
        {      
            ResetProperties();
            StartCoroutine(CheckPoint());
        }
        #endregion

        #region FinishLine
        if (other.gameObject.CompareTag("FinishLine"))
        {
            moveSpeed = 0f;
            GameManager.instance.LevelCompleted = true;
        }
        #endregion    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            disabledDirections = false;           
        }
        else
        {
            disabledDirections = true;
        }

        if (other.gameObject.CompareTag("BridgePart"))
        {
            if (other.TryGetComponent(out BridgePart bridgePart))
            {
                if (stackHolder.transform.childCount == 0)
                {
                    if (direction == Vector3.forward)
                    {
                        bridgePart.Marked = true;
                    }
                }
                else if (stackHolder.transform.childCount > 0)
                {
                    if (direction == Vector3.forward)
                    {
                        bridgePart.Marked = false;
                    }
                }
            }
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BridgePart"))
        {
            if (other.gameObject.TryGetComponent(out BridgePart bridgePart))
            {
                if (!bridgePart.Marked)
                {
                    bridgePart.Passed = !bridgePart.Passed;
                }
            }
        }

        if (other.gameObject.CompareTag("CheckPoint"))
        {
            Vector3 visualPosition = new Vector3(stackHolder.transform.position.x,
            stackHolder.transform.position.y - (stackPrefab.transform.localScale.y / 2f),
            stackHolder.transform.position.z);
            visual.transform.position = visualPosition;
        }
    }

    private void ResetProperties()
    {
        if (stackHolder.transform.childCount > 0)
        {
            foreach (Transform transform in stackHolder.transform)
            {
                Destroy(transform.gameObject);
            }
        }

        Vector3 visualPosition = new Vector3(stackHolder.transform.position.x,
            stackHolder.transform.position.y + (stackPrefab.transform.localScale.y / 2f),
            stackHolder.transform.position.z);
        visual.transform.position = visualPosition;
        goBack = true;
        upOneStack = true;
        goOut = false;
        addition = 0f;
        moveSpeed = 0f; 
    }

    IEnumerator CheckPoint()
    {
        yield return new WaitForSeconds(1f);
        moveSpeed = defaultSpeed;
    }
}
