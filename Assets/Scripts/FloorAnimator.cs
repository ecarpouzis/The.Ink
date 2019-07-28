using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var skeletonAnimation = GetComponent<SkeletonAnimation>();

        var waves = skeletonAnimation.AnimationState.SetAnimation(0, "FloorAnimation", true);
        var bubbles = skeletonAnimation.AnimationState.SetAnimation(1, "FloorBubbles", true);
        bubbles.TimeScale = 1;
        bubbles.TrackTime = Random.Range(0, bubbles.animationEnd);
        
        if(waves.TrackTime < 15)
        {
            waves.TrackTime = 15;
        } if(waves.TrackTime > 45)
        {
            waves.TrackTime = 15;
        }

        if (transform.name == "Floor1")
        {
            waves.TimeScale = .09f;
            
        }
        else if (transform.name == "Floor2")
        {
            waves.TimeScale = .08f;
        }
        waves.TrackTime = Random.Range(0, bubbles.animationEnd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
