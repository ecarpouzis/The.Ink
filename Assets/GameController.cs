using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    float timeToStart = 1f;
    float timeSinceStart = 0f;
    int maxGameTime = 60;
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

            //Total time elapsed since game start
            currentTimePoint = timePlaying - timeRewinding;

            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton5) || Input.GetKey(KeyCode.JoystickButton2))
                && timeRewinding <= timePlaying)
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

            TimeLeft.text = GetSecondsLeftAsString();
        }
    }

    int GetSecondsLeft()
    {
        return maxGameTime - (int)currentTimePoint;
    }
    string GetSecondsLeftAsString()
    {
        return (maxGameTime - (int)currentTimePoint).ToString();
    }



    void StartPlaying()
    {
        isPlaying = true;
        CameraController.isRunning = true;
        CharacterController.isRunning = true;
    }
}
