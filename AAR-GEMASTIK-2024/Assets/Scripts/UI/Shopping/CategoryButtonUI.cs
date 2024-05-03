using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonUI : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    private Button buttonCategory;
    public static event Action<CategoryButtonUI> onClick;
    private void Awake()
    {
        buttonCategory = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }
    private void Start()
    {
        onClick += CategoryButtonUI_onClick;
        buttonCategory.onClick.AddListener(OnClickButton);
    }
    private void OnDestroy()
    {
        onClick -= CategoryButtonUI_onClick;
    }

    private void CategoryButtonUI_onClick(CategoryButtonUI obj)
    {
        Color buttonColor = buttonImage.color;
        if (obj != this)
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
    private void OnMouseEnter()
    {
        Debug.Log("Mouse enter on " + gameObject.name);
        Color buttonColor = buttonImage.color;
        buttonColor.a = 0.5f;
        buttonImage.color = buttonColor;
    }
    private void OnClickButton()
    {
        onClick?.Invoke(this);
    }
}
