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
    [SerializeField] AudioClip interractable, showup, closeup;

    private DefaultInputAction inputAction;
    private bool isPause;
    public static event Action<bool> OnPause;
    private string nameSceneToLoad;
    private void Awake()
    {
        inputAction = new DefaultInputAction();
        inputAction.UI.Enable();
        ReallyContainer.gameObject.SetActive(false);
        isPause = false;
        nameSceneToLoad = "";
    }
    private void Start()
    {
        inputAction.UI.Pause.performed += Pause_performed;
    }
    private void OnDisable()
    {
        inputAction.UI.Disable();
        inputAction.UI.Pause.performed -= Pause_performed;
    }
    private void Pause()
    {
        
        isPause = !isPause;
        if (isPause) PerformedPause();
        else PerformedResume();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isPause = !isPause;
        if (isPause) PerformedPause();
        else PerformedResume();
    }
    private async void PerformedPause()
    {
        PauseContainer.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);
        background.gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(showup);
        await Task.Delay(600);
        OnPause?.Invoke(isPause);
        Time.timeScale = 0;
    }
    private async void PerformedResume()
    {
        PauseContainer.DOAnchorPosY(1080, 0.3f).SetEase(Ease.InBack);
        background.gameObject.SetActive(false);
        AudioManager.Instance.PlaySFX(closeup);
        await Task.Delay(600);
        Time.timeScale = 1;
        OnPause?.Invoke(isPause);
    }
    public void OnResume()
    {
        Pause();
        AudioManager.Instance.PlaySFX(closeup);
    }
    public void OnSetToLoadScene(string sceneName)
    {
        AudioManager.Instance.PlaySFX(interractable);
        nameSceneToLoad = sceneName;
        ReallyContainer.gameObject.SetActive(true);
    }
    public void OnExitReallyContainer() => ReallyContainer.gameObject.SetActive(false);
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
}
