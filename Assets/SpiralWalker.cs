using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralWalker : SkeletonAnimator
{
    // Start is called before the first frame update
    void Start()
    {
        var CurvedTongue = skeletonAnimation.AnimationState.SetAnimation(0, "CurvedTongue", true);
        var ForkedTongue = skeletonAnimation.AnimationState.SetAnimation(1, "ForkedTongue", true);
        var EyeTwitch = skeletonAnimation.AnimationState.SetAnimation(2, "EyeTwitch", true);

        base.offsetTimes.Add("CurvedTongue", Random.Range(0, CurvedTongue.animationEnd));
        base.offsetTimes.Add("ForkedTongue", Random.Range(0, ForkedTongue.animationEnd));
        base.offsetTimes.Add("EyeTwitch", Random.Range(0, EyeTwitch.animationEnd));

    }
    
}
