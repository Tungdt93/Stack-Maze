using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject bridgePrefab;

    private GameObject player;
    private PlatformController platformController;
    [SerializeField] private List<GameObject> stacks = new List<GameObject>();

    public override void Init()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public override void ManagerTask()
    {
        platformController = platform.GetComponent<PlatformController>();
        stacks = platformController.Stacks;
        Vector3 firstStackPosition = platformController.FirstStack.transform.position;
        Vector3 playerPosition = new Vector3(firstStackPosition.x, 
            firstStackPosition.y + (stacks.Last().transform.localScale.y / 2f), 
            firstStackPosition.z);
        player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    }

    void Start()
    {
        Init();
        ManagerTask();
    }

    void Update()
    {
        
    }
}
