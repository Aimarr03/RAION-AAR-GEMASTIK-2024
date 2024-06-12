using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlowMainMenuGameManager : MonoBehaviour
{
    public List<Button> buttonList;
    public Button loadButton;
    public Transform CreditPanel;

    public void Start()
    {
        Debug.Log(DataManager.instance.HasGameData());
        if (!DataManager.instance.HasGameData())
        {
            loadButton.interactable = false;
        }
        
        CreditPanel.gameObject.SetActive(false);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        OnCustomDisable();
        Debug.Log("Start New Game");
        DataManager.instance.NewGame();
        SceneManager.LoadSceneAsync("Level1");
    }
    public void LoadGame()
    {
        OnCustomDisable();
        Debug.Log("Continue Game");
        DataManager.instance.LoadGame();
        SceneManager.LoadSceneAsync(1);
    }
    public void OnCustomDisable()
    {
        foreach (Button button in buttonList)
        {
            button.interactable = false;
        }
    }
    public void ToggleCredit(bool toggle)
    {
        CreditPanel.gameObject.SetActive(toggle);
    }
}
