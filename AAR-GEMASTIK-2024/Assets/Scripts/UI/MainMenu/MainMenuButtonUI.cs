using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioClip onClickAudio;
    public Button button;
    private Image ForeGround;
    private Color originalColor;
    private Color newColor = new Color(47, 202, 255);
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        ForeGround = transform.GetChild(0).GetComponent<Image>();
        originalColor = ForeGround.color;
    }
    public void OnClick()
    {
        AudioManager.Instance.PlaySFX(onClickAudio);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ForeGround.color = originalColor;
    }
    
    public void OnNotFocused()
    {
        ForeGround.color = originalColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ForeGround.color = newColor;
    }
    public void OnFocused()
    {
        ForeGround.color = newColor;
    }
}
