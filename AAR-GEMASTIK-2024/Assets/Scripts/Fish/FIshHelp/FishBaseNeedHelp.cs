using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBaseNeedHelp : MonoBehaviour, IInterractable, IDataPersistance, IDamagable
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
    public ParticleSystem onDeadparticleSystem;
    public static event Action<int> OnBroadcastGettingHelp;

    public abstract void AltInterracted(PlayerInterractionSystem playerInterractionSystem);

    public abstract void Interracted(PlayerInterractionSystem playerInterractionSystem);

    public abstract void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem);

    protected void InvokeOnGettingHelp() => OnGettingHelp?.Invoke();
    protected void InvokeOnBeingNoticed() => OnBeingNoticed?.Invoke();

    protected void InvokeBroadcastGettingHelpDone() => OnBroadcastGettingHelp?.Invoke(bounty);
    public void LoadScene(GameData gameData)
    {
        if (TutorialManager.instance != null) return;
        SubLevelData levelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        levelData.fishNeedHelpList.TryGetValue(id, out bool value);
        if (value)
        {
            gameObject.SetActive(false);
            hasBeenHelped = true;
        }
    }

    public void SaveScene(ref GameData gameData)
    {
        if (TutorialManager.instance != null) return;
        SubLevelData levelData = gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (levelData.fishNeedHelpList.ContainsKey(id))
        {
            levelData.fishNeedHelpList.Remove(id);
        }
        levelData.fishNeedHelpList.Add(id, hasBeenHelped);
    }
    public bool HasBeenHelped() => hasBeenHelped;


    public void TakeDamage(int damage)
    {
        fishCollider.enabled = false;
        onDeadparticleSystem.Play();
        onDeadparticleSystem.transform.parent = null;
        for(int index = 0; index < transform.childCount; index++)
        {
            Transform currentChild = transform.GetChild(index);
            currentChild.gameObject.SetActive(false);
        }
        ExpedictionManager.Instance.InvokeOnLose("Mengapa anda menyerang ikan yang sudah tidak tertolong??");
    }

    public IEnumerator GetSlowed(float duration, float multilpier)
    {
        yield return null;
    }

    public void AddSuddenForce(Vector3 directiom, float forcePower)
    {

    }

    public void OnDisableMove(float moveDuration, int maxAttemptToRecover)
    {

    }
}
