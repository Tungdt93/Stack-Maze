using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform target;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Vector3 offset;
    [SerializeField] [Range(0.01f, 1f)] private float smoothSpeed = 0.125f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 defaultOffset = new Vector3(0.2f, 24f, -8f);

    private void Start()
    {
        player = GameManager.instance.Player;
        playerController = player.GetComponent<PlayerController>();
        target = playerController.Visual.transform;
        platforms = GameManager.instance.Platforms;        
        playerController.OnPlatform += PlayerController_OnPlatform;
        playerController.OnBridge += PlayerController_OnBridge;
    }

    private void PlayerController_OnBridge(Transform visual)
    {
        this.target = visual;
    }

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
        playerController.OnPlatform -= PlayerController_OnPlatform;
        playerController.OnBridge -= PlayerController_OnBridge;
    }

    private void PlayerController_OnPlatform(Transform platform)
    {
        this.target = platform;
        //offset = defaultOffset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
}
