using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreparingUIManager : MonoBehaviour
{
    [SerializeField] private List<BasePreparingPlayerUI> list;
    [SerializeField] private List<string> headerList;

    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private Image previousButton;
    [SerializeField] private Image nextButton;
    private int currentPageIndex = 0;

    private void Awake()
    {
        for(int index = 0; index < list.Count; index++)
        {
            Debug.Log(currentPageIndex == index);
            if (currentPageIndex == index) 
            { 
                headerText.text = headerList[currentPageIndex];
                list[index].gameObject.SetActive(true); 
            }
            else list[index].gameObject.SetActive(false);
        }
        CheckButtonCondition();
    }
    private void CheckButtonCondition()
    {
        Color previousButtonColor = previousButton.color;
        Color nextButtonColor = nextButton.color;
        if (currentPageIndex == 0)
        {
            previousButtonColor.a = 0.3f;
            nextButtonColor.a = 1f;
            previousButton.GetComponent<Button>().interactable = false;
            nextButton.GetComponent<Button>().interactable = true;
        }
        else if (currentPageIndex == list.Count - 1)
        {
            previousButtonColor.a = 1f;
            nextButtonColor.a = 0.3f;
            previousButton.GetComponent<Button>().interactable = true;
            nextButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            previousButtonColor.a = 1f;
            nextButtonColor.a = 1f;
            previousButton.GetComponent<Button>().interactable = true;
            nextButton.GetComponent<Button>().interactable = true;
        }
        previousButton.color = previousButtonColor;
        nextButton.color = nextButtonColor;
        headerText.text = headerList[currentPageIndex];
    }
    public void NextPage()
    {
        BasePreparingPlayerUI currentPage = list[currentPageIndex];
        currentPageIndex++;
        if(currentPageIndex > list.Count - 1)
        {
            currentPageIndex = 0;
        }
        BasePreparingPlayerUI nextPage = list[currentPageIndex];
        currentPage.gameObject.SetActive(false);
        nextPage.gameObject.SetActive(true);
        CheckButtonCondition();
    }
    public void PreviousPage()
    {
        BasePreparingPlayerUI currentPage = list[currentPageIndex];
        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = list.Count-1;
        }
        BasePreparingPlayerUI previouosLayer = list[currentPageIndex];
        currentPage.gameObject.SetActive(false);
        previouosLayer.gameObject.SetActive(true);
        CheckButtonCondition();
    }
}
