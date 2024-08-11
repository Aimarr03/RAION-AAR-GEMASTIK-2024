using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIVisualManager : MonoBehaviour
{
    public PauseGameplayUI GameplayUI_Script;
    private int currentIndex = 0;
    private int maxIndex = 4;
    private GameplayUIVisualHelperButton currentFocusedButton;
    public List<GameplayUIVisualHelperButton> buttonList;
    private void Awake()
    {
        currentFocusedButton = buttonList[currentIndex];
        currentFocusedButton.OnFoccusedButton();
    }
    void Start()
    {
        PauseGameplayUI.OnConfirm += PauseGameplayUI_OnConfirm;
        PauseGameplayUI.OnNegate += PauseGameplayUI_OnNegate;
        PauseGameplayUI.OnNavigatePauseUI += PauseGameplayUI_OnNavigatePauseUI;
    }
    private void OnDisable()
    {
        PauseGameplayUI.OnConfirm += PauseGameplayUI_OnConfirm;
        PauseGameplayUI.OnNegate += PauseGameplayUI_OnNegate;
        PauseGameplayUI.OnNavigatePauseUI += PauseGameplayUI_OnNavigatePauseUI;
    }
    private void PauseGameplayUI_OnNavigatePauseUI(float value)
    {
        int navigation = (int)value;
        currentIndex += navigation;
        if (currentIndex < 0) currentIndex = maxIndex - 1;
        else if (currentIndex == maxIndex) currentIndex = 0;
        GameplayUIVisualHelperButton newFocusedButton = buttonList[currentIndex];
        currentFocusedButton.OnNotFoccusedButton();
        currentFocusedButton = newFocusedButton;
        currentFocusedButton.OnFoccusedButton();
    }

    private void PauseGameplayUI_OnNegate()
    {
        
    }

    private void PauseGameplayUI_OnConfirm()
    {
        currentFocusedButton?.OnFocusedButtonPerformed();
    }
}
