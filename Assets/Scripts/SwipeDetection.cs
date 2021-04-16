using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeInput;
    public delegate void OnSwipeInput(Vector2 direction);

    private Vector2 _tapPosition;
    private Vector2 _swipeDelta;

    private bool _isSwiping;
    private readonly float _deadZone = 80;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isSwiping = true;
            _tapPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetSwipe();
        }

        SearchDerection();
    }

    private void SearchDerection()
    {
        if (_isSwiping)
        {
            if (Input.GetMouseButton(0))
            {
                _swipeDelta = (Vector2)Input.mousePosition - _tapPosition;
            }
        }

        if (_swipeDelta.magnitude > _deadZone)
        {
            if(Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y))
            {
                SwipeInput?.Invoke(_swipeDelta.x > 0? Vector2.right : Vector2.left);
            }
            else
            {
                SwipeInput?.Invoke(_swipeDelta.y > 0? Vector2.up : Vector2.down);
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        _isSwiping = false;
        _tapPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
