using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlatformAnimator : MonoBehaviour
{
    // Sample written for for Spine 3.7
    void Start()
    {
        var skeletonAnimation = GetComponent<SkeletonAnimation>();

        var waves = skeletonAnimation.AnimationState.SetAnimation(0, "Wave", true);
        var bubbles = skeletonAnimation.AnimationState.SetAnimation(1, "Bubbles", true);
        bubbles.TimeScale = 6;
        bubbles.TrackTime = Random.Range(0,bubbles.animationEnd);
        waves.TimeScale = .5f;
        waves.TrackTime = Random.Range(0, bubbles.animationEnd);
    }
}
