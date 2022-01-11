using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject playerPrefab;

    private List<GameObject> stacks = new List<GameObject>();
    private GameObject player;

    public override void Init()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public override void ManagerTask()
    {
        stacks = platform.GetComponent<PlatformController>().Stacks;
        Vector3 lastStackPosition = stacks.Last().transform.position;
        Vector3 playerPosition = new Vector3(lastStackPosition.x, lastStackPosition.y + (stacks.Last().transform.localScale.y / 2), lastStackPosition.z);
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
