using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RebuildPlant : MonoBehaviour
{
    [SerializeField] private RectTransform Canvas_ToStart;
    [SerializeField] private RectTransform Canvas_Processing;
    [SerializeField] private Image image;
    [SerializeField] private Image focusImage;

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
    public void OnSetValue(float minValue, float maxValue)
    {
        focusImage.fillAmount = maxValue;
        float z_rotation = -(360 * minValue);
        focusImage.rectTransform.localRotation = Quaternion.Euler(0, 0, z_rotation);
    }
}

