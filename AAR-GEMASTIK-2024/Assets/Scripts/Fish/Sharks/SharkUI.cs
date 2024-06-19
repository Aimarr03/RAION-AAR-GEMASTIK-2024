using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkUI : MonoBehaviour
{
    public SharkBase sharkBase;
    public RectTransform healthHolder;
    public Image currentHealthUI;

    private void Start()
    {
        sharkBase.onTakeDamage += SharkBase_onTakeDamage;
        healthHolder.gameObject.SetActive(false);
    }

    private void SharkBase_onTakeDamage(bool isDead, float percentage)
    {
        healthHolder.gameObject.SetActive(isDead);
        currentHealthUI.fillAmount = percentage;
        if(isDead) sharkBase.onTakeDamage -= SharkBase_onTakeDamage;
    }
}
