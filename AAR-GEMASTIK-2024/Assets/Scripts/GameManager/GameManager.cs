using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public WeaponSO chosenWeaponSO;
    public AbilitySO chosenAbilitySO;
    public ConsumableItemSO chosenConsumableItemSO;

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
    public bool CheckHasAssigned()
    {
        return chosenAbilitySO != null && chosenConsumableItemSO != null && chosenConsumableItemSO != null;
    }
}
