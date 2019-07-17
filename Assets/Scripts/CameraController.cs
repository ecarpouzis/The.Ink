using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float cameraSpeed = 4f;

    private Vector2 velocity;
    public bool isRunning = false;
    public bool isRewinding = false;
    private Vector2 cameraStartPos;

    // Start is called before the first frame update
    void Start()
    {
        cameraStartPos = transform.position;
        velocity.y = cameraSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            transform.Translate(velocity * Time.deltaTime * Vector2.up, Space.World);
            if (isRewinding && transform.position.y >= cameraStartPos.y)
            {
                transform.Translate(Vector2.down * velocity * Time.deltaTime * 2, Space.World);
            }
        }
    }
}
