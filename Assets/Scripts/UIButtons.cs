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
        NewCharacterController character = GameObject.Find("Character").GetComponent<NewCharacterController>();
        character.Revive();
        character.transform.position = playerStartPosition.position;
        character.transform.localScale = playerStartPosition.localScale;
        character.skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
        character.timeSinceDeath = 0f;
        character._rigidbody.velocity = Vector2.zero;
        character.gameObject.GetComponent<TimeController>().Reset();
        GameController.G.isRewinding = false;
        GameController.G.ResetTimes();
        GameController.G.Pause();
        GameObject.Find("GameMusic").GetComponent<MusicController>().PlayFromCurpoint();
    }

}
