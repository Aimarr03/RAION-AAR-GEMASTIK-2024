using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChosenSetUpUI : MonoBehaviour
{
    [SerializeField] private Image chosenImage;
    [SerializeField] private ItemType itemType;
    [SerializeField] private SustainabilityType sustainabilityType;
    void Start()
    {
        SetUpCard.onChoseItem += SetUpCard_onChoseItem;
    }
    private void OnDisable()
    {
        SetUpCard.onChoseItem -= SetUpCard_onChoseItem;
    }
    private void SetUpCard_onChoseItem(ItemBaseSO obj)
    {
        if (obj.generalData.itemType == itemType)
        {
            if (obj.generalData.itemType == ItemType.Item && obj is ConsumableItemSO consumableItemSO)
            {
                Debug.Log(obj.generalData.itemType);
                Debug.Log(consumableItemSO.type);
                Debug.Log(sustainabilityType);
                if (consumableItemSO.type == sustainabilityType)
                {
                    chosenImage.sprite = consumableItemSO.generalData.icon;
                    chosenImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = consumableItemSO.itemTier.ToString();
                    return;
                }
            }
            else
            {
                chosenImage.sprite = obj.generalData.icon;
            }
        }
    }


}
