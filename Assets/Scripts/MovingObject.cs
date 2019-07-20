using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public bool loopMovement;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float speed = 1;
    public float startTime = 0f;

    // Update is called once per frame
    void Update()
    {
        float adjustedTime = GameController.G.currentTimePoint - startTime;
        if (adjustedTime > 0)
        {
            if (loopMovement)
            {
                transform.position = Vector2.Lerp(startPosition, endPosition, Mathf.PingPong(adjustedTime * speed, 1));
            }
            else
            {
                transform.position = Vector2.Lerp(startPosition, endPosition, adjustedTime * speed);
            }
        }
    }
}
