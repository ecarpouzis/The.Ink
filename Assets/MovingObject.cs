using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    public bool loopMovement;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(startPosition, endPosition, Mathf.PingPong(GameController.G.currentTimePoint * speed, 1));
    }
}
