using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubLevelUI : MonoBehaviour
{
    private Button button;
    public TextMeshProUGUI TextLevel;
    public SubLevelData subLevelData;
    public SubLevelDescriptionSO subLevelDescription;
    public ChooseSubLevelUI choice;
    public AudioClip audioClicked;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SetUpSubLevelData(SubLevelData subLevelData, ChooseSubLevelUI choice, SubLevelDescriptionSO description)
    {
        this.subLevelData = subLevelData;
        this.choice = choice;
        subLevelDescription = description;
        TextLevel.text = subLevelData.subLevelName;
        button.onClick.AddListener(OnClick);
        Debug.Log("On Set Up Data " + subLevelData.subLevelName);
    }
    private void OnClick()
    {
        choice.ShowDataSubLevel(subLevelData, subLevelDescription);
        AudioManager.Instance.PlaySFX(audioClicked);
    }

}
