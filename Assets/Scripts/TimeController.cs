using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionKeyframe
{
    public Vector2 position;
    public Vector2 velocity;

    public PositionKeyframe(Vector2 position, Vector2 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
}

public class TimeController : MonoBehaviour
{
    public CharacterController2D player;
    public ArrayList keyframes;

    public int keyframe = 5;
    private int frameCounter = 0;
    private int reverseCounter = 0;

    private Vector2 currentPosition;
    private Vector2 previousPosition;
    private Vector2 currentVelocity;
    private Vector2 previousVelocity;


    public bool firstRun = true;

    void Awake()
    {
        player = GetComponent<CharacterController2D>();
    }

    void Start()
    {
        keyframes = new ArrayList();
    }

    void FixedUpdate()
    {
        if (GameController.G.isPlaying)
        {
            if (!GameController.G.isRewinding)
            {
                firstRun = true;
                
                    if (frameCounter < keyframe)
                    {
                        frameCounter += 1;
                    }
                    else
                    {
                        frameCounter = 0;
                        keyframes.Add(new PositionKeyframe(player.transform.position, player.velocity));
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
                player.velocity = Vector2.Lerp(previousVelocity, currentVelocity, interpolation);
            }
        }
    }

    void RestorePositions()
    {
        int lastIndex = keyframes.Count - 1;
        int secondToLastIndex = keyframes.Count - 2;

        if (secondToLastIndex >= 0)
        {
            currentPosition = (keyframes[lastIndex] as PositionKeyframe).position;
            previousPosition = (keyframes[secondToLastIndex] as PositionKeyframe).position;

            currentVelocity = (keyframes[lastIndex] as PositionKeyframe).velocity;
            previousVelocity = (keyframes[secondToLastIndex] as PositionKeyframe).velocity;

            keyframes.RemoveAt(lastIndex);
        }
    }
}