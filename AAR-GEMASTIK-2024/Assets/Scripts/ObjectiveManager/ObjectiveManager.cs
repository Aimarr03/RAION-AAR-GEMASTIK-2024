using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectiveManager : MonoBehaviour, IDataPersistance
{
    public int currentPhase { get; protected set; }
    public bool LevelCompleted;

    public abstract void Start();
    public abstract void OnDisable();
    public abstract void LoadScene(GameData gameData);

    public abstract void SaveScene(ref GameData gameData);
}
