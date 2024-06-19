using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Sustainabillity, Weapon, Ability, Item
}
public enum ShopMode
{
    Buy, Upgrade
}
public class ShopUI : BasePreparingPlayerUI
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform CardContainer;
    [SerializeField] private Transform templateContainer;
    
    [SerializeField] private DetailedCardView cardDetailedInfo;
    [SerializeField] private TextMeshProUGUI headerText;
    private List<ItemBaseSO> list;
    private List<ShopItemCard> itemCards;
    public static event Action<ItemType> OnDisplayItem;
    public Image buyFocus;
    public Image upgradeFocus;
    private void Awake()
    {
        templateContainer.gameObject.SetActive(false);
        list = new List<ItemBaseSO>();
        itemCards = new List<ShopItemCard>();
    }
    private void Start()
    {
        cardDetailedInfo.gameObject.SetActive(false);
        DisplayItem(ItemType.Weapon);
        headerText.text = "WEAPON";
        DetailedCardView.OnBoughtSomething += DetailedCardView_OnBoughtSomething;
    }

    private void DetailedCardView_OnBoughtSomething()
    {
        SortByMode();
        UpdateSorting();
    }

    private void ClearContainer()
    {
        for(int i = 0; i < CardContainer.childCount; i++)
        {
            Transform currentChild = CardContainer.GetChild(i);
            if (currentChild == templateContainer) continue;
            Destroy(currentChild.gameObject);
        }
    }
    public void DisplayItem(ItemType type)
    {
        ClearContainer();
        switch (type)
        {
            case ItemType.Sustainabillity:
                list = shopManager.sustainabilityList;
                break;
            case ItemType.Weapon:
                list = shopManager.weaponList;
                break;
            case ItemType.Ability:
                list = shopManager.abilityList;
                break;
            case ItemType.Item: 
                list = shopManager.itemList;
                break;
        }
        itemCards.Clear();
        SortByMode();
        foreach (ItemBaseSO item in list)
        {
            Transform currentCard = Instantiate(templateContainer, CardContainer);
            currentCard.localScale = Vector3.zero;
            currentCard.gameObject.SetActive(true);
            currentCard.name = item.generalData.name;
            ShopItemCard detailItemCard = currentCard.GetComponent<ShopItemCard>();
            itemCards.Add(detailItemCard);
            detailItemCard.SetGeneralData(item, ShopManager.instance.shopMode);
        }
        DOScaleTweenItemCard();
        OnDisplayItem?.Invoke(type);
    }
    private async void DOScaleTweenItemCard()
    {
        for(int index = 0; index < itemCards.Count; index++)
        {
            ShopItemCard currentItemCard = itemCards[index];
            if(currentItemCard == null) continue;
            currentItemCard.transform.DOScale(1, 0.3f).SetEase(Ease.OutBounce);
            AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnPop);
            await Task.Delay(150);
        }
    }
    public void BuyModeAction() 
    { 
        shopManager.shopMode = ShopMode.Buy;
        SortByMode();
        UpdateSorting();
    }

    public void UpgradeModeAction()
    {
        shopManager.shopMode = ShopMode.Upgrade;
        SortByMode();
        UpdateSorting();
    }
    private void SortByMode()
    {
        switch(shopManager.shopMode)
        {
            case ShopMode.Buy:
                buyFocus.gameObject.SetActive(true);
                upgradeFocus.gameObject.SetActive(false);
                ShowBuyable();
                
                break;
            case ShopMode.Upgrade:
                buyFocus.gameObject.SetActive(false);
                upgradeFocus.gameObject.SetActive(true);
                ShowUpgradable();
                break;
        }
    }
    private void ShowUpgradable() 
    {
        list.Sort((item1, item2) => item1.BuyModeComparison(item2));
        if (itemCards.Count <= 0) return;
        itemCards.Sort((item1, item2) => item1.UpgradeModeComparison(item2));
    }
    private void ShowBuyable()
    {
        list.Sort((item1, item2) => item1.BuyModeComparison(item2));
        if (itemCards.Count <= 0) return;
        itemCards.Sort((item1, item2) => item1.BuyModeComparison(item2));
    }
    public void DisplayWeapon()
    {
        headerText.text = "WEAPON";
        DisplayItem(ItemType.Weapon);
    }
    public void DisplayAbility() 
    {
        headerText.text = "ABILITY";
        DisplayItem(ItemType.Ability); 
    }
    public void DisplayItem() 
    {
        headerText.text = "ITEM";
        DisplayItem(ItemType.Item); 
    }
    public void DisplaySustainability() 
    {
        headerText.text = "SUSTAINABILITY";
        DisplayItem(ItemType.Sustainabillity);
    } 
    private void UpdateSorting()
    {
        if (itemCards.Count <= 0) return;
        for(int i= 0; i<itemCards.Count; i++)
        {
            ShopItemCard card = itemCards[i];
            Transform currentCardTransform = card.transform;
            card.SetGeneralData(ShopManager.instance.shopMode);
            currentCardTransform.SetSiblingIndex(i);
            
        }
    }

    public override IEnumerator OnEnterState()
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().DOAnchorPosX(0, 0.7f);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnDisplay);
        yield return new WaitForSeconds(0.7f);
    }

    public override IEnumerator OnExitState()
    {
        GetComponent<RectTransform>().DOAnchorPosX(1000, 0.7f);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnHide);
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}
