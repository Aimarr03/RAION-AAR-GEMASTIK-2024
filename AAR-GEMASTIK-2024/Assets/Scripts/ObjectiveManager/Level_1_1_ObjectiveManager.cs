using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_1_ObjectiveManager : ObjectiveManager
{
    [SerializeField] private Transform DialogueContainer;
    [SerializeField] private AudioClip onProgress, onCompletedProgress;
    private List<TrashBase> ListOfTrashes;
    private List<TrashCore> ListOfTrashCores;
    private List<FishBaseNeedHelp> ListOfFishNeedHelps;
    private List<RebuildPlant> ListOfRebuildPlant;

    private CollectedObjects trashCollection;
    private CollectedObjects fishCollection;
    private CollectedObjects trashCoreCollection;
    private CollectedObjects CoralCollection;

    ObjectiveData trashObjective;
    ObjectiveData fishObjective;
    ObjectiveData trashCoreObjective;
    ObjectiveData CoralObjective;

    private int BufferFishCurrentCollected, BufferTrashCurrentCollected, BufferTrashCoreCurrentCollected, BufferCoralCurrentCollected;
    #region Monobehaviour Callbacks
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
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
        BufferFishCurrentCollected = subLevelData.fishNeededHelpCountDone;
        BufferTrashCurrentCollected = subLevelData.trashCountDone;
        if(!subLevelData.collectedAdditionalCollectableObjects.ContainsKey("Trash Core"))
        {
            Debug.Log("Trash Core doesn't contain at all");
            subLevelData.collectedAdditionalCollectableObjects.Add("Trash Core", 0);
        }
        if (!subLevelData.collectedAdditionalCollectableObjects.ContainsKey("Rebuild Plant"))
        {
            Debug.Log("Coral doesn't contain at all");
            subLevelData.collectedAdditionalCollectableObjects.Add("Rebuild Plant", 0);
        }
        BufferTrashCoreCurrentCollected = subLevelData.collectedAdditionalCollectableObjects["Trash Core"];
        BufferCoralCurrentCollected = subLevelData.collectedAdditionalCollectableObjects["Rebuild Plant"];
    }
    public override void SaveScene(ref GameData gameData)
    {
        SubLevelData subLevelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        subLevelData.currentCompletedPhase = currentPhase;
        subLevelData.collectedAdditionalCollectableObjects["Trash Core"] = trashCoreCollection.currentCollected;
        if(CoralCollection != null)
        {
            subLevelData.collectedAdditionalCollectableObjects["Rebuild Plant"] = CoralCollection.currentCollected;
        }
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
        trashCollection.currentCollected += BufferTrashCurrentCollected;
        
        fishCollection = new CollectedObjects((int)(ListOfFishNeedHelps.Count * 0.75f));
        fishCollection.currentCollected += BufferFishCurrentCollected;

        trashCoreCollection = new CollectedObjects((int)(ListOfTrashCores.Count));
        trashCoreCollection.currentCollected += BufferTrashCoreCurrentCollected;

        foreach (RebuildPlant plant in ListOfRebuildPlant)
        {
            plant.OnDisableFunction();
        }
        trashObjective = new ObjectiveData("Bersihkan <b>sampah-sampah</b> di area: ", trashCollection.currentCollected, trashCollection.minCollected);
        fishObjective = new ObjectiveData("Menolongi <b>ikan-ikan</b> di area: ", fishCollection.currentCollected, fishCollection.minCollected);
        trashCoreObjective = new ObjectiveData("Bersihkan <b>gumpalan sampah</b> di area: ", trashCoreCollection.currentCollected, trashCoreCollection.minCollected);

        InvokeNewObjectives(new List<ObjectiveData> { trashObjective, fishObjective, trashCoreObjective });

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
        fishObjective.currentProgress = fishCollection.currentCollected;
        InvokeObjectiveProgress(fishObjective);
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
        trashObjective.currentProgress = trashCollection.currentCollected;
        InvokeObjectiveProgress(trashObjective);
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
        trashCoreObjective.currentProgress = trashCoreCollection.currentCollected;
        InvokeObjectiveProgress(trashCoreObjective);
        CheckIsDonePhase1();
    }
    private void CheckIsDonePhase1() 
    {
        bool isDone = trashCollection.isDone && trashCoreCollection.isDone && fishCollection.isDone;
        if (isDone)
        {
            currentPhase++;
            SetObjective();
            //OnFinishedPhased?.Invoke(800);
            AudioManager.Instance.PlaySFX(onCompletedProgress);
        }
        else
        {
            AudioManager.Instance.PlaySFX(onProgress);
        }
    }
    #endregion
    #region PHASE 2
    private void PhaseTwoObjective()
    {
        Debug.Log("Phase Two from the Sub Level");
        CoralCollection = new CollectedObjects(ListOfRebuildPlant.Count);
        CoralCollection.currentCollected += BufferCoralCurrentCollected;
        CoralObjective = new ObjectiveData("Pasangkan <b>Karang Artifisal</b> ke area tertentu", CoralCollection.currentCollected, CoralCollection.minCollected);
        foreach (RebuildPlant plant in ListOfRebuildPlant)
        {
            plant.OnEnableFunction();
        }
        InvokeNewObjectives(new List<ObjectiveData> { CoralObjective });
        RebuildPlant.ActionOnDonePlanted += RebuildPlant_ActionOnDonePlanted;
    }

    private void RebuildPlant_ActionOnDonePlanted()
    {
        CoralCollection.currentCollected++;
        Debug.Log($"Plant Planted, progress ongoing  {CoralCollection.currentCollected}/{CoralCollection.minCollected}");
        if (CoralCollection.isDone)
        {
            Debug.Log("Phase two COMPLETE!");
            currentPhase++;
            AudioManager.Instance.PlaySFX(onCompletedProgress);
            SetObjective();
            RebuildPlant.ActionOnDonePlanted -= RebuildPlant_ActionOnDonePlanted;
        }
        else
        {
            AudioManager.Instance.PlaySFX(onProgress);
        }
        InvokeObjectiveProgress(CoralObjective);
    }

    public override List<ObjectiveData> GetOverallObjectives()
    {
        Debug.Log("Objectives Data");
        switch(currentPhase)
        {
            case 0:
                return new List<ObjectiveData>
                {
                    trashObjective,
                    fishObjective,
                    trashCoreObjective
                };
            case 1:
                return new List<ObjectiveData>
                {
                    CoralObjective
                };
            case 2:
                return new List<ObjectiveData>
                {
                    new ObjectiveData("Level Usai, Kerja Bagus!", 0, 0)
                };
            default: return null;
        }
    }
    #endregion
}
[Serializable]
public class CollectedObjects
{
    public string descriptionObjectives;
    public int currentCollected;
    public int minCollected;

    public CollectedObjects(int minCollected)
    {
        this.minCollected = minCollected;
        currentCollected = 0;
    }
    public bool isDone => currentCollected >= minCollected;
}
