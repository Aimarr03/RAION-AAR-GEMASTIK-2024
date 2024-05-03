using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CategoryButtonUI : MonoBehaviour
{
    public UnityEvent unityEvent;
    [SerializeField] private Image buttonImage;
    [SerializeField] private ItemType itemType;
    private Button buttonCategory;
    private void Awake()
    {
        buttonCategory = GetComponent<Button>();
        //buttonImage = GetComponentInChildren<Image>();
    }
    private void Start()
    {
        ShopUI.OnDisplayItem += ShopUI_OnDisplayItem;
    }
    private void ShopUI_OnDisplayItem(ItemType itemType)
    {
        Color buttonColor = buttonImage.color;
        if (itemType != this.itemType)
        {
            buttonColor.a = 0;
            buttonImage.color = buttonColor;
        }
        else
        {
            buttonColor.a = 1;
            buttonImage.color = buttonColor;
        }
    }

    private void OnDestroy()
    {
        ShopUI.OnDisplayItem -= ShopUI_OnDisplayItem;
    }
    private void OnMouseEnter()
    {
        Debug.Log("Mouse enter on " + gameObject.name);
        Color buttonColor = buttonImage.color;
        buttonColor.a = 0.5f;
        buttonImage.color = buttonColor;
    }
    private void OnMouseDown()
    {
        Debug.Log("Testing ANJING");
    }
}
