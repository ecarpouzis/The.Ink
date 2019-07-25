using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIButtons : MonoBehaviour
{  

    public void ContinueGame()
    {
        GameController.G.Pause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Transform playerStartPosition = GameObject.Find("CharStartPosition").transform;
        CharacterController2D character = GameObject.Find("Character").GetComponent<CharacterController2D>();
        character.Revive();
        character.transform.position = playerStartPosition.position;
        character.transform.localScale = playerStartPosition.localScale;
        character.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        character.grounded = true;
        character.timeSinceDeath = 0f;
        character.velocity = Vector2.zero;
        character.gameObject.GetComponent<TimeController>().Reset();
        GameController.G.timeSinceStart = 1;
        GameController.G.isRewinding = false;
        GameController.G.timePlaying = 0f;
        GameController.G.timeRewinding = 0f;
        GameController.G.currentTimePoint = 0f;
        GameController.G.timeSinceRewind = 0f;
        GameController.G.Pause();
        GameObject.Find("GameMusic").GetComponent<MusicController>().PlayFromStart();
    }

}
