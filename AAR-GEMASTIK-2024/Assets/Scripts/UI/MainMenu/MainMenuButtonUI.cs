using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioClip onClickAudio;
    private Image ForeGround;
    private Color originalColor;
    private Color newColor = new Color(47, 202, 255);
    private void Awake()
    {
        Button button = GetComponent<Button>();
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
        Debug.Log("On Pointer Exit");
        ForeGround.color = originalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("On Pointer Enter");
        ForeGround.color = newColor;
    }
}
