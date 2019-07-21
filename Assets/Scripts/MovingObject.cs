using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public bool invisWhenReachDestination = false;
    public bool useLocalSpace = false;
    public bool flipOnLoop;
    public bool loopMovement;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float delay;
    public float speed = 1;
    public float offsetTime = 0f;
    float prevLoopPoint;
    float curLoopPoint;
    MeshRenderer myMesh;
    Collider2D myCollider;
    bool isFirstMovementScript = true;
    float nextDelay = 999;
    public int order;

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

                    if (myCollider != null)
                    {
                        myCollider.enabled = true;
                    }
                    if (transform.name == "Munch")
                    {
                        //Hardcoded Munch's rotation tree
                        Vector3 newRot = transform.rotation.eulerAngles;
                        switch (order)
                        {
                            case 1:
                                newRot.z = 90;
                                break;
                            case 2:
                                newRot.z = 180;
                                break;
                            case 3:
                                newRot.z = 270;
                                break;
                        }
                        transform.rotation = Quaternion.Euler(newRot);
                    }
                    if (useLocalSpace)
                    {
                        transform.localPosition = Vector3.Lerp(startPosition, endPosition, (adjustedTime - delay) * speed);
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(startPosition, endPosition, (adjustedTime - delay) * speed);
                    }
                }
                else
                {
                    //It is before my delay, and I am the first movement script to trigger.
                    if (isFirstMovementScript && GameController.G.currentTimePoint - delay < 0)
                    {
                        HideMe();

                    }
                }
            }
            if ((adjustedTime - delay) * speed > 1)
            {
                if (invisWhenReachDestination)
                {
                    HideMe();
                }
            }
        }
    }

    void HideMe()
    {
        //If I'm the first movement script, disable my collider and mesh before the delay is up
        myMesh.enabled = false;
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }
    }

}
