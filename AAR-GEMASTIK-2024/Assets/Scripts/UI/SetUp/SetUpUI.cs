using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpUI : BasePreparingPlayerUI
{
    [SerializeField] private Transform templateCard;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private Transform weaponContainer, abilityContainer, itemContainer, StartLevel;
    [SerializeField] private List<Transform> containerList;
    [SerializeField] private Button button;
    [Header("Audio"),SerializeField] private AudioClip StartGame;
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
        
    }
    private void Start()
    {
        SetUpCard.onChoseItem += SetUpCard_onChoseItem;
        GameManager.Instance.OnChangeLevelChoice += Instance_OnChangeLevelChoice;
        SetUp();
        StartLevel.GetChild(0).GetComponent<Button>().onClick.AddListener(OnStartLevel);
    }
    private void OnStartLevel()
    {
        if (GameManager.Instance.CanStartGame() && GameManager.Instance.currentLevelChoice != "")
        {
            AudioManager.Instance.PlaySFX(StartGame);
            GameManager.Instance.LoadLevel();
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioContainerUI.instance.uninterractable);
        }
        
    }
    private void OnDestroy()
    {
        SetUpCard.onChoseItem -= SetUpCard_onChoseItem;
        GameManager.Instance.OnChangeLevelChoice -= Instance_OnChangeLevelChoice;
    }

    private void SetUpCard_onChoseItem(ItemBaseSO itemSO)
    {
        SetFocusUI(itemSO);
    }
    private void Instance_OnChangeLevelChoice(string obj)
    {
        button.interactable = GameManager.Instance.currentLevelChoice != null;
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
            Debug.Log(currentWeaponSO.generalData.unlocked);
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
        if (GameManager.Instance.CanStartGame()) return;
        GameManager.Instance.LoadScene(1);
        DataManager.instance.SaveGame();
    }

    public override IEnumerator OnEnterState()
    {
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnDisplay);
        GameManager.Instance.OnEnteredSetUpLevel();
        foreach (Transform currentContainer in containerList)
        {
            AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnPop);
            currentContainer.gameObject.SetActive(true);
            currentContainer.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.3f);
        }
        StartLevel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.7f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.5f);
        if (!DataManager.instance.gameData.tutorialShopDone) UI_ConversationManager.Instance.PlaySetUpConversation();
    }

    public override IEnumerator OnExitState()
    {
        foreach (Transform currentContainer in containerList)
        {
            AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnPop);
            currentContainer.DOScale(0, 0.5f).SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(0.3f);
        }
        StartLevel.GetComponent<RectTransform>().DOAnchorPosY(-300, 0.7f).SetEase(Ease.InBounce);
        transform.DOScale(0, 0.5f).SetEase(Ease.OutBounce);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnHide);
        yield return new WaitForSeconds(0.5f);
    }
}
