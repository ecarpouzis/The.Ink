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
    MeshRenderer myMesh;
    Collider2D myCollider;

    private void Awake()
    {
        myMesh = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float adjustedTime = GameController.G.currentTimePoint + offsetTime;

        prevLoopPoint = curLoopPoint;
        curLoopPoint = Mathf.PingPong(adjustedTime * speed, 1);


        if (loopMovement)
        {
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
            transform.position = Vector2.Lerp(startPosition, endPosition, curLoopPoint);
        }
        else
        {
                if (delay == 0)
                {
                    transform.position = Vector2.Lerp(startPosition, endPosition, adjustedTime * speed);
                }
                else
                {
                    if (GameController.G.currentTimePoint - delay > 0)
                    {
                        myMesh.enabled = true;
                        myCollider.enabled = true;
                        transform.position = Vector2.Lerp(startPosition, endPosition, adjustedTime - delay * speed);
                    }
                    else
                    {
                        myMesh.enabled = false;
                        myCollider.enabled = false;
                    }
                }
            }
    }
}
