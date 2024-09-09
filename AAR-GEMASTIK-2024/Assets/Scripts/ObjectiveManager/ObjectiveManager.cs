using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectiveManager : MonoBehaviour, IDataPersistance
{
    public int currentPhase { get; protected set; }
    public bool LevelCompleted;
    public static ObjectiveManager Instance;
    public static event Action<List<ObjectiveData>> OnPhaseCompleted;
    public static event Action<ObjectiveData> OnObjectiveProgress;

    public abstract List<ObjectiveData> GetOverallObjectives();
    protected virtual void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    public abstract void Start();
    public abstract void OnDisable();
    public abstract void LoadScene(GameData gameData);

    public abstract void SaveScene(ref GameData gameData);
    protected void InvokeObjectiveProgress(ObjectiveData objectives) => OnObjectiveProgress?.Invoke(objectives);
    protected void InvokeNewObjectives(List<ObjectiveData> ListOfObjectives) => OnPhaseCompleted?.Invoke(ListOfObjectives);
}
[Serializable]
public class ObjectiveData
{
    public string description;
    public int currentProgress;
    public int maxProgress;
    public float progress => currentProgress/maxProgress;
    public ObjectiveData(string description, int currentProgress, int maxProgress)
    {
        this.description = description;
        this.currentProgress = currentProgress;
        this.maxProgress = maxProgress;
    }
}