using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SustainabilityShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textInfo;
    public SustainabilitySystemSO sustainabilitySO;

    private void Start()
    {
        DetailedCardView.OnUpgradedSomething += DetailedCardView_OnUpgradedSomething;
        SetTextInfo();
    }

    private void DetailedCardView_OnUpgradedSomething()
    {
        SetTextInfo();
    }

    private void SetTextInfo()
    {
        int value = sustainabilitySO.maxValueTimesLevel;
        textInfo.text = $"{value}/{value}";
    }
}
