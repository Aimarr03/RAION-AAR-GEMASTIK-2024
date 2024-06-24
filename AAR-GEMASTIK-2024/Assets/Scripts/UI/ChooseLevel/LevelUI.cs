using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour, IDataPersistance
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI trashProgress;
    [SerializeField] private TextMeshProUGUI sharkProgress;
    [SerializeField] private TextMeshProUGUI fishProgress;
    [SerializeField] private Image image;
    [SerializeField] private Image lockedBackground;
    [SerializeField] private Image OnFocusBackground;

    private void Start()
    {
        button.onClick.AddListener(PlaySound);
        OnFocusBackground.gameObject.SetActive(false);
        GameManager.Instance.OnChangeLevelChoice += Instance_OnChangeLevelChoice;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnChangeLevelChoice -= Instance_OnChangeLevelChoice;
    }
    public void LoadScene(GameData gameData)
    {
        Debug.Log("Load Data For Level");
        LevelData levelData = DataManager.instance.gameData.GetLevelData(levelIndex);
        button.interactable = levelData.hasBeenUnlocked;
        lockedBackground.gameObject.SetActive(!button.interactable);
        trashProgress.text = $"{levelData.trashProgress.ToString("0")}%";
        sharkProgress.text = $"{levelData.sharkMutatedProgress.ToString("0")}%";
        fishProgress.text = $"{levelData.fishNeededHelpProgress.ToString("0")}%";
    }

    public void SaveScene(ref GameData gameData)
    {
        
    }
    private void PlaySound()
    {
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.interractable);
    }
    private void Instance_OnChangeLevelChoice(int index)
    {
        OnFocusBackground.gameObject.SetActive(index == levelIndex);
    }
}
