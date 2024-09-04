using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI_Focus : MonoBehaviour
{
    public List<Image> focusPanelList;
    
    public Image BackgroundFocusPanel;
    public Image invisbleBlockRay;
    
    public Image MainShopPanel;
    public Image ModeTypeToShopPanel;
    public Image ItemTypeToShopPanel;
    public Image ShipPanel;
    public Image MoneyPanel;

    private void Awake()
    {
        focusPanelList = new List<Image> { MainShopPanel, ModeTypeToShopPanel, ItemTypeToShopPanel, ShipPanel, MoneyPanel };
    }
    private void Start()
    {
        HideAll();
        ConversationManager.OnConversationStarted += OnConversationStarted;
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }
    private void OnConversationEnded()
    {
        HideAll();
        invisbleBlockRay.gameObject.SetActive(false);
    }
    private void OnConversationStarted()
    {
        invisbleBlockRay.gameObject.SetActive(true);
    }
    public void HideFocusPanel()
    {
        foreach (Image item in focusPanelList)
        {
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
}
