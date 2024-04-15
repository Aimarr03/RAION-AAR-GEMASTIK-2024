using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public enum ItemType
{
    Sustainabillity, Weapon, Ability, Item
}
public enum ShopMode
{
    Buy, Upgrade
}
public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform CardContainer;
    [SerializeField] private Transform templateContainer;
    [SerializeField] private ShopMode shopMode;
    private List<ItemBaseSO> list;
    private List<ShopItemCard> itemCards;
    private void Awake()
    {
        templateContainer.gameObject.SetActive(false);
        list = new List<ItemBaseSO>();
        itemCards = new List<ShopItemCard>();
    }
    private void Start()
    {
        DisplayItem(ItemType.Weapon);
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
            currentCard.gameObject.SetActive(true);
            ShopItemCard detailItemCard = currentCard.GetComponent<ShopItemCard>();
            itemCards.Add(detailItemCard);
            detailItemCard.header.text = item.generalData.name;
        }
    }
    public void BuyModeAction() => shopMode = ShopMode.Buy;
    public void UpgradeModeAction() => shopMode = ShopMode.Upgrade;
    public void SortByMode()
    {
        switch(shopMode)
        {
            case ShopMode.Buy:
                ShowBuyable();
                break;
            case ShopMode.Upgrade:
                ShowUpgradable();
                break;
        }
    }
    public void ShowUpgradable() => list.Sort((item1, item2) => item1.UpgradeModeComparison(item2));
    public void ShowBuyable() => list.Sort((item1, item2) => item1.BuyModeComparison(item2));
    public void DisplayWeapon() => DisplayItem(ItemType.Weapon);
    public void DisplayAbility() => DisplayItem(ItemType.Ability);
    public void DisplayItem() => DisplayItem(ItemType.Item);
    public void DisplaySustainability() => DisplayItem(ItemType.Sustainabillity);
    
}
