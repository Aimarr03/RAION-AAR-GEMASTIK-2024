using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class UI_TrashCore : MonoBehaviour
{
    [SerializeField] TrashCore trashCore;
    [SerializeField] private RectTransform UI_ProcessToTake;
    [SerializeField] private RectTransform UI_CanBeTaken;

    [SerializeField] private Image processValue;
    [SerializeField] private Image indicationValue;
    public void OnSwitchingUI(bool canBeTaken)
    {
        UI_ProcessToTake.gameObject.SetActive(!canBeTaken);
        UI_CanBeTaken.gameObject.SetActive(canBeTaken);
    }
    public void OnEnableView(PlayerCoreSystem player)
    {
        bool IsPlayerNull = player != null;
        UI_ProcessToTake.gameObject.SetActive(IsPlayerNull);
        UI_CanBeTaken.gameObject.SetActive(IsPlayerNull);
    }
    public void SetProcessLoadingValue(float value)
    {
        processValue.fillAmount = value;
    }
    public void SetIndicationValue(float minValue, float maxValue)
    {
        indicationValue.fillAmount = maxValue;
        float z_rotation = -(360 * minValue);
        indicationValue.rectTransform.localRotation = Quaternion.Euler(0,0,z_rotation);
    }
}
