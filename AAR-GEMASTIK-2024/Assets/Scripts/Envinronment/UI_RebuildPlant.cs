using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RebuildPlant : MonoBehaviour
{
    [SerializeField] private RectTransform Canvas_ToStart;
    [SerializeField] private RectTransform Canvas_Processing;
    [SerializeField] private Image image;

    private void Awake()
    {
        Canvas_Processing.gameObject.SetActive(false);
        Canvas_ToStart.gameObject.SetActive(false);
    }
    public void SwitchCanvasToProcess(bool processing)
    {
        Canvas_ToStart.gameObject.SetActive(!processing);
        Canvas_Processing.gameObject.SetActive(processing);
    }
    public void ToggleAllCanvas(bool input)
    {
        Canvas_ToStart.gameObject.SetActive(input);
        Canvas_Processing.gameObject.SetActive(input);
    }
    public void ShowProcess(float process)
    {
        image.fillAmount = process;
    }
}

