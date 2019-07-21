using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public bool flipOnLoop;
    public bool loopMovement;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float delay;
    public float speed = 1;
    public float offsetTime = 0f;
    float prevLoopPoint;
    float curLoopPoint;

    // Update is called once per frame
    void Update()
    {
        float adjustedTime = GameController.G.currentTimePoint - offsetTime;

        prevLoopPoint = curLoopPoint;
        curLoopPoint = Mathf.PingPong(adjustedTime * speed, 1);

        if (flipOnLoop)
        {
            Vector2 curDir = transform.localScale;
            if (prevLoopPoint > curLoopPoint)
            {
                curDir.x = -1;
            }
            else
            {
                curDir.x = 1;
            }
            transform.localScale = curDir;
        }

        if (loopMovement)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, curLoopPoint);
        }
        else
        {
            if (delay != 0)
            {
                transform.position = Vector2.Lerp(startPosition, endPosition, adjustedTime * speed);
            }
            else
            {
                if (adjustedTime - delay > 0)
                {
                    transform.position = Vector2.Lerp(startPosition, endPosition, adjustedTime - delay * speed);
                }
            }
        }
    }
}
