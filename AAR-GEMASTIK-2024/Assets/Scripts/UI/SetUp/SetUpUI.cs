using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpUI : BasePreparingPlayerUI
{
    [SerializeField] private Transform templateCard;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform weaponContainer, abilityContainer, itemContainer;
    [SerializeField] private List<Transform> containerList;
    private List<SetUpCard> weaponList;
    private List<SetUpCard> abilityList;
    private List<SetUpCard> itemList;
    
    private void Awake()
    {
        weaponList = new List<SetUpCard>();
        abilityList = new List<SetUpCard>();
        itemList = new List<SetUpCard>();
        containerList = new List<Transform> { weaponContainer, abilityContainer, itemContainer };
        transform.localScale = Vector3.zero;
        foreach(Transform t in containerList)
        {
            t.localScale = Vector3.zero;
        }
        SetUp();
    }
    private void Start()
    {
        SetUpCard.onChoseItem += SetUpCard_onChoseItem;
    }
    private void OnDestroy()
    {
        SetUpCard.onChoseItem -= SetUpCard_onChoseItem;
    }

    private void SetUpCard_onChoseItem(ItemBaseSO itemSO)
    {
        SetFocusUI(itemSO);
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
            weaponList.Add(cardData);
        }
        foreach (ItemBaseSO currentAbilitySO in shopManager.abilityList)
        {
            currentCard = Instantiate(templateCard, abilityContainer);
            SetUpCard cardData = currentCard.GetComponent<SetUpCard>();
            cardData.SetUpData(currentAbilitySO, ItemType.Ability);
            currentCard.gameObject.SetActive(true);
            abilityList.Add(cardData);
        }
        foreach (ItemBaseSO currentItemSO in shopManager.itemList)
        {
            currentCard = Instantiate(templateCard, itemContainer);
            SetUpCard cardData = currentCard.GetComponent<SetUpCard>();
            cardData.SetUpData(currentItemSO, ItemType.Item);
            currentCard.gameObject.SetActive(true);
            itemList.Add(cardData);
        }
    }
    public void SetFocusUI(ItemBaseSO itemSO)
    {
        List<SetUpCard> cardList = new List<SetUpCard>();
        switch (itemSO)
        {
            case WeaponSO: cardList = weaponList; break;
            case AbilitySO: cardList=abilityList; break;
            case ConsumableItemSO: cardList = itemList; break;
        }
        foreach(SetUpCard card in cardList)
        {
            if(itemSO is ConsumableItemSO)
            {
                ConsumableItemFocusBackground(itemSO, card);
            }
            else
            {
                NotConsumableItemFocusBackground(itemSO, card);
            }
            
        }
    }
    private void NotConsumableItemFocusBackground(ItemBaseSO itemSO, SetUpCard card)
    {
        if (card.itemBaseSO == itemSO)
        {
            card.backgroundFocus.gameObject.SetActive(true);
            return;
        }
        card.backgroundFocus.gameObject.SetActive(false);
    }
    private void ConsumableItemFocusBackground(ItemBaseSO itemSO, SetUpCard card)
    {
        ConsumableItemSO chosenConsumableItemSO = itemSO as ConsumableItemSO;
        ConsumableItemSO cardConsumableItemSO = card.itemBaseSO as ConsumableItemSO;
        //Debug.Log(card.itemBaseSO == itemSO);
        if (chosenConsumableItemSO.type != cardConsumableItemSO.type) return;
        if (card.itemBaseSO == itemSO)
        {
            card.backgroundFocus.gameObject.SetActive(true);
            return;
        }
        card.backgroundFocus.gameObject.SetActive(false);
    }
    public void StartExpedition()
    {
        if (GameManager.Instance.chosenWeaponSO == null) return;
        GameManager.Instance.LoadScene(1);
    }

    public override IEnumerator OnEnterState()
    {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        foreach(Transform currentContainer in containerList)
        {
            currentContainer.gameObject.SetActive(true);
            currentContainer.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public override IEnumerator OnExitState()
    {
        foreach (Transform currentContainer in containerList)
        {
            currentContainer.DOScale(0, 0.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.3f);
        }
        transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
