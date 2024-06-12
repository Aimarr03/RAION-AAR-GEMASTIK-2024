using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseShoppingUI : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform PauseContainer;

    private DefaultInputAction playerInput;
    public static event Action OnPause;
    private bool isPause;
    private void Awake()
    {
        playerInput = new DefaultInputAction();
        playerInput.UI.Enable();
        isPause = false;
    }
    private void Start()
    {
        PauseContainer.DOAnchorPosY(1080, 0.3f);
        playerInput.UI.Pause.performed += Pause_performed;
    }
    private void OnDisable()
    {
        playerInput.UI.Pause.performed -= Pause_performed;
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
    private void PerformedPause()
    {
        PauseContainer.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack);
        background.gameObject.SetActive(true);
    }
    private void PerformedResume()
    {
        PauseContainer.DOAnchorPosY(1080, 0.3f).SetEase(Ease.InBack);
        background.gameObject.SetActive(false);
    }
    public void OnResume()
    {
        Pause();
    }
    public void OnGoToMainMenu()
    {
        DataManager.instance.SaveGame();
        Pause();
        SceneManager.LoadScene(0);
    }
    public void OnExit()
    {
        DataManager.instance.SaveGame();
        Pause();
        Application.Quit();
    }
}
