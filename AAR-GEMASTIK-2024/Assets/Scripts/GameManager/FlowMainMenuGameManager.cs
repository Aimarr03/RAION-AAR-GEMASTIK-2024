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
        GameManager.Instance.LoadScene("Level1");
    }
    public void LoadGame()
    {
        OnCustomDisable();
        Debug.Log("Continue Game");
        DataManager.instance.LoadGame();
        GameManager.Instance.LoadScene(1);
    }
    public void OnCustomDisable()
    {
        AudioManager.Instance.OnGraduallyStopUnderwaterSFX(0.8f);
        foreach (Button button in buttonList)
        {
            button.interactable = false;
        }
    }
    public void OnLoadCredit()
    {
        GameManager.Instance.LoadScene("Credits");
    }
}
