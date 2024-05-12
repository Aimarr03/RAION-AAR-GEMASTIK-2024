using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public struct StatsDataPassing
{
    public Sprite imageData;
    public string statsNameString;
    public string statsValueString;
    public bool unlocked;
    public StatsDataPassing(Sprite imageData, string statsNameString, string statsValueString, bool unlocked)
    {
        this.imageData = imageData;
        this.statsNameString = statsNameString;
        this.statsValueString = statsValueString;
        this.unlocked = unlocked;
    }
}
public class StatsCard : MonoBehaviour
{
    public Image IconStats;
    public TextMeshProUGUI statsName;
    public TextMeshProUGUI BuyStatsValue;
    public Transform upgradeContainerStatsValue;
    private float[] upgradeValue;
    public void SetUpData(StatsDataPassing dataPassing)
    {
        if(dataPassing.imageData != null)
        {
            IconStats.sprite = dataPassing.imageData;
        }
        statsName.text = dataPassing.statsNameString;
        BuyStatsValue.text = dataPassing.statsValueString;
        BuyStatsValue.gameObject.SetActive(true);
        upgradeContainerStatsValue.gameObject.SetActive(false);
    }
    public void OnUpdateUpgradeStats(float[] upgradeValue, bool unlocked)
    {
        if(!unlocked)
        {
            if(upgradeValue != null)
            {
                BuyStatsValue.text = upgradeValue[0].ToString();
                BuyStatsValue.gameObject.SetActive(true);
                upgradeContainerStatsValue.gameObject.SetActive(false);
            }
            return;
        }
        this.upgradeValue = upgradeValue;
        upgradeContainerStatsValue.gameObject.SetActive(true);
        BuyStatsValue.gameObject.SetActive(false);
        TextMeshProUGUI currentValue = upgradeContainerStatsValue.GetChild(0).GetComponent<TextMeshProUGUI>();
        currentValue.text = this.upgradeValue[0].ToString();
        TextMeshProUGUI nextLevelValue = upgradeContainerStatsValue.GetChild(2).GetComponent<TextMeshProUGUI>();
        nextLevelValue.text = this.upgradeValue[1].ToString();
    }
}
