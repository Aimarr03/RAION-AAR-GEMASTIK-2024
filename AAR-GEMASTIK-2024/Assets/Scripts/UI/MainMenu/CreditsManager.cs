using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private RectTransform CreditsContainer;
    [SerializeField] private AudioClip creditsAudioClip, interactableAction, uninterractableAction;
    [SerializeField] private float multiplierMove = 2410;
    private DefaultInputAction inputAction;
    private int index = 0;
    private int maxIndex = 1;

    private void Awake()
    {
        inputAction = new DefaultInputAction();
        inputAction.Credits.Enable();
        index = 0;
    }
    private void Start()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.StartNewMusic(creditsAudioClip, 0.3f, 0.6f);
        }
        inputAction.Credits.NextPage.performed += NextPage_performed;
        inputAction.Credits.Previouspage.performed += Previouspage_performed;
        inputAction.Credits.Escape.performed += Escape_performed;
    }
    private void OnDisable()
    {
        inputAction.Credits.Disable();
        inputAction.Credits.NextPage.performed -= NextPage_performed;
        inputAction.Credits.Previouspage.performed -= Previouspage_performed;
        inputAction.Credits.Escape.performed -= Escape_performed;
    }
    private void NextPage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (index >= maxIndex)
        {
            AudioManager.Instance?.PlaySFX(uninterractableAction);
            return;
        }
        index++;
        AudioManager.Instance?.PlaySFX(interactableAction);
        CreditsContainer.DOAnchorPosX(-(index * multiplierMove), 1.3f).SetEase(Ease.OutBack);
    }
    private void Previouspage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (index <= 0)
        {
            AudioManager.Instance?.PlaySFX(uninterractableAction);
            return;
        }
        index--;
        AudioManager.Instance?.PlaySFX(interactableAction);
        CreditsContainer.DOAnchorPosX((index * multiplierMove), 1.3f).SetEase(Ease.OutBack);
    }
    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        AudioManager.Instance?.StopMusic(0.4f);
        GameManager.Instance?.LoadScene(0);
    }

    

    
}
