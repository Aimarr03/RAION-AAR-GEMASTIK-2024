using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationInputManager : MonoBehaviour
{
    private DefaultInputAction inputSystem;
    [SerializeField] private NPCConversation dummyConverstaion;
    [SerializeField] private RectTransform SkipPanel;
    [SerializeField] private List<Button> SkipPanelButtonList;
    private Button currentSkipButton;
    private int currentIndex = 0;
    private int maxIndex => SkipPanelButtonList.Count;
    private void Awake()
    {
        inputSystem = new DefaultInputAction();
        SkipPanel.gameObject.SetActive(false);
    }
    private void Start()
    {
        ConversationManager.OnConversationStarted += OnConversationStarted;
        ConversationManager.OnConversationEnded += OnConversationFinished;

        EnableDialogueInput();
        
        //DialogueEditor.ConversationManager.Instance.StartConversation(dummyConverstaion);
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= OnConversationStarted;
        ConversationManager.OnConversationEnded -= OnConversationFinished;

        DisableDialogueInput();
    }
    private void DisableDialogueInput()
    {
        inputSystem.Dialogue.Navigation.performed -= Navigation_performed;
        inputSystem.Dialogue.ContinueButton.performed -= ContinueButton_performed;
        inputSystem.Dialogue.Skip.performed -= Skip_performed;
    }
    private void EnableDialogueInput()
    {
        inputSystem.Dialogue.Navigation.performed += Navigation_performed;
        inputSystem.Dialogue.ContinueButton.performed += ContinueButton_performed;
        inputSystem.Dialogue.Skip.performed += Skip_performed;
    }

    private void OnConversationStarted()
    {
        inputSystem.Dialogue.Enable();
    }
    private void OnConversationFinished()
    {
        inputSystem.Dialogue.Disable();
        
    }
    private void ContinueButton_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ConversationManager.Instance.PressSelectedOption();
    }
    private void Skip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        SkipAction();
    }
    public void SkipAction()
    {
        DisableDialogueInput();
        inputSystem.Dialogue.Navigation.performed += NavigateSkipButton;
        inputSystem.Dialogue.ContinueButton.performed += OnInterractSkipButton;
        SkipPanel.gameObject.SetActive(true);
    }

    private void Navigation_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float input = inputSystem.Dialogue.Navigation.ReadValue<float>();
        if (input > 0)
        {
            ConversationManager.Instance.SelectNextOption();
        }
        else if (input < 0)
        {
            ConversationManager.Instance.SelectPreviousOption();
        }
    }
    public void OnConfirmSkip()
    {
        ConversationManager.Instance.EndConversation();
        OnCancelSkip();
    }
    public void OnCancelSkip()
    {
        EnableDialogueInput();
        inputSystem.Dialogue.Navigation.performed -= NavigateSkipButton;
        inputSystem.Dialogue.ContinueButton.performed -= OnInterractSkipButton;
        SkipPanel.gameObject.SetActive(false);
        currentIndex = 0;
        if(currentSkipButton != null)currentSkipButton.GetComponent<Image>().color = Color.white;
        currentSkipButton = null;
    }
    public void NavigateSkipButton(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        int value = (int)inputSystem.Dialogue.Navigation.ReadValue<float>();
        int bufferIndex = currentIndex + value;
        if(bufferIndex >= 0 && bufferIndex < maxIndex)
        {
            currentIndex = bufferIndex;
        }
        if (currentSkipButton != null) currentSkipButton.GetComponent<Image>().color = Color.white;
        currentSkipButton = SkipPanelButtonList[currentIndex];
        currentSkipButton.GetComponent <Image>().color = Color.blue;
    }
    public void OnInterractSkipButton(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (currentSkipButton == null) return;
        currentSkipButton.onClick.Invoke();
    }
}
