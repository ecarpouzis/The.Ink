using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{
    float timeSinceStart;
    bool gameStarted = false;
    public Transform drip;
    float dropSpeed = .6f;
    public ParticleSystem gameStartSplash;
    public MeshRenderer character;
    public GameObject Logo;
    public GameObject introCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")){
            gameStarted = true;
            var skel = drip.GetComponent<SkeletonAnimation>();
            var fall = skel.AnimationState.SetAnimation(0, "DripFall", true);
            fall.TimeScale = .4f;
        }
        if (gameStarted)
        {
            timeSinceStart += Time.deltaTime;
            drip.transform.Translate(Vector3.down * dropSpeed);
            transform.Translate(Vector3.down * dropSpeed);
        }
        if(drip.transform.position.y < -4.09)
        {
            var cam = GameObject.Find("Main Camera");
            cam.GetComponent<Camera>().enabled = true;
            cam.GetComponent<CameraController>().enabled = true;
            cam.GetComponent<GameController>().StartPlaying();
            cam.GetComponent<AudioListener>().enabled = true;
            this.GetComponent<Camera>().enabled = false;
            gameStartSplash.Play();
            character.enabled = true;
            Logo.SetActive(false);
            introCanvas.SetActive(false);
            Destroy(drip.gameObject);
            Destroy(this.gameObject);
        }
    }
}
