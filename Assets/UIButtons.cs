using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public void ContinueGame()
    {
        GameController.G.Pause();
    }
}
