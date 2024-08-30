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
}
