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
    public StatsDataPassing(Sprite imageData, string statsNameString, string statsValueString)
    {
        this.imageData = imageData;
        this.statsNameString = statsNameString;
        this.statsValueString = statsValueString;
    }
}
public class StatsCard : MonoBehaviour
{
    public Image IconStats;
    public TextMeshProUGUI statsName;
    public TextMeshProUGUI statsValue;

    public void SetUpData(StatsDataPassing dataPassing)
    {
        if(dataPassing.imageData != null)
        {
            IconStats.sprite = dataPassing.imageData;
        }
        statsName.text = dataPassing.statsNameString;
        statsValue.text = dataPassing.statsValueString;
    }
}
