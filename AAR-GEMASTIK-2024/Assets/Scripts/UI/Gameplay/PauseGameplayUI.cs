using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PauseGameplayUI : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform PauseContainer;
    [SerializeField] RectTransform ReallyContainer;
    [SerializeField] RectTransform UI_Guide;
    [SerializeField] AudioClip interractable, showup, closeup;

    private DefaultInputAction inputAction;
    private bool isPause;
    public static event Action<bool> OnPause;
    public static event Action<float> OnNavigatePauseUI;
    public static event Action OnConfirm;
    public static event Action OnNegate;
    public static event Action<bool> OnShowConfirmation;
    private string nameSceneToLoad;
    private void Awake()
    {
        inputAction = new DefaultInputAction();
        ReallyContainer.gameObject.SetActive(false);
        UI_Guide.gameObject.SetActive(false);
        isPause = false;
        nameSceneToLoad = "";
    }
    private void Start()
    {
        PlayerInputSystem.InvokePause += PlayerInputSystem_InvokePause;
        inputAction.Pause_UI.Disable();
        inputAction.Pause_UI.Navigation.performed += Navigation_performed;
        inputAction.Pause_UI.Negate.performed += Negate_performed;
        inputAction.Pause_UI.Confirm.performed += Confirm_performed;
    }
    private void OnDisable()
    {
        PlayerInputSystem.InvokePause -= PlayerInputSystem_InvokePause;
        inputAction.Pause_UI.Navigation.performed -= Navigation_performed;
        inputAction.Pause_UI.Negate.performed -= Negate_performed;
        inputAction.Pause_UI.Confirm.performed -= Confirm_performed;
    }
    private void Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnConfirm?.Invoke();
    }

    private void Negate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnNegate?.Invoke();
    }

    private void Navigation_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float readValue = inputAction.Pause_UI.Navigation.ReadValue<float>();
        Debug.Log("Value on Navigation " + readValue);
        OnNavigatePauseUI?.Invoke(readValue);
    }
    public void Pause()
    {
        PlayerInputSystem_InvokePause();
    }
    private void PlayerInputSystem_InvokePause()
    {
        isPause = true;
        PerformedPause();
    }
    private async void PerformedPause()
    {
        PauseContainer.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);
        background.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(showup);
        await Task.Delay(600);
        OnPause?.Invoke(isPause);
        Time.timeScale = 0;
        inputAction.Pause_UI.Enable();
    }
    public void Resume()
    {
        isPause = false;
        PerformedResume();
    }
    private async void PerformedResume()
    {
        Time.timeScale = 1;
        PauseContainer.DOAnchorPosY(1080, 0.3f).SetEase(Ease.InBack);
        background.gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX(closeup);
        await Task.Delay(600);
        OnPause?.Invoke(isPause);
        inputAction.Pause_UI.Disable();
    }
    
    public void OnSetToLoadScene(string sceneName)
    {
        AudioManager.Instance.PlaySFX(interractable);
        nameSceneToLoad = sceneName;
        OnShowConfirmation?.Invoke(true);
        ReallyContainer.gameObject.SetActive(true);
    }
    public void OnExitReallyContainer()
    {
        OnShowConfirmation?.Invoke(false);
        ReallyContainer.gameObject.SetActive(false);
    }
    public void OnLoadScne()
    {
        AudioManager.Instance.PlaySFX(interractable);
        Time.timeScale = 1;
        if(nameSceneToLoad == "Quit")
        {
            Application.Quit();
            return;
        }
        GameManager.Instance.LoadScene(nameSceneToLoad);
    }
    public void ShowGuide()
    {
        UI_Guide.gameObject.SetActive(true);
        inputAction.Pause_UI.Negate.performed -= Negate_performed;
        inputAction.Pause_UI.Negate.performed += UIGuide_performed;
    }
    private void UIGuide_performed(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        UI_Guide.gameObject.SetActive(false);
        inputAction.Pause_UI.Negate.performed += Negate_performed;
        inputAction.Pause_UI.Negate.performed -= UIGuide_performed;
    }
}
