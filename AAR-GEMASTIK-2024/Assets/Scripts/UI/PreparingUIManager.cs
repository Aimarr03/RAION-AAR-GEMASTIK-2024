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
    public static event Action<bool> OnChangeUI;
    private void Awake()
    {
        /*for(int index = 0; index < list.Count; index++)
        {
            Debug.Log(currentPageIndex == index);
            if (currentPageIndex == index) 
            { 
                headerText.text = headerList[currentPageIndex];
                list[index].gameObject.SetActive(true); 
            }
            else list[index].gameObject.SetActive(false);
        }*/
        OnChangeUI?.Invoke(false);
        CheckButtonCondition();
    }
    private void Start()
    {
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnDisplay);
    }
    private void CheckButtonCondition()
    {
        bool isLastPage = false;
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
            isLastPage = true;
        }
        else
        {
            previousButtonColor.a = 1f;
            nextButtonColor.a = 1f;
            previousButton.GetComponent<Button>().interactable = true;
            nextButton.GetComponent<Button>().interactable = true;
        }
        OnChangeUI?.Invoke(isLastPage);
        previousButton.color = previousButtonColor;
        nextButton.color = nextButtonColor;
        headerText.text = headerList[currentPageIndex];
    }
    public void NextPage()
    {
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.interractable);
        StartCoroutine(OnTransitionNext());
        currentPageIndex++;
        if(currentPageIndex > list.Count - 1)
        {
            currentPageIndex = 0;
        }
        CheckButtonCondition();
    }
    public void PreviousPage()
    {
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.interractable);
        StartCoroutine(OnTransitionPrevious());
        currentPageIndex--;
        if (currentPageIndex < 0)
        {
            currentPageIndex = list.Count-1;
        }
        CheckButtonCondition();
    }
    private IEnumerator OnTransitionNext()
    {
        yield return list[currentPageIndex].OnExitState();
        StartCoroutine(list[currentPageIndex].OnEnterState());
    }
    private IEnumerator OnTransitionPrevious()
    {
        yield return list[currentPageIndex].OnExitState();
        StartCoroutine(list[currentPageIndex].OnEnterState());
    }
}
