using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public WeaponSO chosenWeaponSO;
    public AbilitySO chosenAbilitySO;
    //public ConsumableItemSO chosenConsumableItemSO;
    public HealthItemSO chosenHealthItemSO;
    public OxygenItemSO chosenOxygenItemSO;
    public EnergyItemSO chosenEnergyItemSO;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetChosenItemSO(ConsumableItemSO itemSO)
    {
        switch (itemSO.type)
        {
            case SustainabilityType.Health:
                chosenHealthItemSO = itemSO as HealthItemSO;
                break;
            case SustainabilityType.Oxygen:
                chosenOxygenItemSO = itemSO as OxygenItemSO;
                break;
            case SustainabilityType.Energy: 
                chosenEnergyItemSO = itemSO as EnergyItemSO;
                break;
        }
    }
    public bool CheckHasAssigned()
    {
        return chosenAbilitySO != null && chosenWeaponSO != null;
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
