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

    private void EnemyUI_OnChangeValue(UpdateEnemyHealthValue obj)
    {
        HealthBar.gameObject.SetActive(true);
        float percentage = obj.PercentageValue;
        currentHealthVisual.fillAmount = percentage;
    }
}
