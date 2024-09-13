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
        if (!weightSystem.canAddWeight(weight)) return;
        weightSystem.OnIncreaseValue(weight);
        Debug.Log("Player receive trash");
        OnCollected();
        //Destroy(gameObject);
    }

    public void LoadScene(GameData gameData)
    {
        if (TutorialManager.instance != null) return;
        SubLevelData levelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        levelData.trashList.TryGetValue(id, out bool hasCollected);
        if(hasCollected)
        {
            OnDeloading();
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        if (ExpedictionManager.Instance != null && ExpedictionManager.Instance.IsLosing()) return;
        if (TutorialManager.instance != null) return;
        SubLevelData levelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if(levelData.trashList.ContainsKey(id))
        {
            levelData.trashList.Remove(id);
        }
        levelData.trashList.Add(id, collected);
    }
    private void OnDeloading()
    {
        collected = true;
        for(int index = 0; index < transform.childCount; index++)
        {
            Transform child = transform.GetChild(index);
            child.gameObject.SetActive(false);
        }
        //GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
    }
    private void OnCollected()
    {
        OnDeloading();
        OnCollectedEvent?.Invoke();
    }
    public bool HasBeenCollected() => collected;
}
