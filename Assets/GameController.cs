using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    float timeToStart = 1f;
    float timeSinceStart = 0f;
    public CameraController CameraController;
    public CharacterController2D CharacterController;
    TimeController characterTimeController;
    bool isRewinding = false;
    bool isPlaying = false;
    float timePlaying = 0f;
    float timeRewinding = 0f;
    public float currentTimePoint = 0f;
    public UnityEngine.UI.Text TimeLeft;

    // Start is called before the first frame update
    void Start()
    {
        characterTimeController = CharacterController.GetComponent<TimeController>();
        CharacterController.isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        if (timeSinceStart > timeToStart)
        {
            StartPlaying();
        }

        if (isPlaying)
        {
            if (isRewinding)
            {
                timeRewinding += Time.deltaTime;
            }
            else
            {
                timePlaying += Time.deltaTime;
            }

            currentTimePoint = timePlaying - timeRewinding;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton5) || Input.GetKey(KeyCode.JoystickButton2))
            {
                isRewinding = true;
                CameraController.isRewinding = true;
                characterTimeController.isRewinding = true;
            }
            else
            {
                isRewinding = false;
                CameraController.isRewinding = false;
                characterTimeController.isRewinding = false;
            }
            TimeLeft.text = secondsLeft().ToString();
        }
    }

    int maxTime = 60;
    int secondsLeft()
    {
        
        return maxTime - (int)currentTimePoint;
    }

    void StartPlaying()
    {
        isPlaying = true;
        CameraController.isRunning = true;
        CharacterController.isRunning = true;
    }
}
