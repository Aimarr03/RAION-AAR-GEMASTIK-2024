using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlowMainMenuGameManager : MonoBehaviour
{
    [Header("Data FLow")]
    [SerializeField] private Image Background;
    [SerializeField] private Image Ship;
    [SerializeField] private Image trashPlastic1;
    [SerializeField] private Image trashPlastic2;
    [SerializeField] private Image trashBottle;
    [SerializeField] private Image fish;
    [Header("Data UI")]
    public List<Button> buttonList;
    public Button loadButton;
    public Transform CreditPanel;

    public void Start()
    {
        OnStartLoading();
        Debug.Log(DataManager.instance.HasGameData());
        if (!DataManager.instance.HasGameData())
        {
            loadButton.interactable = false;
        }
        CreditPanel.gameObject.SetActive(false);
    }
    private void OnStartLoading()
    {
        float y_value = 0;
        y_value = Ship.GetComponent<RectTransform>().anchoredPosition.y;
        Ship.GetComponent<RectTransform>().DOAnchorPosY(-10 + y_value, Random.Range(0.9f, 1.7f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        y_value = trashPlastic2.GetComponent<RectTransform>().anchoredPosition.y;
        trashPlastic2.GetComponent<RectTransform>().DOAnchorPosY(-10 + y_value, Random.Range(0.9f, 1.7f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        y_value = trashBottle.GetComponent<RectTransform>().anchoredPosition.y;
        trashBottle.GetComponent<RectTransform>().DOAnchorPosY(-10 + y_value, Random.Range(0.9f, 1.7f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        y_value = fish.GetComponent<RectTransform>().anchoredPosition.y;
        fish.GetComponent<RectTransform>().DOAnchorPosY(-10 + y_value, Random.Range(0.9f, 1.7f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
        GameManager.Instance.LoadScene(2);
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
