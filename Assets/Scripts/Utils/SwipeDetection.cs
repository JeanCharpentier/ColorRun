using System;
using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    IPlayer srvPlayer;
    [SerializeField]
    float minDistance = 0.2f;
    [SerializeField]
    float maxTime = 1f;
    [SerializeField, Range(0,1)]
    float directionThreshold = 0.9f;


    InputsManager inputManager;
    Vector2 startPosition;
    float startTime;
    Vector2 endPosition;
    float endTime;

    void Awake()
    {
        inputManager = InputsManager.Instance;
        srvPlayer = ServicesLocator.GetService<IPlayer>();
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    private void OnDisable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }
    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if(Vector3.Distance(startPosition, endPosition) >= minDistance && (endTime - startTime) <= maxTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }else
        {
            srvPlayer.SwitchColor();
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            srvPlayer.Jump();
        }else if(Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            srvPlayer.VDash();
        }else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.LogWarning("Swipe Left");
        }else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.LogWarning("Swipe Right");
        }
    }
}
