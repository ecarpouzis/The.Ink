using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDrip : MonoBehaviour
{
    public CircleCollider2D myCollider;
    public float delay = 0;
    MeshRenderer myMesh;
    TrackEntry drip;
    private void Awake()
    {
        myMesh = GetComponent<MeshRenderer>();
        myMesh.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {

        var sk = GetComponent<SkeletonAnimation>();
        drip = sk.AnimationState.AddAnimation(0, "DripGrow", false, delay);
        var fall = sk.AnimationState.AddAnimation(0, "DripFall", false, 0);
        sk.AnimationState.AddAnimation(0, "DripFalling", true, 0);
        fall.End += StartFalling;

    }

    private void Update()
    {
        if (drip.trackTime > 0)
        {
            myMesh.enabled = true;
        }
    }

    void StartDrip(TrackEntry trackEntry)
    {
        Debug.Log("Drip!");
    }

    void StartFalling(TrackEntry trackEntry)
    {
        myCollider.enabled = true;
    }
}
