using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public List<ItemBaseSO> weaponList;
    public List<ItemBaseSO> abilityList;
    public List<ItemBaseSO> itemList;
    public List<ItemBaseSO> sustainabilityList;
    public ShopMode shopMode;
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
