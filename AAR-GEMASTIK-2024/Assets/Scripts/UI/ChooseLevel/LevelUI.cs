using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour, IDataPersistance
{
    [SerializeField] private int levelIndex;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private LevelData levelData;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Image lockedBackground;
    [SerializeField] private Image OnFocusBackground;

    private ChooseSubLevelUI chooseSubLevelUI;

    private void Awake()
    {
        chooseSubLevelUI = FindAnyObjectByType<ChooseSubLevelUI>();
    }
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
        levelData = DataManager.instance.gameData.GetLevelData($"Level {levelIndex}_null");
        button.interactable = levelData.hasBeenUnlocked;
        lockedBackground.gameObject.SetActive(!button.interactable);
        if (levelData.hasBeenUnlocked)
        {
            int langkah = levelData.GetBanyakLangkah();
            string langkah_string = langkah > 0 ? langkah.ToString() : "Belum Mulai";
            description.text += langkah_string;
        }
        /*trashProgress.text = $"{levelData.trashCountDone.ToString()}";
        sharkProgress.text = $"{levelData.sharkMutatedCountDone.ToString()}";
        fishProgress.text = $"{levelData.fishNeededHelpCountDone.ToString()}";
        string format = levelData.progress > 0 ? "0.00" : "0";
        Debug.Log((levelData.progress *100).ToString());*/
        /*progressOverall.text = $"Proses: {(levelData.progress * 100).ToString(format)}%";*/
    }
    

    public void SaveScene(ref GameData gameData)
    {
        
    }
    private void PlaySound()
    {
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.interractable);
        chooseSubLevelUI.OpenPanel(levelData);
    }
    private void Instance_OnChangeLevelChoice(string levelName)
    {
        OnFocusBackground.gameObject.SetActive(levelName == levelData.levelName);
    }
}
