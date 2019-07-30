using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeToStart = 1f;
    public float timeSinceStart = 0f;
    int maxGameTime = 60;
    public CameraController CameraController;
    public NewCharacterController CharacterController;
    TimeController characterTimeController;
    public static GameController G;
    public bool isRewinding = false;
    bool prevRewindState = false;
    public bool isPlaying = false;
    public float timePlaying = 0f;
    public float timeRewinding = 0f;
    public float currentTimePoint = 0f;
    public UnityEngine.UI.Text TimeLeft;
    public UnityEngine.UI.Text WhiteTimeLeft;
    public float percThroughTime;
    public static float curGametime = 0f;

    public float timeSinceRewind = 0f;
    float minTimeBetweenRewinds = .25f;
    public bool isDeathPaused = false;
    public bool isPaused = false;

    float fixedTime = 0f;
    float fixedRewindTime = 0f;
    public float fixedTimePoint { get { return fixedTime - fixedRewindTime; } }

    public void ResetTimes()
    {
        timePlaying = 0f;
        fixedTime = 0f;
        fixedRewindTime = 0f;
        timeRewinding = 0f;
        currentTimePoint = 0f;
        timeSinceRewind = 0f;
    }
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

    private void FixedUpdate()
    {
        if (isPlaying && !isDeathPaused && !isPaused)
        {
            if (!isRewinding)
            {
                fixedTime += Time.fixedDeltaTime;
            }
            else
            {
                fixedRewindTime += Time.fixedDeltaTime;
            }
        }

        percThroughTime = ((fixedTimePoint - 0) / (maxGameTime - 0));
    }

    public Camera mainCam;
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


        if (timeSinceStart > timeToStart && !isPlaying && !isDeathPaused && mainCam.enabled)
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

                    TimeLeft.text = GetSecondsLeftAsString();
                    WhiteTimeLeft.text = GetSecondsLeftAsString();
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
        GameController.G.isRewinding = false;
        if (isPaused)
        {
            MusicController.m.StopMusic();
        }
        else
        {
            MusicController.m.PlayFromCurpoint();
        }
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
        return maxGameTime - (int)fixedTimePoint;
    }
    string GetSecondsLeftAsString()
    {
        return (maxGameTime - (int)fixedTimePoint).ToString();
    }

    public void StartPlaying()
    {
        MusicController.m.StartForwardMusic();
        isPlaying = true;
    }
}
