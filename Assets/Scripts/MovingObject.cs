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
    bool isFirstMovementScript = true;
    float nextDelay = 999;
    bool munchRunOnce = false;

    private void Awake()
    {
        myMesh = GetComponent<MeshRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        MovingObject[] movementScripts = GetComponents<MovingObject>();
        float myDelay = delay;
        foreach (MovingObject o in movementScripts)
        {
            if (o.delay < myDelay)
            {
                isFirstMovementScript = false;
            }
            if (o.delay > myDelay && o.delay < nextDelay)
            {
                nextDelay = o.delay;
            }
        }
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
            //If I have a delay:
            else
            {
                if (GameController.G.currentTimePoint - delay > 0 && GameController.G.currentTimePoint < nextDelay)
                {
                    myMesh.enabled = true;
                    myCollider.enabled = true;
                    if (transform.name == "Munch")
                    {
                        //Hardcoded Munch's local movement, z value, and rotation decision tree:
                        Vector3 newPos = Vector2.Lerp(startPosition, endPosition, (adjustedTime - delay) * speed);
                        newPos.z = 9;
                        transform.localPosition = newPos;
                        if (!isFirstMovementScript && !munchRunOnce)
                        {
                            Vector3 newRot = transform.rotation.eulerAngles;
                            newRot.z += 90;
                            transform.rotation = Quaternion.Euler(newRot);
                            munchRunOnce = true;
                        }
                    }
                    else
                    {
                        transform.position = Vector2.Lerp(startPosition, endPosition, (adjustedTime - delay) * speed);
                    }
                }
                else
                {
                    //It is before my delay, and I am the first movement script to trigger.
                    if (isFirstMovementScript && GameController.G.currentTimePoint - delay < 0)
                    {
                        //If I'm the first movement script, disable my collider and mesh before the delay is up
                        myMesh.enabled = false;
                        if (myCollider != null)
                        {
                            myCollider.enabled = false;
                        }

                    }
                }
            }
        }
    }
}
