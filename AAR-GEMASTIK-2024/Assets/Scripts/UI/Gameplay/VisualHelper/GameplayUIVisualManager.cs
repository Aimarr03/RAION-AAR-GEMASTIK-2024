using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIVisualManager : MonoBehaviour
{
    public PauseGameplayUI GameplayUI_Script;
    private int currentIndex = 0;
    private int maxIndex = 4;
    private GameplayUIVisualHelperButton currentFocusedButton;
    public List<GameplayUIVisualHelperButton> buttonGameplayUI_List;
    public List<GameplayUIVisualHelperButton> buttonConfirmationUI_List;
    private void Awake()
    {
        currentFocusedButton = buttonGameplayUI_List[currentIndex];
        currentFocusedButton.OnFoccusedButton();
    }
    void Start()
    {
        PauseGameplayUI.OnConfirm += PauseGameplayUI_OnConfirm;
        PauseGameplayUI.OnNegate += Negate_Hide_PauseUI;
        PauseGameplayUI.OnNavigatePauseUI += PauseGameplayUI_OnNavigatePauseUI;
        PauseGameplayUI.OnShowConfirmation += PauseGameplayUI_OnShowConfirmation;
    }
    private void OnDisable()
    {
        PauseGameplayUI.OnConfirm -= PauseGameplayUI_OnConfirm;
        PauseGameplayUI.OnNegate -= Negate_Hide_PauseUI;
        PauseGameplayUI.OnNavigatePauseUI -= PauseGameplayUI_OnNavigatePauseUI;
        PauseGameplayUI.OnShowConfirmation -= PauseGameplayUI_OnShowConfirmation;
    }
    private void PauseGameplayUI_OnConfirm()
    {
        currentFocusedButton?.OnFocusedButtonPerformed();
    }
    private void PauseGameplayUI_OnShowConfirmation(bool showStatus)
    {
        if (showStatus)
        {
            PauseGameplayUI.OnNegate -= Negate_Hide_PauseUI;
            PauseGameplayUI.OnNavigatePauseUI -= PauseGameplayUI_OnNavigatePauseUI;
            
            PauseGameplayUI.OnNegate += Negate_Hide_Confirmation;
            PauseGameplayUI.OnNavigatePauseUI += PauseGameplayUI_OnNavigateConfirmationUI;
            currentIndex = 0;
            maxIndex = buttonConfirmationUI_List.Count;
            ChangeCurrentFocusedButton(buttonConfirmationUI_List[currentIndex]);
        }
        else
        {
            PauseGameplayUI.OnNegate -= Negate_Hide_Confirmation;
            PauseGameplayUI.OnNavigatePauseUI -= PauseGameplayUI_OnNavigateConfirmationUI;
            
            PauseGameplayUI.OnNavigatePauseUI += PauseGameplayUI_OnNavigatePauseUI;
            PauseGameplayUI.OnNegate += Negate_Hide_PauseUI;
            currentIndex = 0;
            maxIndex = buttonGameplayUI_List.Count;
            ChangeCurrentFocusedButton(buttonGameplayUI_List[currentIndex]);
        }
    }
    private void Negate_Hide_PauseUI()
    {
        GameplayUI_Script.Resume();
    }
    private void Negate_Hide_Confirmation()
    {
        GameplayUI_Script.OnExitReallyContainer();
    }
    private void PauseGameplayUI_OnNavigatePauseUI(float value)
    {
        int navigation = (int)value;
        currentIndex += navigation;
        if (currentIndex < 0) currentIndex = maxIndex - 1;
        else if (currentIndex == maxIndex) currentIndex = 0;
        ChangeCurrentFocusedButton(buttonGameplayUI_List[currentIndex]);
    }
    private void PauseGameplayUI_OnNavigateConfirmationUI(float value)
    {
        int navigation = (int)value;
        currentIndex += navigation;
        if (currentIndex < 0) currentIndex = maxIndex - 1;
        else if (currentIndex == maxIndex) currentIndex = 0;
        ChangeCurrentFocusedButton(buttonConfirmationUI_List[currentIndex]);
    }
    private void ChangeCurrentFocusedButton(GameplayUIVisualHelperButton newButton)
    {
        currentFocusedButton.OnNotFoccusedButton();
        currentFocusedButton = newButton;
        currentFocusedButton.OnFoccusedButton();
    }
}
