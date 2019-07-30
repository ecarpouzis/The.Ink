using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameObject : MonoBehaviour
{
    public Image gameOverFade;
    bool isEnding = false;
    public void DoEnding()
    {
        if (!isEnding)
        {
            isEnding = true;
            StartCoroutine(LoadNextLevelAsync());
        }
    }

    float timeSinceEnd = 0f;
    float endSpeed = .3f;
    private void Update()
    {
        GameController.curGametime = GameController.G.fixedTimePoint;
        if (isEnding)
        {
            timeSinceEnd += Time.deltaTime;
            Color c = gameOverFade.color;
            c.a = Mathf.Lerp(0, 1, timeSinceEnd * endSpeed);
            gameOverFade.color = c;
            if (c.a >= .99f)
            {
                ao.allowSceneActivation = true;
            }
        }
    }

    AsyncOperation ao;
    IEnumerator LoadNextLevelAsync()
    {
        ao = SceneManager.LoadSceneAsync("End", LoadSceneMode.Single);
        ao.allowSceneActivation = false;
        yield return ao;
    }

}
