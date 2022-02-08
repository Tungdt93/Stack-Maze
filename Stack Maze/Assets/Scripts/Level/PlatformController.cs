using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ballHolder;
    [SerializeField] private GameObject firstStack;
    [SerializeField] private Transform theStack;  
    [SerializeField] private List<GameObject> stacks = new List<GameObject>();  

    [SerializeField] private bool firstPlatform;
    [SerializeField] private bool lastPlatform;
   
    private GameObject lastStack;

    public List<GameObject> Stacks { get => stacks; set => stacks = value; }
    public GameObject FirstStack { get => firstStack; set => firstStack = value; }
    public GameObject LastStack { get => lastStack; set => lastStack = value; }
    public GameObject Wall { get => wall; set => wall = value; }
    public GameObject BallHolder { get => ballHolder; set => ballHolder = value; }

    void Awake()
    {
        GetNonStaticStacks();
        //GetFirstAndLastStack();
    }

    private void GetNonStaticStacks()
    {
        foreach (Transform child in theStack)
        {
            if (child.CompareTag("Stack"))
            {
                if (!lastPlatform)
                {
                    child.gameObject.transform.localScale = new Vector3(0.95f, 0.18f, 0.95f);
                    child.gameObject.AddComponent<Stack>();
                    child.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    child.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.6f, 1f, 0.6f);
                    stacks.Add(child.gameObject);
                }
                else
                {
                    Vector3 childPosition = child.transform.position;
                    Vector3 newBulletPosition = new Vector3(childPosition.x,
                        childPosition.y + 0.2f, childPosition.z);
                    Instantiate(bulletPrefab, newBulletPosition, Quaternion.identity, ballHolder.transform);
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                child.gameObject.tag = "StaticStack";               
            }

            if (child.CompareTag("StaticStack"))
            {
                if (lastPlatform)
                {
                    child.gameObject.transform.localScale = new Vector3(0.95f, 0.18f, 0.95f);
                }
            }
        }
  
    }

    private void GetFirstAndLastStack()
    {
        if (firstPlatform)
        {
            int index = 0;
            float minXPosition = stacks[0].transform.position.x;
            float minZPosition = stacks[0].transform.position.z;
            for (int i = 0; i < stacks.Count; i++)
            {
                if (stacks[i].transform.position.x < minXPosition)
                {
                    if (stacks[i].transform.position.z < minZPosition)
                    {
                        minXPosition = stacks[i].transform.position.x;
                        minZPosition = stacks[i].transform.position.z;
                        index = i;
                    }
                }
            }
            firstStack = stacks[index];
        }
    }

    void Update()
    {
        
    }
}
