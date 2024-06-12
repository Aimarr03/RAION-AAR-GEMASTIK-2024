using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour, IDataPersistance
{
    public static ShopManager instance;
    public List<ItemBaseSO> weaponList;
    public List<ItemBaseSO> abilityList;
    public List<ItemBaseSO> itemList;
    public List<ItemBaseSO> sustainabilityList;
    public ShopMode shopMode;

    public void LoadScene(GameData gameData)
    {
        
        foreach (ItemBaseSO item in weaponList)
        {
            WeaponSO weapon = item as WeaponSO;
            weapon.LoadScene(gameData);
        }
        foreach (ItemBaseSO item in abilityList)
        {
            AbilitySO ability = item as AbilitySO;
            ability.LoadScene(gameData);
        }
        foreach(ItemBaseSO item in itemList)
        {
            ConsumableItemSO consumableItem = item as ConsumableItemSO;
            consumableItem.LoadScene(gameData);
        }
        foreach(ItemBaseSO item in sustainabilityList)
        {
            SustainabilitySystemSO systemSO = item as SustainabilitySystemSO;
            systemSO.LoadScene(gameData);
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        foreach (ItemBaseSO item in weaponList)
        {
            WeaponSO weapon = item as WeaponSO;
            weapon.SaveScene(ref gameData);
        }
        foreach (ItemBaseSO item in abilityList)
        {
            AbilitySO ability = item as AbilitySO;
            ability.SaveScene(ref gameData);
        }
        foreach (ItemBaseSO item in itemList)
        {
            ConsumableItemSO consumableItem = item as ConsumableItemSO;
            consumableItem.SaveScene(ref gameData);
        }
        foreach (ItemBaseSO item in sustainabilityList)
        {
            SustainabilitySystemSO systemSO = item as SustainabilitySystemSO;
            systemSO.SaveScene(ref gameData);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
}
