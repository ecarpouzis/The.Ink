using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Keyframe
{
    public Vector2 position;
    public Vector2 velocity;

    public Keyframe(Vector2 position, Vector2 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
}

public class TimeController : MonoBehaviour
{
    public GameObject player;
    public ArrayList keyframes;
    public bool isRewinding = false;

    public int keyframe = 5;
    private int frameCounter = 0;
    private int reverseCounter = 0;

    private Vector2 currentPosition;
    private Vector2 previousPosition;
    private Vector2 currentVelocity;
    private Vector2 previousVelocity;


    public bool firstRun = true;

    void Start()
    {
        keyframes = new ArrayList();
    }

    void Update()
    {
        if (!isRewinding)
        {
            firstRun = true;
            if (frameCounter < keyframe)
            {
                frameCounter += 1;
            }
            else
            {
                frameCounter = 0;
                keyframes.Add(new Keyframe(player.transform.position, player.GetComponent<CharacterController2D>().velocity));
            }
        }
        else
        {
            if (reverseCounter > 0)
            {
                reverseCounter -= 1;
            }
            else
            {
                reverseCounter = keyframe;
                RestorePositions();
            }

            if (firstRun)
            {
                firstRun = false;
                RestorePositions();
            }

            float interpolation = (float)reverseCounter / (float)keyframe;
            player.transform.position = Vector2.Lerp(previousPosition, currentPosition, interpolation);
            player.GetComponent<CharacterController2D>().velocity = Vector2.Lerp(previousVelocity, currentVelocity, interpolation);
        }

        //if (keyframes.Count > 128)
        //{
        //    keyframes.RemoveAt(0);
        //}
    }
    
    void RestorePositions()
    {
        int lastIndex = keyframes.Count - 1;
        int secondToLastIndex = keyframes.Count - 2;

        if (secondToLastIndex >= 0)
        {
            currentPosition = (keyframes[lastIndex] as Keyframe).position;
            previousPosition = (keyframes[secondToLastIndex] as Keyframe).position;

            currentVelocity = (keyframes[lastIndex] as Keyframe).velocity;
            previousVelocity = (keyframes[secondToLastIndex] as Keyframe).velocity;

            keyframes.RemoveAt(lastIndex);
        }
    }
}