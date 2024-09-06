using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_1_ObjectiveManager : ObjectiveManager
{
    private List<TrashBase> ListOfTrashes;
    private List<TrashCore> ListOfTrashCores;
    private List<FishBaseNeedHelp> ListOfFishNeedHelps;
    private List<RebuildPlant> ListOfRebuildPlant;

    private CollectedObjects trashCollection;
    private CollectedObjects fishCollection;
    private CollectedObjects trashCoreCollection;
    private CollectedObjects plantCollection;
    

    #region Monobehaviour Callbacks
    private void Awake()
    {
        ListOfTrashCores = new List<TrashCore>();

        IEnumerable<TrashBase> trashes = FindObjectsOfType<TrashBase>();
        IEnumerable<TrashCore> trashCores = FindObjectsOfType<TrashCore>();
        IEnumerable<FishBaseNeedHelp> fishNeedHelp = FindObjectsOfType<FishBaseNeedHelp>();
        IEnumerable<RebuildPlant> rebuildPlants = FindObjectsOfType<RebuildPlant>();
        ListOfTrashes = new List<TrashBase>(trashes);
        ListOfTrashCores = new List<TrashCore>(trashCores);
        ListOfFishNeedHelps = new List<FishBaseNeedHelp>(fishNeedHelp);
        ListOfRebuildPlant = new List<RebuildPlant>(rebuildPlants);
        
    }
    public override void Start()
    {
        SetObjective();
    }
    public override void OnDisable()
    {
        TrashCore.TrashCoreCollected -= TrashCore_TrashCoreCollected;
        TrashBase.OnCollectedEvent -= TrashBase_OnCollectedEvent;
        FishBaseNeedHelp.OnBroadcastGettingHelp -= FishBaseNeedHelp_OnBroadcastGettingHelp;
        RebuildPlant.ActionOnDonePlanted -= RebuildPlant_ActionOnDonePlanted;
    }
    #endregion
    #region IDATAPERSISTANCE
    public override void LoadScene(GameData gameData)
    {
        SubLevelData subLevelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (subLevelData.currentCompletedPhase > 0)
        {
            currentPhase = subLevelData.currentCompletedPhase;
        }
    }
    public override void SaveScene(ref GameData gameData)
    {
        
    }
    #endregion
    #region OBJECTIVES
    private void SetObjective()
    {
        switch(currentPhase)
        {
            case 0:
                PhaseOneObjective();
                break;
            case 1: 
                PhaseTwoObjective(); break;
            case 2:
                LevelCompleted = true;
                break;
        }
    }
    private void PhaseOneObjective()
    {
        Debug.Log("Phase One from the Sub Level");
        trashCollection = new CollectedObjects((int)(ListOfTrashes.Count * 0.75f));
        fishCollection = new CollectedObjects((int)(ListOfFishNeedHelps.Count * 0.75f));
        trashCoreCollection = new CollectedObjects((int)(ListOfTrashCores.Count));

        foreach (RebuildPlant plant in ListOfRebuildPlant)
        {
            plant.OnDisableFunction();
        }

        TrashCore.TrashCoreCollected += TrashCore_TrashCoreCollected;
        TrashBase.OnCollectedEvent += TrashBase_OnCollectedEvent;
        FishBaseNeedHelp.OnBroadcastGettingHelp += FishBaseNeedHelp_OnBroadcastGettingHelp;
    }
    #endregion
    #region PHASE 1
    private void FishBaseNeedHelp_OnBroadcastGettingHelp(int obj)
    {
        fishCollection.currentCollected++;
        Debug.Log($"Fish Helped, progress ongoing  {fishCollection.currentCollected}/{fishCollection.minCollected}");
        if(fishCollection.isDone )
        {
            Debug.Log("Objective Fish Done!");
            FishBaseNeedHelp.OnBroadcastGettingHelp -= FishBaseNeedHelp_OnBroadcastGettingHelp;
        }
        CheckIsDonePhase1();
    }

    private void TrashBase_OnCollectedEvent()
    {
        trashCollection.currentCollected++;
        Debug.Log($"Trash Collected, progress ongoing  {trashCollection.currentCollected}/{trashCollection.minCollected}");
        if (trashCollection.isDone)
        {
            Debug.Log("Objective Trash Done!");
            TrashBase.OnCollectedEvent -= TrashBase_OnCollectedEvent;
        }
        CheckIsDonePhase1();
    }

    private void TrashCore_TrashCoreCollected()
    {
        trashCoreCollection.currentCollected++;
        Debug.Log($"Trash Core Collected, progress ongoing  {trashCoreCollection.currentCollected}/{trashCoreCollection.minCollected}");
        if (trashCoreCollection.isDone)
        {
            Debug.Log("Objective Core Done!");
            TrashCore.TrashCoreCollected -= TrashCore_TrashCoreCollected;
        }
        CheckIsDonePhase1();
    }
    private void CheckIsDonePhase1() 
    {
        bool isDone = trashCollection.isDone && trashCoreCollection.isDone && fishCollection.isDone;
        if (isDone)
        {
            currentPhase++;
            SetObjective();
        }
    }
    #endregion
    #region PHASE 2
    private void PhaseTwoObjective()
    {
        Debug.Log("Phase Two from the Sub Level");
        plantCollection = new CollectedObjects(ListOfRebuildPlant.Count);
        foreach (RebuildPlant plant in ListOfRebuildPlant)
        {
            plant.OnEnableFunction();
        }
        RebuildPlant.ActionOnDonePlanted += RebuildPlant_ActionOnDonePlanted;
    }

    private void RebuildPlant_ActionOnDonePlanted()
    {
        plantCollection.currentCollected++;
        Debug.Log($"Plant Planted, progress ongoing  {plantCollection.currentCollected}/{plantCollection.minCollected}");
        if (plantCollection.isDone)
        {
            Debug.Log("Phase two COMPLETE!");
            currentPhase++;
            SetObjective();
            RebuildPlant.ActionOnDonePlanted -= RebuildPlant_ActionOnDonePlanted;
        }
    }
    #endregion
}
[Serializable]
public class CollectedObjects
{
    public int currentCollected;
    public int minCollected;

    public CollectedObjects(int minCollected)
    {
        this.minCollected = minCollected;
        currentCollected = 0;
    }
    public bool isDone => currentCollected >= minCollected;
}
