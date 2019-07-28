using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleaAnimator : SkeletonAnimator
{
    public ReverseParticleSystem fleaBubblesRewind;

    void Start()
    {
        var Crystals = skeletonAnimation.AnimationState.SetAnimation(0, "Crystals", true);
        var Head = skeletonAnimation.AnimationState.SetAnimation(1, "FleaHead", true);
        var Body = skeletonAnimation.AnimationState.SetAnimation(2, "FleaBody", true);
        var MushroomTentacles = skeletonAnimation.AnimationState.SetAnimation(3, "MushroomTentacles", true);
        var MushroomPuff = skeletonAnimation.AnimationState.SetAnimation(4, "MushroomPuff", true);

        base.offsetTimes.Add("FleaHead", Random.Range(0, Head.animationEnd));
        base.offsetTimes.Add("Crystals", Random.Range(0, Crystals.animationEnd));
        base.offsetTimes.Add("FleaBody", Random.Range(0, Body.animationEnd));
    }

    new void Update()
    {
        if (GameController.G.isRewinding)
        {
            if (!fleaBubblesRewind.enabled)
            {
                fleaBubblesRewind.enabled = true;
            }
        }
        else
        {
            if (fleaBubblesRewind.enabled)
            {
                fleaBubblesRewind.enabled = false;
            }
        }
        base.Update();
    }
}
