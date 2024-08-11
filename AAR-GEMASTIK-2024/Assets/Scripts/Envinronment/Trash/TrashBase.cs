using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrashBase : MonoBehaviour, IDataPersistance
{
    [SerializeField] protected string id;
    protected bool collected;
    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    protected PlayerCoreSystem playerCoreSystem;
    [SerializeField] protected float weight;
    public static event Action OnCollectedEvent;
    protected abstract void OnTriggerEnter2D(Collider2D other);
    public void OnTakenByPlayer()
    {
        if (playerCoreSystem == null) return;
        WeightSystem weightSystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity) as WeightSystem;
        if(!weightSystem.canAddWeight(weight)) return;
        weightSystem.OnIncreaseValue(weight);
        Debug.Log("Player receive trash");
        OnCollected();
        //Destroy(gameObject);
    }

    public void LoadScene(GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        levelData.trashList.TryGetValue(id, out bool hasCollected);
        if(hasCollected)
        {
            OnDeloading();
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        if(levelData.trashList.ContainsKey(id))
        {
            levelData.trashList.Remove(id);
        }
        levelData.trashList.Add(id, collected);
    }
    private void OnDeloading()
    {
        collected = true;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
    }
    private void OnCollected()
    {
        OnDeloading();
        OnCollectedEvent?.Invoke();
    }
    public bool HasBeenCollected() => collected;
}
