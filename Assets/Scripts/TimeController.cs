using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeController : MonoBehaviour
{
    NewCharacterController player;

    public ArrayList keyframes;
    
    private Vector2 currentPosition;
    private Vector2 currentVelocity;
    private Vector2 currentScale;
    private string currentAnimation;

    public bool firstRun = true;

    public void Reset()
    {
        keyframes = new ArrayList();
    }

    void Awake()
    {
        player = GetComponent<NewCharacterController>();
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
                keyframes.Add(new PositionKeyframe(player.transform.position, player._rigidbody.velocity, player.transform.localScale, player.skeletonAnimation.AnimationName, GameController.G.fixedTimePoint));
            }
            else
            {
                RestorePositions();
                player.transform.position = currentPosition;
                player._rigidbody.velocity = currentVelocity;
                player.transform.localScale = currentScale;
                if(player.skeletonAnimation.AnimationName != currentAnimation)
                {
                    player.skeletonAnimation.AnimationName = currentAnimation;
                }
            }
        }
    }

    void RestorePositions()
    {
        int lastIndex = keyframes.Count - 1;
        int secondToLastIndex = keyframes.Count - 2;
        float nextFrameTime = (keyframes[lastIndex] as PositionKeyframe).frameTime;
        if (secondToLastIndex >= 0)
        {
            while((keyframes[lastIndex] as PositionKeyframe).frameTime > GameController.G.fixedTimePoint)
            {
                keyframes.RemoveAt(lastIndex);
                lastIndex = keyframes.Count - 1;
            }
            currentPosition = (keyframes[lastIndex] as PositionKeyframe).position;
            if (player.IsGrounded())
            {
                currentVelocity = Vector2.zero;
            }
            else
            {
                currentVelocity = (keyframes[lastIndex] as PositionKeyframe).velocity;
            }
            currentScale = (keyframes[lastIndex] as PositionKeyframe).localScale;
            currentAnimation = (keyframes[lastIndex] as PositionKeyframe).animation;
           keyframes.RemoveAt(lastIndex);
        }
    }

    class PositionKeyframe
    {

        public Vector2 position;
        public Vector2 velocity;
        public float frameTime;
        public Vector2 localScale;
        public string animation;

        public PositionKeyframe(Vector2 position, Vector2 velocity, Vector2 localScale, string animation, float frameTime)
        {
            this.position = position;
            this.velocity = velocity;
            this.frameTime = frameTime;
            this.localScale = localScale;
            this.animation = animation;
        }

    }
}

