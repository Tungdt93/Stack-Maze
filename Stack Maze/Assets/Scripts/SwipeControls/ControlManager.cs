using UnityEngine;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private Swipe swipeControls;
    [SerializeField] private PlayerController player;
    private Vector3 direction;

    private void Start()
    {
        player = GameManager.instance.Player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (swipeControls.Swiped)
        {
            if (!player.DisabledDirections)
            {
                if (swipeControls.SwipeLeft)
                    direction = Vector3.left;
                if (swipeControls.SwipeRight)
                    direction = Vector3.right;
                if (swipeControls.SwipeUp)
                    direction = Vector3.forward;
                if (swipeControls.SwipeDown)
                    direction = Vector3.back;
            }
            else
            {
                if (swipeControls.SwipeDown)
                    direction = Vector3.back;
            }

            if (direction != Vector3.zero)
                player.ChangeDirection(direction);
        }      
    }
}
