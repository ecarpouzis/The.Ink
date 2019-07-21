using UnityEngine;
using System.Collections;
using System;

public class SoundSubClip : MonoBehaviour {
    public AudioSource thisClip;
    float endTime;
    bool countDown = false;

	// Use this for initialization
	void Awake () {
        thisClip = gameObject.GetComponent<AudioSource>();
    }

    public void Play()
    {
        thisClip.Play();
    }

    public void Play(float startTime)
    {
        if (startTime >= 0)
        {
            thisClip.time = startTime;
        }
            thisClip.Play();
        
    }

    public void Play(float startTime, float givenEnd)
    {
        thisClip.time = startTime;
        endTime = givenEnd;
        thisClip.Play();
    }

    public void Stop()
    {
        thisClip.Stop();
    }

    // Update is called once per frame
    void Update () {
        if (endTime > 0)
        {
            countDown = true;
        }
        if (countDown)
        {
            if (thisClip.time > endTime)
            {
                thisClip.Stop();
            }
        }
    }
}
