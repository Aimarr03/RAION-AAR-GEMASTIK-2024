using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayUIVisualHelperButton : MonoBehaviour
{
    public PauseGameplayUI pauseUIScript;
    [SerializeField] private Color notFocusedColor;
    [SerializeField] private Color FocusedColor;

    public UnityEvent OnPerformed;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void OnFoccusedButton()
    {
        image.color = FocusedColor;
    }
    public void OnNotFoccusedButton()
    {
        image.color = notFocusedColor;
    }
    public void OnFocusedButtonPerformed()
    {
        OnPerformed?.Invoke();
    }
    private void Start()
    {
        
    }
    private void OnDisable()
    {
        
    }
}
