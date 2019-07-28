using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nosedragon : SkeletonAnimator
{
    // Start is called before the first frame update
    void Start()
    {
        base.playAt = 33;
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        var noseSway = skeletonAnimation.AnimationState.SetAnimation(0, "NoseSway", true);
    }

    bool hasPlayedMooseman = false;
    Spine.TrackEntry mooseTrack;

    new void Update()
    {
        //If we we haven't played the animation, it's after the time to start, and we're not rewinding, play the animation
        if (!hasPlayedMooseman && GameController.G.currentTimePoint > base.playAt && !GameController.G.isRewinding)
        {
            hasPlayedMooseman = true;
            mooseTrack = skeletonAnimation.AnimationState.SetAnimation(1, "MoosemanMove", false);
        }

        //If we have a defined track
        if (mooseTrack != null) {
            //If we've played the animaion, it's before the time the animation ends, and the animation isn't currently playing, play the animation
            if (GameController.G.currentTimePoint < mooseTrack.animationEnd + base.playAt && mooseTrack.IsComplete && GameController.G.isRewinding)
            {
                mooseTrack = skeletonAnimation.AnimationState.SetAnimation(1, "MoosemanMove", false);
                hasPlayedMooseman = false;
            }
        }
        base.Update();
    }
}
