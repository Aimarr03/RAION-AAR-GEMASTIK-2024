using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGameplayUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image cooldownIcon;
    [SerializeField] Image background;
    [SerializeField] Color focusedColor;
    Color defaultColor;
    public SustainabilityType type;
    public TextMeshProUGUI quantity;
    ConsumableItemSO itemData;
    public void SetUp(ConsumableItemSO itemData)
    {
        /*if (itemData.generalData.icon != null)
        {
            icon.sprite = itemData.generalData.icon;
            cooldownIcon.sprite = icon.sprite;
        }*/
        this.itemData = itemData;
        quantity.text = itemData.quantity.ToString();
        defaultColor = background.color;
    }

    public void StartCooldown(float duration)
    {
        StartCoroutine(OnUICooldown(duration));
    }
    private IEnumerator OnUICooldown(float duration)
    {
        float currentDuration = 0;
        quantity.text = itemData.quantity.ToString();
        icon.fillAmount = 0;
        while(currentDuration < duration)
        {
            currentDuration += Time.deltaTime;
            icon.fillAmount = (currentDuration / duration);
            yield return null;
        }
    }
    public void OnFocusIcon()
    {
        background.color = focusedColor;
    }
    public void OnNotFocusedIcon()
    {
        background.color = defaultColor;
    }
}
