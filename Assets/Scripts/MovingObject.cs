using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public bool startAtOriginalGamespot = false;
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
    Renderer myRender;
    Collider2D myCollider;
    bool isFirstMovementScript = true;
    float nextDelay = 999;
    public int order;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        myRender = GetComponent<Renderer>();
        myCollider = GetComponent<Collider2D>();
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        if (startAtOriginalGamespot)
        {
            if (!useLocalSpace)
            {
                startPosition = transform.position;
            }
            else
            {
                startPosition = transform.localPosition;
            }
        }

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
    
    bool hasFlipped = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        float adjustedTime = GameController.G.fixedTimePoint + offsetTime;

        if (loopMovement)
        {
            Vector2 curScale = transform.localScale;
            curLoopPoint = Mathf.PingPong(adjustedTime * speed, 1);
            Vector2 newPos = Vector2.Lerp(startPosition, endPosition, curLoopPoint);

            bool closeToStartOrEnd = false;

            float closeStart = Mathf.Abs(startPosition.x - newPos.x);
            float closeEnd = Mathf.Abs(endPosition.x - newPos.x);
            //Debug.Log("Timepoint: "+GameController.G.fixedTimePoint+" Fixed Distance:"+Vector2.Distance(startPosition,endPosition)+" Attempted Distance Traveled: "+(adjustedTime * speed));
            //Debug.Log(" Distance Traveled modded by Fixed Distance: " + (adjustedTime * speed) % Vector2.Distance(startPosition, endPosition) + " Prev value modded 2:" + (((adjustedTime * speed) % Vector2.Distance(startPosition, endPosition))%2));
            //One minus the Total Distance traveled, modded by the distance between Start and End, modded by 2.
            float curDir = -1+(((adjustedTime * speed) % Vector2.Distance(startPosition, endPosition)) % 2);
            if (curDir > 0)
            {
                curScale.x = -1;
            }
            else
            {
                curScale.x = 1;
            }
            transform.localScale = curScale;
            _rigidbody.MovePosition(newPos);
        }
        else
        {
            if (delay == 0)
            {
                _rigidbody.MovePosition(Vector2.Lerp(startPosition, endPosition, adjustedTime * speed));
            }
            //If I have a delay:
            else
            {
                if (GameController.G.fixedTimePoint - delay > 0 && GameController.G.fixedTimePoint < nextDelay)
                {
                    myRender.enabled = true;

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
                        _rigidbody.MovePosition(Vector3.Lerp(startPosition, endPosition, (adjustedTime - delay) * speed));
                    }
                }
                else
                {
                    //It is before my delay, and I am the first movement script to trigger.
                    if (isFirstMovementScript && GameController.G.fixedTimePoint - delay < 0)
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
        myRender.enabled = false;
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }
    }

}
