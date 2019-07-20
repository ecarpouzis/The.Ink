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
    public static GameController G;
    public bool isRewinding = false;
    public bool isPlaying = false;
    float timePlaying = 0f;
    float timeRewinding = 0f;
    public float currentTimePoint = 0f;
    public UnityEngine.UI.Text TimeLeft;
    public float percThroughTime;
    
    private void Awake()
    {
        characterTimeController = CharacterController.GetComponent<TimeController>();
        G = this;
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
            percThroughTime = ((currentTimePoint - 0) / (maxGameTime - 0));

            if (Input.GetButton("Rewind")
                && timeRewinding <= timePlaying)
            {
                isRewinding = true;
            }
            else
            {
                isRewinding = false;
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
    }
}
