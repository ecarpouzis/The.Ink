using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeToStart = 1f;
    public float timeSinceStart = 0f;
    int maxGameTime = 60;
    public CameraController CameraController;
    public CharacterController2D CharacterController;
    TimeController characterTimeController;
    public static GameController G;
    public bool isRewinding = false;
    bool prevRewindState = false;
    public bool isPlaying = false;
    public float timePlaying = 0f;
    public float timeRewinding = 0f;
    public float currentTimePoint = 0f;
    public UnityEngine.UI.Text TimeLeft;
    public float percThroughTime;

    public float timeSinceRewind = 0f;
    float minTimeBetweenRewinds = .25f;
    public bool isDeathPaused = false;
    public bool isPaused = false;

    private void Awake()
    {
        characterTimeController = CharacterController.GetComponent<TimeController>();
        G = this;
    }

    public void DeathPause()
    {
        isDeathPaused = true;
        isPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        timeSinceRewind += Time.deltaTime;
        timeSinceStart += Time.deltaTime;


        if (timeSinceStart > timeToStart && !isPlaying && !isDeathPaused)
        {
            StartPlaying();
        }
        if (!isPaused)
        {
            if (Input.GetButtonDown("Menu"))
            {
                Pause();
            }
            else
            {
                if (isPlaying && !isDeathPaused)
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

                    TimeLeft.text = GetSecondsLeftAsString();
                }

                RewindButtonCheck();
                if (isRewinding && !prevRewindState)
                {
                    OnRewindStart();
                }
                else if (!isRewinding && prevRewindState)
                {
                    OnRewindStop();
                }

                prevRewindState = isRewinding;
            }
        }
        else
        {
            if (Input.GetButtonDown("Menu"))
            {
                Pause();
            }
        }
    }

    public GameObject pauseMenu;
    public void Pause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
    }

    void RewindButtonCheck()
    {
        if (GameController.G.currentTimePoint > 0)
        {
            if (Input.GetButton("Rewind") && timeSinceRewind > minTimeBetweenRewinds)
            {
                isRewinding = true;
            }
            if (Input.GetButtonUp("Rewind"))
            {
                timeSinceRewind = 0;
                isRewinding = false;
            }
        }
        else
        {
            timeSinceRewind = 0;
            isRewinding = false;
        }
    }

    void OnRewindStart()
    {
        isDeathPaused = false;
        MusicController.m.StartRewoundMusic();
    }

    void OnRewindStop()
    {
        MusicController.m.StartForwardMusic();
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
        MusicController.m.StartForwardMusic();
        isPlaying = true;
    }
}
