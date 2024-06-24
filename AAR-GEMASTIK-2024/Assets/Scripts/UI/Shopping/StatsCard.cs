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
    private UpgradeStats upgradeValue;
    public void SetUpData(BuyStats buyStats)
    {
        /*if(dataPassing.imageData != null)
        {
            IconStats.sprite = dataPassing.imageData;
        }*/
        statsName.text = buyStats.header;
        BuyStatsValue.text = buyStats.value;
        BuyStatsValue.gameObject.SetActive(true);
        upgradeContainerStatsValue.gameObject.SetActive(false);
    }
    public void OnUpdateUpgradeStats(UpgradeStats upgradeValue, bool unlocked)
    {
        if (upgradeValue.nextValue == null) return;
        if(!unlocked)
        {
            if(upgradeValue != null)
            {
                BuyStatsValue.text = upgradeValue.value;
                BuyStatsValue.gameObject.SetActive(true);
                upgradeContainerStatsValue.gameObject.SetActive(false);
            }
            return;
        }
        this.upgradeValue = upgradeValue;
        upgradeContainerStatsValue.gameObject.SetActive(true);
        BuyStatsValue.gameObject.SetActive(false);
        TextMeshProUGUI currentValue = upgradeContainerStatsValue.GetChild(0).GetComponent<TextMeshProUGUI>();
        currentValue.text = upgradeValue.value;
        TextMeshProUGUI nextLevelValue = upgradeContainerStatsValue.GetChild(2).GetComponent<TextMeshProUGUI>();
        nextLevelValue.text = upgradeValue.nextValue;
    }
}
