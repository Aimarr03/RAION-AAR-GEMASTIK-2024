using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyBase;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private Transform HealthBar;
    [SerializeField] private Image currentHealthVisual;
    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    private void Awake()
    {
        HealthBar.gameObject.SetActive(false);
    }
    private void Start()
    {
        enemyBase.GetHealthSystem().OnChangeValue += EnemyUI_OnChangeValue;
        enemyName.text = enemyBase.fishName;
    }

    private void EnemyUI_OnChangeValue(UpdateEnemyHealthValue dataHealth)
    {
        HealthBar.gameObject.SetActive(true);
        float percentage = dataHealth.PercentageValue;
        Debug.Log(dataHealth.currentValue);
        Debug.Log(dataHealth.maxValue);
        Debug.Log("Percentage Value " + dataHealth.PercentageValue);
        currentHealthVisual.fillAmount = percentage;
    }
}
