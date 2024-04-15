using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingUIManager : MonoBehaviour
{
    [SerializeField] private List<BasePreparingPlayerUI> list;
    private int currentPageIndex = 0;

    private void Awake()
    {
        for(int index = 0; index < list.Count; index++)
        {
            Debug.Log(currentPageIndex == index);
            if(currentPageIndex == index) list[index].gameObject.SetActive(true);
            else list[index].gameObject.SetActive(false);
        }
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
    }
}
