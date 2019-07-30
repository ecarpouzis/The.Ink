using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
    public SoundSubClip gongEffect;
    public RectTransform creditsPanel;
    public float timeEnding;
    public SkeletonAnimation fisherman;
    bool isScrolling = true;
    float scrollSpeed = 1f;
    float fishermanSpeed = 3f;
    float timewalking = 0f;
    public SoundSubClip music;

    // Start is called before the first frame update
    void Start()
    {
        fisherman.AnimationState.SetAnimation(0, "Fishing", true);
        music.Play(GameController.curGametime);
    }

    bool hasWalkInitialized = false;
    void initializeWalk()
    {
        fisherman.AnimationState.AddAnimation(0, "Stand", false, 0);
        walk = fisherman.AnimationState.AddAnimation(0, "Walk", true, 0);
        hasWalkInitialized = true;
    }

    TrackEntry walk;
    TrackEntry gongHit;
    bool hasPlayedGongsound = false;
    float hasGonging = 0f;
    public Transform gongRotObj;
    float gongSpeed = 50f;
    //float gongRotScale = 1;
    // Update is called once per frame
    void Update()
    {
        if(hasGonging > 7f && hasPlayedGongsound)
        {
            SceneManager.LoadScene("Main");
        }
        if (hasPlayedGongsound)
        {
            Vector3 gongPos = gongRotObj.transform.position;
            Quaternion rot = gongRotObj.localRotation;
            Vector3 rotEuler = rot.eulerAngles;
            rotEuler.x = 0-Mathf.PingPong(hasGonging * gongSpeed, 45f);
            
            gongRotObj.localRotation = Quaternion.Euler(rotEuler);
        }

        timeEnding += Time.deltaTime;
        if (timewalking > 1f)
        {
            fisherman.skeleton.FindSlot("FishingLine").Attachment = null;
        }
        if (isScrolling)
        {
            float num = creditsPanel.offsetMax.y;
            num += timeEnding * scrollSpeed;
            creditsPanel.offsetMax = new Vector2(creditsPanel.offsetMax.x, num);
            if (creditsPanel.offsetMax.y > 6000)
            {
                isScrolling = false;
            }
        }else if(fisherman.transform.position.x >= 18)
        {
            hasGonging += Time.deltaTime;
            if (gongHit == null)
            {
                gongHit = fisherman.AnimationState.SetAnimation(0, "Gonghit", false);
            }
            if(hasGonging >= 1.2f && !hasPlayedGongsound)
            {
                DoGong();
            }
        }

        else
        {
            timewalking += Time.deltaTime;
            fisherman.skeleton.FindSlot("Ripples").Attachment = null;
            fisherman.skeleton.FindSlot("whiteout2").Attachment = null;
            if (!hasWalkInitialized)
            {
                initializeWalk();
            }
            float fishermanPos = fisherman.transform.position.x;
            if (walk != null)
            {
                fishermanPos += Time.deltaTime * fishermanSpeed;
                fisherman.transform.position = new Vector2(fishermanPos, fisherman.transform.position.y);
            }
        }
    }

    void DoGong()
    {
        gongEffect.Play(.2f);
        hasPlayedGongsound = true;
    }
}
