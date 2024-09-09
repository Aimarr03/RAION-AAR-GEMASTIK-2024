using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSubLevelUI : MonoBehaviour
{
    [SerializeField] private PreparingUIManager UI_SHOP_MANAGER;
    [SerializeField] private SerializableDictionary<string, SubLevelDescriptionSO> DictionarySubLevelDescription;
    private LevelData levelData;
    [Header("MainPanel"), SerializeField]
    private RectTransform backgroundPanel;
    [SerializeField] private RectTransform mainPanel;

    [Header("Sub Level Container")]
    [SerializeField] private RectTransform SubLevelContainer;
    [SerializeField] private SubLevelUI SubLevelTemplate;
    [SerializeField] private Color unlockedColor;

    [Header("Detailed Sub Level Container")]
    [SerializeField] private RectTransform DetailedDataSubLevelContainer;
    [SerializeField] private TextMeshProUGUI SubLevel_Code;
    [SerializeField] private TextMeshProUGUI SubLevel_Name;
    [SerializeField] private TextMeshProUGUI SubLevel_Phase;
    [SerializeField] private TextMeshProUGUI SubLevel_Description;
    [SerializeField] private TextMeshProUGUI SubLevel_FishProgress;
    [SerializeField] private TextMeshProUGUI SubLevel_TrashProgress;

    [Header("Button To Choose")]
    [SerializeField] private Button footerButton;
    [SerializeField] private ChooseLevel chooseLevel;
    public AudioClip StartLevel;
    private SubLevelData currentChoiceSubLevelData;
    private void Awake()
    {
        footerButton.onClick.AddListener(OnChooseLevel);
        SubLevelTemplate.gameObject.SetActive(false);
        OnHidePanel();
    }
    public void OpenPanel(LevelData levelData)
    {
        backgroundPanel.gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(true);
        if(SubLevelContainer.childCount > 0)
        {
            for(int index = 0; index <  SubLevelContainer.childCount; index++)
            {
                Transform currentSubLevel = SubLevelContainer.transform.GetChild(index);
                if (currentSubLevel == SubLevelTemplate.GetComponent<Transform>()) continue;
                Destroy(currentSubLevel.gameObject);
            }
        }
        this.levelData = levelData;
        DetailedDataSubLevelContainer.gameObject.SetActive(false);
        for(int i = 0; i < levelData.subLevels.Count; i++)
        {
            SubLevelUI currentSubLevelUI = Instantiate(SubLevelTemplate, SubLevelContainer);
            SubLevelData SubLevelData = levelData.subLevels[i];
            currentSubLevelUI.gameObject.SetActive(true);
            if (SubLevelData.isUnlocked)
            {
                currentSubLevelUI.GetComponent<Image>().color = unlockedColor;
                currentSubLevelUI.GetComponent<Button>().interactable = SubLevelData.isUnlocked;
                Color textColor = currentSubLevelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
                textColor.a = 1;
                currentSubLevelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;
            }
            if (DictionarySubLevelDescription.ContainsKey(SubLevelData.subLevelName))
            {
                SubLevelDescriptionSO levelDescription = DictionarySubLevelDescription[SubLevelData.subLevelName];
                currentSubLevelUI.SetUpSubLevelData(SubLevelData, this, levelDescription);
            }
            else
            {
                currentSubLevelUI.TextLevel.text = "Terkunci";
            }
        }
    }
    public void ShowDataSubLevel(SubLevelData SubLevelData, SubLevelDescriptionSO levelDescription)
    {
        DetailedDataSubLevelContainer.gameObject.SetActive(true);
        footerButton.interactable = true;
        currentChoiceSubLevelData = SubLevelData;
        SubLevel_Code.text = SubLevelData.subLevelName;
        SubLevel_Name.text = levelDescription == null ? "" : levelDescription.subLevelName;
        string fase = SubLevelData.currentCompletedPhase == 0 ? "Belum Mulai" : SubLevelData.currentCompletedPhase.ToString();
        SubLevel_Phase.text = $"Fase: {fase}";
        SubLevel_Description.text = levelDescription == null ? "": levelDescription.description ;
        SubLevel_FishProgress.text = $": {SubLevelData.fishNeededHelpCountDone}";
        SubLevel_TrashProgress.text = $": {SubLevelData.trashCountDone}";

    }
    private async void OnChooseLevel()
    {
        GameManager.Instance.SetLevel(currentChoiceSubLevelData.subLevelName);
        AudioManager.Instance?.PlaySFX(StartLevel);
        OnHidePanel();
        await Task.Delay(50);
        UI_SHOP_MANAGER.NextPage();
    }
    public void OnHidePanel()
    {
        backgroundPanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(false);
        footerButton.interactable = false;
    }
    public void ShowDummyPanel()
    {
        LevelData dummyLevelData= DataManager.instance.gameData.levels[0];
        SubLevelData dummySubLevelData = dummyLevelData.subLevels[0];
        OpenPanel(dummyLevelData);
        ShowDataSubLevel(dummySubLevelData, null);
    }
    
}
