using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform CardContainer;
    [SerializeField] private Transform templateContainer;

    private void Awake()
    {
        templateContainer.gameObject.SetActive(false);
    }
    private void Start()
    {
        DisplayWeapon();
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
    public void DisplayWeapon()
    {
        ClearContainer();
        foreach(WeaponSO weaponSO in shopManager.weaponList)
        {
            Transform currentCard = Instantiate(templateContainer, CardContainer);
            currentCard.gameObject.SetActive(true);
        }
    }
    public void DisplayAbility()
    {
        ClearContainer() ;
        foreach(AbilitySO abilitySO in shopManager.abilityList)
        {
            Transform currentCard = Instantiate(templateContainer, CardContainer);
            currentCard.gameObject.SetActive(true);
        }
    }
    public void DisplayItem()
    {
        ClearContainer();
        foreach(ConsumableItemSO itemSO in shopManager.itemList)
        {
            Transform currentCard = Instantiate(templateContainer, CardContainer);
            currentCard.gameObject.SetActive(true);
        }
    }

}
