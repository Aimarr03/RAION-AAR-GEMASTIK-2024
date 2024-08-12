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

    private List<MainMenuButtonUI> mainMenuButtonList;
    private int currentIndex = -1;
    private int maxIndex = 4;
    private DefaultInputAction inputSystem;
    private MainMenuButtonUI currentMainMenuButton;
    private void Awake()
    {
        inputSystem = new DefaultInputAction();
        inputSystem.Pause_UI.Enable();
        mainMenuButtonList = new List<MainMenuButtonUI>();
        maxIndex = buttonList.Count;
        foreach (var button in buttonList)
        {
            mainMenuButtonList.Add(button.gameObject.GetComponent<MainMenuButtonUI>());
        }
    }
    public void Start()
    {
        OnStartLoading();
        Debug.Log(DataManager.instance.HasGameData());
        if (!DataManager.instance.HasGameData())
        {
            loadButton.interactable = false;
        }
        CreditPanel.gameObject.SetActive(false);

        inputSystem.Pause_UI.Navigation.performed += Navigation_performed;
        inputSystem.Pause_UI.Confirm.performed += Confirm_performed;
    }
    private void OnDisable()
    {
        inputSystem.Pause_UI.Navigation.performed -= Navigation_performed;
        inputSystem.Pause_UI.Confirm.performed -= Confirm_performed;
    }
    private void Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentMainMenuButton?.GetComponent<Button>().onClick.Invoke();
    }

    private void Navigation_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        int input = (int)inputSystem.Pause_UI.Navigation.ReadValue<float>();
        currentIndex += input;
        if (currentIndex >= maxIndex) currentIndex = 0;
        if (currentIndex < 0) currentIndex = maxIndex - 1;
        OnChangeNewFocusedButton(mainMenuButtonList[currentIndex]);
    }
    public void OnChangeNewFocusedButton(MainMenuButtonUI newMainMenuButton)
    {
        currentMainMenuButton?.OnNotFocused();
        currentMainMenuButton = newMainMenuButton;
        currentMainMenuButton?.OnFocused();
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
