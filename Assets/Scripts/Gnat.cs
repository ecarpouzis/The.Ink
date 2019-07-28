using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnat : SkeletonAnimator
{
    public ReverseParticleSystem Particles1;
    public ReverseParticleSystem Particles2;

    new void Update()
    {
        if (Particles1 != null)
        {
            if (GameController.G.isRewinding)
            {
                if (!Particles1.enabled)
                {
                    Particles1.enabled = true;
                }
                if (!Particles2.enabled)
                {
                    Particles2.enabled = true;
                }
            }
            else
            {
                if (Particles1.enabled)
                {
                    Particles1.enabled = false;
                }
                if (Particles2.enabled)
                {
                    Particles2.enabled = false;
                }
            }

            base.Update();
        }
    }
}