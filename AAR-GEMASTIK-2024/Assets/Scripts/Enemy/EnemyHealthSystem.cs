using System;
using UnityEngine;

public struct UpdateEnemyHealthValue
{
    public int currentValue;
    public int maxValue;
    public readonly float PercentageValue => currentValue / maxValue;
    public UpdateEnemyHealthValue(int currentValue, int maxValue)
    {
        this.currentValue = currentValue;
        this.maxValue = maxValue;
    }
}
public class EnemyHealthSystem
{
    private EnemyBase enemyBase;
    private int currentHealth;
    private int maxHealth;
    public event Action<UpdateEnemyHealthValue> OnChangeValue;

    public EnemyHealthSystem(EnemyBase enemyBase, int maxHealth)
    {
        this.enemyBase = enemyBase;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }
    public void OnDecreaseHealth(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if(currentHealth == 0)
        {
            enemyBase.SetDead();
        }
        else
        {
            UpdateEnemyHealthValue HealthData = new UpdateEnemyHealthValue(currentHealth, maxHealth);
            OnChangeValue?.Invoke(HealthData);
        }
    }
}
