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
            currentCard.gameObject.SetActive(true);
        }
        foreach (ItemBaseSO currentAbilitySO in shopManager.abilityList)
        {
            currentCard = Instantiate(templateCard, abilityContainer);
            currentCard.gameObject.SetActive(true);
        }
        foreach (ItemBaseSO currentItemSO in shopManager.weaponList)
        {
            currentCard = Instantiate(templateCard, itemContainer);
            currentCard.gameObject.SetActive(true);
        }
    }
}
