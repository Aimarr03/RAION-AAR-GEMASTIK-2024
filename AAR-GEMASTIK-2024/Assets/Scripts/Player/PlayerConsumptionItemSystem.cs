using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumptionItemSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
    }
}
