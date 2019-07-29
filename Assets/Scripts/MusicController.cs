using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public SoundSubClip gameMusic;
    public SoundSubClip gameMusicReversed;
    public static MusicController m;

    private void Awake()
    {
        m = this;
    }

    public void PlayFromCurpoint()
    {
        gameMusicReversed.Stop();
        gameMusic.Stop();
        gameMusic.Play(GameController.G.fixedTimePoint);
    }

    public void StartForwardMusic()
    {
        gameMusicReversed.Stop();
        gameMusic.Play(GameController.G.fixedTimePoint);
    }


    public void StopMusic()
    {

        gameMusicReversed.Stop();
        gameMusic.Stop();
    }

    public void StartRewoundMusic()
    {
        gameMusic.Stop();
        gameMusicReversed.Play(gameMusicReversed.thisClip.clip.length - GameController.G.fixedTimePoint);
    }

}
