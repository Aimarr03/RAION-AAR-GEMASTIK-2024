using DG.Tweening;
using System.Buffers;
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
    public List<MainMenuButtonUI> mainMenuList;
    public Button loadButton;
    public Transform CreditPanel;

    [Header("Tutorial Panel")]
    public RectTransform tutorialPanel;
    public List<MainMenuButtonUI> tutorialButtonList;
    int maxIndexTutorialPanel => tutorialButtonList.Count;
    [Header("New Game Panel")]
    public RectTransform newGamePanel;
    public List<MainMenuButtonUI> newGameButtonList;
    
    
    int maxIndexList => currentList.Count;
    
    private List<MainMenuButtonUI> currentList;
    private int currentIndex = -1;
    private DefaultInputAction inputSystem;
    private MainMenuButtonUI currentMainMenuButton;
    private bool onConfirmNewGame;
    private void Awake()
    {
        inputSystem = new DefaultInputAction();
        inputSystem.Pause_UI.Enable();
        currentList = mainMenuList;
        tutorialPanel.gameObject.SetActive(false);
        newGamePanel.gameObject.SetActive(false);
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

        inputSystem.Pause_UI.Navigation.performed += MainMenu_Navigation_performed;
        inputSystem.Pause_UI.Confirm.performed += MainMenu_Confirm_performed;
    }
    private void OnDisable()
    {
        inputSystem.Pause_UI.Navigation.performed -= MainMenu_Navigation_performed;
        inputSystem.Pause_UI.Confirm.performed -= MainMenu_Confirm_performed;
    }
    private void MainMenu_Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentMainMenuButton?.GetComponent<Button>().onClick.Invoke();
    }

    private void MainMenu_Navigation_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(currentIndex == -1)
        {
            currentIndex = 0;
        }
        else
        {
            int input = (int)inputSystem.Pause_UI.Navigation.ReadValue<float>();
            int buffer = currentIndex + input;
            if (buffer >= 0 && buffer < currentList.Count) currentIndex = buffer;
        }
        OnChangeNewFocusedButton(currentList[currentIndex]);
    }
    public void OnChangeNewFocusedButton(MainMenuButtonUI newMainMenuButton)
    {
        currentMainMenuButton?.OnNotFocused();
        currentMainMenuButton = newMainMenuButton;
        currentMainMenuButton?.OnFocused();
    }
    
    public void OnQuit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        //OnCustomDisable();
        Debug.Log("Start New Game");
        if (DataManager.instance.HasGameData())
        {
            newGamePanel.gameObject.SetActive(true);
            tutorialPanel.gameObject.SetActive(false);
            currentList = newGameButtonList;
            currentIndex = -1;
            OnChangeNewFocusedButton(null);
        }
        else
        {
            LoadTutorialPanel();
        }
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
        foreach (MainMenuButtonUI button in mainMenuList)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }
    public void OnLoadCredit()
    {
        GameManager.Instance.LoadScene("Credits");
    }
    public void OnConfirmNewGame()
    {
        onConfirmNewGame = true;
        LoadTutorialPanel();
    }
    public void OnConfirmTutorial(bool input)
    {
        DataManager.instance.NewGame();
        string levelName = input ? "Level Tutorial" : "ShoppingMenu";
        if(!input)DataManager.instance.gameData.money = 1000;
        DataManager.instance.gameData.tutorialGameplay = input;
        DataManager.instance.gameData.tutorialShopDone = !input;
        GameManager.Instance.LoadScene(levelName);
    }
    public void OnCancel()
    {
        newGamePanel.gameObject.SetActive(false);
        tutorialPanel.gameObject.SetActive(false);
        currentList = mainMenuList;
        currentIndex = -1;
        OnChangeNewFocusedButton(null);
    }
    public void LoadTutorialPanel()
    {
        newGamePanel.gameObject.SetActive(false);
        tutorialPanel.gameObject.SetActive(true);
        currentList = tutorialButtonList;
        currentIndex = -1;
        OnChangeNewFocusedButton(null);
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
}
