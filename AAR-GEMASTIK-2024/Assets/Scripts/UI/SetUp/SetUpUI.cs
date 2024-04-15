using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpUI : BasePreparingPlayerUI
{
    [SerializeField] private Transform templateCard;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform weaponContainer, abilityContainer, itemContainer;

    private void Awake()
    {
        SetUp();
    }
    private void SetUp()
    {
        Transform currentCard;
        foreach(ItemBaseSO currentWeaponSO in shopManager.weaponList)
        {
            currentCard = Instantiate(templateCard, weaponContainer);
            SetUpCard cardData = currentCard.GetComponent<SetUpCard>();
            cardData.SetUpData(currentWeaponSO, ItemType.Weapon);
            currentCard.gameObject.SetActive(true);
        }
        foreach (ItemBaseSO currentAbilitySO in shopManager.abilityList)
        {
            currentCard = Instantiate(templateCard, abilityContainer);
            SetUpCard cardData = currentCard.GetComponent<SetUpCard>();
            cardData.SetUpData(currentAbilitySO, ItemType.Ability);
            currentCard.gameObject.SetActive(true);
        }
        foreach (ItemBaseSO currentItemSO in shopManager.itemList)
        {
            currentCard = Instantiate(templateCard, itemContainer);
            SetUpCard cardData = currentCard.GetComponent<SetUpCard>();
            cardData.SetUpData(currentItemSO, ItemType.Item);
            currentCard.gameObject.SetActive(true);
        }
    }
    public void StartExpedition()
    {
        if (GameManager.Instance.chosenWeaponSO == null) return;
        GameManager.Instance.LoadScene(1);
    }
}
