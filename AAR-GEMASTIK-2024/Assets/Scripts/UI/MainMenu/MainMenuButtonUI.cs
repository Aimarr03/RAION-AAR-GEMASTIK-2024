using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonUI : MonoBehaviour
{
    [SerializeField] AudioClip onClickAudio;
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        AudioManager.Instance.PlaySFX(onClickAudio);
    }
}
