using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Munch : SkeletonAnimator
{
    private void Start()
    {
        var Headbob = skeletonAnimation.AnimationState.SetAnimation(0, "Headbob", true);
        var LowerClawTwitch = skeletonAnimation.AnimationState.SetAnimation(1, "LowerClawTwitch", true);
        var NewWalk = skeletonAnimation.AnimationState.SetAnimation(2, "NewWalk", true);
        var ArmSway = skeletonAnimation.AnimationState.SetAnimation(3, "ArmSway", true);
    }
    

}
