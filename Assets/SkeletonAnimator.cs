using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimator : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public float playAt = 0;
    Dictionary<string, float> timeScales = new Dictionary<string, float>();
    public Dictionary<string, float> offsetTimes = new Dictionary<string, float>();

    public void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    // Update is called once per frame
    public void Update()
    {
        var tracks = skeletonAnimation.AnimationState.Tracks;

        foreach (var track in tracks)
        {
            if (!timeScales.ContainsKey(track.animation.name))
            {
                timeScales.Add(track.animation.name, track.timeScale);
            }
        }

        if (GameController.G.isRewinding)
        {
            foreach (var currentMovementTrack in tracks)
            {
                float offset = 0;
                if (offsetTimes.ContainsKey(currentMovementTrack.animation.name)){
                    offset = offsetTimes[currentMovementTrack.animation.name];
                }

                currentMovementTrack.timeScale = 0f; // so time isn't moved forward by SkeletonAnimation/AnimationState
                currentMovementTrack.animationLast = 0f; // this may cause multiple events to fire if you have those.
                var targetTime = GameController.G.currentTimePoint * timeScales[currentMovementTrack.animation.name] + offset; // you should decrement targetTime by a delta time (to act as the animation time cursor that moves backwards.)
                if (!currentMovementTrack.loop)
                {
                    targetTime = GameController.G.currentTimePoint - playAt;
                }
                if (targetTime < 0)
                {
                    targetTime = currentMovementTrack.animationEnd;
                }
                currentMovementTrack.trackTime = targetTime;
            }
            skeletonAnimation.state.Apply(skeletonAnimation.skeleton); // may be required, depending on project script execution order settings. It would be more efficient if this script runs before the SkeletonAnimation.cs and you won't need this.
        }
        else
        {
            foreach (var currentMovementTrack in tracks)
            {
                float offset = 0;
                if (offsetTimes.ContainsKey(currentMovementTrack.animation.name))
                {
                    offset = offsetTimes[currentMovementTrack.animation.name];
                }
                currentMovementTrack.timeScale = timeScales[currentMovementTrack.animation.name]; // so time isn't moved forward by SkeletonAnimation/AnimationState
                if (currentMovementTrack.loop)
                {
                    currentMovementTrack.trackTime = GameController.G.currentTimePoint * timeScales[currentMovementTrack.animation.name] + offset; //When we continue playing, start from the correct timepoint
                }
                else
                {
                    currentMovementTrack.trackTime = GameController.G.currentTimePoint - playAt;
                }
            }
            skeletonAnimation.state.Apply(skeletonAnimation.skeleton); // may be required, depending on project script execution order settings. It would be more efficient if this script runs before the SkeletonAnimation.cs and you won't need this.
        }
    }
}
