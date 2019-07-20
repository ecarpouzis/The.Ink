using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position;
        endPosition.y += 250;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.G.isPlaying)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, GameController.G.percThroughTime);
        }
    }
}
