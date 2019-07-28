using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatedObject : MonoBehaviour
{
    public GameObject objectEndSpot;
    Vector3 startPosition;
    Vector3 endPosition;
    public float speed = 1;
    public float offsetTime = 0f;
    float prevLoopPoint = 0;
    float curLoopPoint = 0;
    Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        endPosition = objectEndSpot.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = _rigidbody.position;
        endPosition.z = startPosition.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.G.isPlaying) { 
                float adjustedTime = GameController.G.fixedTimePoint + offsetTime;
                curLoopPoint = Mathf.Repeat(adjustedTime * speed, 1);

                 _rigidbody.MovePosition(Vector3.Lerp(startPosition, endPosition, curLoopPoint));

                //Update the previous loop point
                prevLoopPoint = curLoopPoint;
        }
    }
}
