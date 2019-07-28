using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlatformAnimator : SkeletonAnimator
{
    // Sample written for for Spine 3.7
    void Start()
    {

        var waves = skeletonAnimation.AnimationState.SetAnimation(0, "Wave", true);
        var bubbles = skeletonAnimation.AnimationState.SetAnimation(1, "Bubbles", true);
        if (transform.name.Contains("ShortPlatform"))
        {
            bubbles.TimeScale = 1;
        }
        else
        {
            bubbles.TimeScale = 6;
        }
        
        waves.TimeScale = .5f;

        base.offsetTimes.Add("Bubbles", Random.Range(0, bubbles.animationEnd));
        base.offsetTimes.Add("Wave", Random.Range(0, waves.animationEnd));
    }
}
