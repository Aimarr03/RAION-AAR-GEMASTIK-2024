using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI_Focus : MonoBehaviour
{
    public List<Image> focusPanelList;

    [Header("Main Panel")]
    public Image BackgroundFocusPanel;
    public Image invisbleBlockRay;

    [Header("Shop Focus Panel")]
    public Image MainShopPanel;
    public Image ModeTypeToShopPanel;
    public Image ItemTypeToShopPanel;
    public Image ShipPanel;
    public Image MoneyPanel;

    [Header("Choose Level Panel")]
    public Image LevelChoicePanel;
    public Image CurrentLevelPanel;
    public Image SubLevelChoice;
    public Image SubLevelList;
    public Image SubLevelDescription;
    public Image SubLevelProgress;
    public Image SubLevelConfirm;

    [Header("Set Up Focus")]
    public Image StartFocus;
    public Image ItemFocus;
    public Image ItemChosenFocus;
    private void Awake()
    {
        focusPanelList = new List<Image> { MainShopPanel, ModeTypeToShopPanel, ItemTypeToShopPanel, ShipPanel, MoneyPanel,
        LevelChoicePanel, CurrentLevelPanel, SubLevelChoice, SubLevelList, SubLevelDescription, SubLevelProgress, SubLevelConfirm
        , StartFocus, ItemFocus, ItemChosenFocus};
    }
    private void Start()
    {
        ConversationManager.OnConversationStarted += OnConversationStarted;
        ConversationManager.OnConversationEnded += OnConversationEnded;
        OnConversationEnded();
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= OnConversationStarted;
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }
    private void OnConversationEnded()
    {
        HideAll();
        invisbleBlockRay.gameObject.SetActive(false);
    }
    private void OnConversationStarted()
    {
        Debug.Log("Conversation Started going to make beside dialogue panel uninterractable");
        invisbleBlockRay.gameObject.SetActive(true);
    }
    public void HideFocusPanel()
    {

        foreach (Image item in focusPanelList)
        {
            //Debug.Log(item.gameObject);
            item.gameObject.SetActive(false);
        }
    }
    public void ShowMainShopPanel()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        MainShopPanel.gameObject.SetActive(true);
    }
    public void ShowModeTypeToShopPanel()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        ModeTypeToShopPanel.gameObject.SetActive(true);
    }
    public void ShowItemTypeToShopPanel()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        ItemTypeToShopPanel.gameObject.SetActive(true);
    }
    public void ShowShipPanel()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        ShipPanel.gameObject.SetActive(true);
    }
    public void ShowMoneyPanel()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        MoneyPanel.gameObject.SetActive(true);
    }
    public void HideAll()
    {
        BackgroundFocusPanel.gameObject.SetActive(false);
        HideFocusPanel();
    }
    public void ShowLevelChoice()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        LevelChoicePanel.gameObject.SetActive(true);
    }
    public void ShowCurrentLevel()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        CurrentLevelPanel.gameObject.SetActive(true);
    }
    public void ShowSubLevelChoice()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        SubLevelChoice.gameObject.SetActive(true);
    }
    public void ShowSubLevelList()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        SubLevelList.gameObject.SetActive(true);
    }
    public void ShowSubLevelDescription()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        SubLevelDescription.gameObject.SetActive(true);
    }
    public void ShowSubLevelProgress()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        SubLevelProgress.gameObject.SetActive(true);
    }
    public void ShowSubLevelConfirm()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        SubLevelConfirm.gameObject.SetActive(true);
    }
    public void ShowStartLevelFocus()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        StartFocus.gameObject.SetActive(true);
    }
    public void ShowItemFocus()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        ItemFocus.gameObject.SetActive(true);
    }
    public void ShowActiveItemFocus()
    {
        HideFocusPanel();
        BackgroundFocusPanel.gameObject.SetActive(true);
        ItemChosenFocus.gameObject.SetActive(true);
    }
}
