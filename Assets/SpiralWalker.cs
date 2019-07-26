using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralWalker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        var skeletonAnimation = GetComponent<SkeletonAnimation>();

        var CurvedTongue = skeletonAnimation.AnimationState.SetAnimation(0, "CurvedTongue", true);
        var ForkedTongue = skeletonAnimation.AnimationState.SetAnimation(1, "ForkedTongue", true);
        var EyeTwitch = skeletonAnimation.AnimationState.SetAnimation(2, "EyeTwitch", true);
        CurvedTongue.TrackTime = Random.Range(0, CurvedTongue.animationEnd);
        ForkedTongue.TrackTime = Random.Range(0, ForkedTongue.animationEnd);
        EyeTwitch.TrackTime = Random.Range(0, EyeTwitch.animationEnd);
    }
    
}
