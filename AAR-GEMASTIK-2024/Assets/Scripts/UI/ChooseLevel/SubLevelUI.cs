using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubLevelUI : MonoBehaviour
{
    private Button button;
    public SubLevelData subLevelData;
    public ChooseSubLevelUI choice;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SetUpSubLevelData(SubLevelData subLevelData, ChooseSubLevelUI choice)
    {
        this.subLevelData = subLevelData;
        this.choice = choice;
        button.onClick.AddListener(OnClick);
        Debug.Log("On Set Up Data " + subLevelData.subLevelName);
    }
    private void OnClick()
    {
        choice.ShowDataSubLevel(subLevelData);
    }

}
