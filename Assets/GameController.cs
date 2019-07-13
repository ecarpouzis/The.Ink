using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    float timeToStart = 1f;
    float timeSinceStart = 0f;
    public CameraController CameraController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        if(timeSinceStart > timeToStart)
        {
            CameraController.isRunning = true;
        }
    }
}
