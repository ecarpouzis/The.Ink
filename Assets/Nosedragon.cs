using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nosedragon : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    // Start is called before the first frame update
    void Start()
    {

        skeletonAnimation = GetComponent<SkeletonAnimation>();
        var noseSway = skeletonAnimation.AnimationState.SetAnimation(0, "NoseSway", true);

        //bubbles.TrackTime = Random.Range(0, bubbles.animationEnd);
        //waves.TimeScale = .5f;
        //waves.TrackTime = Random.Range(0, bubbles.animationEnd);
    }

    bool hasPlayedMooseman = false;

    private void Update()
    {
        if (!hasPlayedMooseman && GameController.G.currentTimePoint > 33)
        {
            hasPlayedMooseman = true;
            var mooseman = skeletonAnimation.AnimationState.SetAnimation(1, "MoosemanMove", false);
        }
    }
}
