using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        //text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        text.text = EconomyManager.Instance.currentMoney.ToString();
        EconomyManager.Instance.UseMoney += Instance_UseMoney;
    }

    private void Instance_UseMoney(int obj)
    {
        text.text = obj.ToString();
    }

}
