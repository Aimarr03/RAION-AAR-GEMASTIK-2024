using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverGameplayUI : MonoBehaviour
{
    [SerializeField] private RectTransform backgroundGameOverContent;
    [SerializeField] private RectTransform GameOverContent;
    [SerializeField] private TextMeshProUGUI descriptionOnWhyLose;

    [SerializeField] private RectTransform GameOverTutorial;
    [SerializeField] private RectTransform GameOverGameplay;
    [SerializeField] private RectTransform CurrentGameOverPanel;
    
    int current_index = 0;
    int MaxIndex => CurrentGameOverPanel.transform.childCount;
    Button currentButton;
    DefaultInputAction inputAction;
    
    private void Awake()
    {
        inputAction = new DefaultInputAction();
        backgroundGameOverContent.gameObject.SetActive(false);
        GameOverContent.DOAnchorPosY(-1080, 0);
    }
    private void Start()
    {
        bool tutorialExistence = TutorialManager.instance != null;
        GameOverTutorial.gameObject.SetActive(tutorialExistence);
        GameOverGameplay.gameObject.SetActive(!tutorialExistence);
        CurrentGameOverPanel = tutorialExistence ? GameOverTutorial : GameOverGameplay;
        ExpedictionManager.Instance.OnLose += Instance_OnLose;
    }
    private void OnDisable()
    {
        ExpedictionManager.Instance.OnLose -= Instance_OnLose;
        inputAction.Pause_UI.Navigation.performed -= Navigation_performed;
        inputAction.Pause_UI.Confirm.performed -= Confirm_performed;
    }

    private async void Instance_OnLose(string type)
    {
        await Task.Delay(800);
        string description = type;
        inputAction.Pause_UI.Enable();
        inputAction.Pause_UI.Navigation.performed += Navigation_performed;
        inputAction.Pause_UI.Confirm.performed += Confirm_performed;
        descriptionOnWhyLose.text = $"Alasan Mengapa Kalah \n{description}";
        backgroundGameOverContent.gameObject.SetActive(true);
        GameOverContent.DOAnchorPosY(0, 0.8f).SetEase(Ease.OutBack);
    }

    private void Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        int input = (int)inputAction.Pause_UI.Navigation.ReadValue<float>();
        int buffer = input + current_index;
        if(buffer >= 0 && buffer < MaxIndex)
        {
            if(currentButton != null) currentButton.GetComponent<Image>().color = Color.white;
            current_index = buffer;
        }
        currentButton = CurrentGameOverPanel.GetChild(current_index).GetComponent<Button>();
        currentButton.GetComponent<Image>().color = Color.blue;
    }

    private void Navigation_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (currentButton != null) currentButton.onClick.Invoke();
    }

    public void OnLoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }
}
