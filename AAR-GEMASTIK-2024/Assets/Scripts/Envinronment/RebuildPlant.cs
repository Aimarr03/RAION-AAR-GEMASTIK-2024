using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RebuildPlant : MonoBehaviour, IInterractable, IDataPersistance
{
    [SerializeField] protected string id;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<SpriteRenderer> childCorals;
    private PlayerCoreSystem coreSystem;
    private Collider2D plantCollider;
    [SerializeField] private UI_RebuildPlant ui;
    [SerializeField] private List<AudioClip> audioClipList;
    [SerializeField] private AudioClip startPlanting, failedPlanting;

    private float zeroAlpha = 0f;
    private float quaterAlpha = .25f;
    private float halfAlpha = 0.5f;
    private float fullAlpha = 1f;
    private float currentDuration = 0f;

    private float MaxIntervalDuration = 3.3f;
    private float currentIntervalDuration = 0f;
    private float minValue = 0f;
    private float Contraint_MaxValue = 0.37f;
    private float currentMaxValue;

    private int Phase = 0;
    private int MaxPhase => childCorals.Count + 1;
    private float percentageDuration => currentDuration / currentIntervalDuration;


    private bool canBeInterracted;
    private bool isInterracted;
    private bool donePlanted;

    public static event Action ActionOnDonePlanted;
    private SpriteRenderer currentSprite;
    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprite = spriteRenderer;
        plantCollider = GetComponent<Collider2D>();
        foreach(SpriteRenderer currentChild in childCorals)
        {
            currentChild.gameObject.SetActive(true);
            ChangeAlphaValue(zeroAlpha, currentChild);
        }
        canBeInterracted = true;
        ChangeAlphaValue(quaterAlpha);
    }
    private void Update()
    {
        if (isInterracted && canBeInterracted)
        {
            currentDuration += Time.deltaTime;
            ui.ShowProcess(currentDuration / MaxIntervalDuration);
            if (currentDuration > MaxIntervalDuration)
            {
                OnFailed();
            }
        }
    }
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        if (isInterracted)
        {
            bool failed = percentageDuration < minValue || percentageDuration > (minValue + currentMaxValue);
            if (!failed)
            {
                ChangeAlphaValue(halfAlpha, currentSprite);
                Phase++;
                if(Phase >= MaxPhase)
                {
                    OnDonePlanted();
                    return;
                }
                AudioManager.Instance.PlaySFX(audioClipList[UnityEngine.Random.Range(0, audioClipList.Count)]);
                CalculatePhase();
            }
            else
            {
                OnFailed();
            }
        }
    }
    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        if (!canBeInterracted) return;
        if (isInterracted)
        {
            AltInterracted(playerInterractionSystem);
            return;
        }
        AudioManager.Instance.PlaySFX(startPlanting);
        isInterracted = true;
        ui.SwitchCanvasToProcess(true);
        coreSystem.moveSystem.Stop();
        CalculatePhase();
    }
    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (!canBeInterracted) return;
        if (coreSystem == null)
        {
            isInterracted = false;
            currentDuration = 0f;
            ui.ToggleAllCanvas(false);
            Phase = 0;
        }
        else
        {
            ui.SwitchCanvasToProcess(false);
            ChangeAlphaValue(halfAlpha, currentSprite);
            Phase = 0;
        }
        this.coreSystem = coreSystem;
    }
    private async void OnFailed()
    {
        AudioManager.Instance.PlaySFX(failedPlanting);
        isInterracted = false;
        canBeInterracted = false;
        currentSprite = spriteRenderer;
        ChangeAlphaValue(zeroAlpha, currentSprite);
        foreach(SpriteRenderer sprite in childCorals)
        {
            ChangeAlphaValue(zeroAlpha, sprite);
        }
        Phase = 0;
        currentDuration = 0;
        ui.ToggleAllCanvas(false);
        await Task.Delay(2200);
        if(coreSystem != null)
        {
            canBeInterracted = true;
            ui.SwitchCanvasToProcess(false);
            ChangeAlphaValue(quaterAlpha, currentSprite);
        }
    }
    private void CalculatePhase()
    {
        if(Phase > 0)
        {
            currentSprite = childCorals[Phase-1];
        }
        currentDuration = 0f;
        currentMaxValue = Contraint_MaxValue  - (0.035f * Phase);
        currentIntervalDuration = MaxIntervalDuration - (0.0325f * Phase);
        minValue = UnityEngine.Random.Range(0.3f, 1 - currentMaxValue);
        ChangeAlphaValue(quaterAlpha, currentSprite);
        ui.OnSetValue(minValue, currentMaxValue);
        
    }
    private void ChangeAlphaValue(float value)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = value;
        spriteRenderer.color = currentColor;
    }
    private void ChangeAlphaValue(float value, SpriteRenderer focusSprite)
    {
        Color currentColor = focusSprite.color;
        currentColor.a = value;
        focusSprite.color = currentColor;
    }
    private void OnDonePlanted()
    {
        currentDuration = 0f;
        donePlanted = true;
        ChangeAlphaValue(fullAlpha, spriteRenderer);
        foreach(SpriteRenderer childCoral in childCorals)
        {
            ChangeAlphaValue(fullAlpha, childCoral);
        }
        canBeInterracted = false;
        isInterracted = false;
        ui.ToggleAllCanvas(false);
        plantCollider.enabled = false;
        GetComponent<Collider>().enabled = false;
        ActionOnDonePlanted?.Invoke();
    }
    public void OnDisableFunction()
    {
        canBeInterracted = false;
        isInterracted = false;
        ChangeAlphaValue(zeroAlpha);
        ui.ToggleAllCanvas(false);
        plantCollider.enabled = false;
    }
    public void OnEnableFunction()
    {
        canBeInterracted = true;
        isInterracted = false;
        ChangeAlphaValue(quaterAlpha);
        ui.ToggleAllCanvas(false);
        plantCollider.enabled = true;
    }


    public void LoadScene(GameData gameData)
    {
        SubLevelData subLevelData = DataManager.instance.gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (subLevelData == null) return;
        if (!subLevelData.additionalCollectableObjects.ContainsKey("Rebuild Plant"))
        {
            subLevelData.additionalCollectableObjects.Add("Rebuild Plant", new SerializableDictionary<string, bool>());
        }
        SerializableDictionary<string, bool> trashCoreDictionary = subLevelData.additionalCollectableObjects["Rebuild Plant"]; 
        if (trashCoreDictionary.TryGetValue(id, out bool value))
        {
            if (value)
            {
                currentDuration = 0f;
                donePlanted = true;
                ChangeAlphaValue(fullAlpha, spriteRenderer);
                foreach (SpriteRenderer childCoral in childCorals)
                {
                    ChangeAlphaValue(fullAlpha, childCoral);
                }
                canBeInterracted = false;
                isInterracted = false;
                ui.ToggleAllCanvas(false);
                plantCollider.enabled = false;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
    public void SaveScene(ref GameData gameData)
    {
        if (ExpedictionManager.Instance != null && ExpedictionManager.Instance.IsLosing()) return;
        SubLevelData subLevelData = DataManager.instance.gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (subLevelData == null) return;
        if (!subLevelData.additionalCollectableObjects.ContainsKey("Rebuild Plant"))
        {
            subLevelData.additionalCollectableObjects.Add("Rebuild Plant", new SerializableDictionary<string, bool>());
        }
        SerializableDictionary<string, bool> trashCoreDictionary = subLevelData.additionalCollectableObjects["Rebuild Plant"];
        if (trashCoreDictionary.ContainsKey(id))
        {
            trashCoreDictionary.Remove(id);
        }
        trashCoreDictionary.Add(id, donePlanted);
    }
}
