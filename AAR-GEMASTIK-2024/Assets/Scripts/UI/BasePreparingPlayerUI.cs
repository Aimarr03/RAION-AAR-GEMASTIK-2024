using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BasePreparingPlayerUI : MonoBehaviour
{
    [SerializeField] protected PreparingUIManager uiManager;
    public abstract IEnumerator OnEnterState();
    public abstract IEnumerator OnExitState();
}
