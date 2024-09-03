using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FocusPanelUI : MonoBehaviour
{
    [Header("Main Panel")]
    public List<FocusPanelImage> focusPanelList;
    public Image backgroundFocusPanel;
    public RectTransform gameplayPanel;

    [Header("Specific Panel")]
    public FocusPanelImage healthPanel;
    public FocusPanelImage TubePanel;
    public FocusPanelImage NavigationPanel;
    public FocusPanelImage weaponPanel;
    public FocusPanelImage abilityPanel;
    private void Awake()
    {
        focusPanelList = new List<FocusPanelImage>
        {
            healthPanel,
            TubePanel,
            NavigationPanel,
            weaponPanel,
            abilityPanel
        };
        ConversationManager.OnConversationStarted += ConversationStart;
        ConversationManager.OnConversationEnded += ConversationFinish;
    }
    private void Start()
    {
        
        //ConversationStart();
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= ConversationStart;
        ConversationManager.OnConversationEnded -= ConversationFinish;
    }
    private void ConversationStart()
    {
        gameplayPanel.gameObject.SetActive(false);
        Debug.Log("Gameplay Panel set to false");
    }
    private void ConversationFinish()
    {
        gameplayPanel.gameObject.SetActive(true);
        ShowAllPanel();
    }
    public void ShowPanel()
    {
        //await Task.Delay(800);
        gameplayPanel.gameObject.SetActive(true);
        backgroundFocusPanel.gameObject.SetActive(true);
    }
    public void ShowHealth()
    {
        HidePanel();
        healthPanel.ShowPanel(true);
        ShowPanel();
    }
    public void ShowTube()
    {
        HidePanel();
        TubePanel.ShowPanel(true);
        ShowPanel();
    }
    public void ShowNavigation()
    {
        HidePanel();
        NavigationPanel.ShowPanel(true);
        ShowPanel();
    }
    public void ShowWeapon()
    {
        HidePanel();
        weaponPanel.ShowPanel(true);
        ShowPanel();
    }
    public void ShowAbility()
    {
        HidePanel();
        abilityPanel.ShowPanel(true);
        ShowPanel();
    }
    public void HidePanel()
    {
        backgroundFocusPanel.gameObject.SetActive(false);
        foreach (FocusPanelImage image in focusPanelList)
        {
            image.ShowPanel(false);
        }
    }
    public void ShowAllPanel()
    {
        foreach (FocusPanelImage image in focusPanelList)
        {
            image.ShowPanel(true);
        }
    }
}
[Serializable]
public class FocusPanelImage
{
    public RectTransform theImage;
    public RectTransform focusPanel;
    public void ShowPanel(bool input)
    {
        theImage.gameObject.SetActive(input);
        focusPanel.gameObject.SetActive(input);
    }
}
