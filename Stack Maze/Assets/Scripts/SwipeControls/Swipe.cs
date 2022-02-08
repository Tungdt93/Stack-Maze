using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [SerializeField] private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;

    private Vector2 startTouch, swipeDelta;
    private bool isDraging, swiped;

    public bool SwipeLeft { get => swipeLeft; set => swipeLeft = value; }
    public bool SwipeRight { get => swipeRight; set => swipeRight = value; }
    public bool SwipeUp { get => swipeUp; set => swipeUp = value; }
    public bool SwipeDown { get => swipeDown; set => swipeDown = value; }
    public bool Swiped { get => swiped; set => swiped = value; }

    private void Start()
    {
        swiped = false;
    }

    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetToDefault();
        }
        #endregion

        #region Mobile Inputs
        if(Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                ResetToDefault();
            }
        }
        #endregion

        swipeDelta = Vector2.zero;

        if (isDraging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }    
        }

        if (swipeDelta.magnitude > 125f)
        {
            swiped = true;
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0)
                    swipeLeft = true;              
                else
                    swipeRight = true;
            }
            else
            {
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
        }
    }

    private void ResetToDefault()
    {
        swiped = false;
        isDraging = false;
        startTouch = swipeDelta = Vector2.zero;
    }
}
