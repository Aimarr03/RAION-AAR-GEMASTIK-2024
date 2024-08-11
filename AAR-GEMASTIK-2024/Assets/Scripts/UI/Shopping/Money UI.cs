using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private int currentMoney;
    private void Awake()
    {
        //text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        text.text = EconomyManager.Instance.currentMoney.ToString();
        currentMoney = Int32.Parse(text.text);
        EconomyManager.Instance.UseMoney += Instance_UseMoney;
    }
    private void OnDisable()
    {
        EconomyManager.Instance.UseMoney -= Instance_UseMoney;
    }

    private void Instance_UseMoney(int newMoneyValue)
    {
        text.text = newMoneyValue.ToString();
        StartCoroutine(OnChangeMoneyUIGradually(newMoneyValue));
    }
    private IEnumerator OnChangeMoneyUIGradually(int newValue)
    {
        float currentTime = 0;
        float maxTime = 2;
        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime/ maxTime);
            float currentValue = Mathf.Lerp(currentMoney, newValue, t);
            text.text = currentValue.ToString("0");
            AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnTransaction);
            yield return null;
        }
        text.text = newValue.ToString();
        currentMoney = newValue;
    }
}
