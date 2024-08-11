using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBaseNeedHelp : MonoBehaviour, IInterractable, IDataPersistance
{
    [SerializeField] private string id;

    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public bool playerIsNull { get => playerCoreSystem == null; }
    public int bounty;
    protected bool hasBeenHelped = false;
    [SerializeField] protected Collider2D fishCollider;
    protected PlayerCoreSystem playerCoreSystem;
    public event Action OnGettingHelp;
    public event Action OnBeingNoticed;
    public static event Action<int> OnBroadcastGettingHelp;

    public abstract void AltInterracted(PlayerInterractionSystem playerInterractionSystem);

    public abstract void Interracted(PlayerInterractionSystem playerInterractionSystem);

    public abstract void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem);

    protected void InvokeOnGettingHelp() => OnGettingHelp?.Invoke();
    protected void InvokeOnBeingNoticed() => OnBeingNoticed?.Invoke();

    protected void InvokeBroadcastGettingHelpDone() => OnBroadcastGettingHelp?.Invoke(bounty);
    public void LoadScene(GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        levelData.fishNeedHelpList.TryGetValue(id, out bool value);
        if (value)
        {
            gameObject.SetActive(false);
            hasBeenHelped = true;
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        if (levelData.fishNeedHelpList.ContainsKey(id))
        {
            levelData.fishNeedHelpList.Remove(id);
        }
        levelData.fishNeedHelpList.Add(id, hasBeenHelped);
    }
    public bool HasBeenHelped() => hasBeenHelped;
}
