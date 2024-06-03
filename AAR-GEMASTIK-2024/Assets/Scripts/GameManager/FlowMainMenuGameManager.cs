using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowMainMenuGameManager : MonoBehaviour
{
    public void OnExit()
    {
        Application.Quit();
    }
    public void OnNewGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
