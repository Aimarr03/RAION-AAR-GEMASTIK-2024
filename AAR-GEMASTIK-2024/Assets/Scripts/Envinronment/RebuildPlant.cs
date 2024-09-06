using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebuildPlant : MonoBehaviour, IInterractable, IDataPersistance
{
    [SerializeField] protected string id;
    private SpriteRenderer spriteRenderer;
    private PlayerCoreSystem coreSystem;
    private Collider2D plantCollider;
    [SerializeField] private UI_RebuildPlant ui;

    private float zeroAlpha = 0f;
    private float quaterAlpha = .25f;
    private float halfAlpha = 0.5f;
    private float fullAlpha = 1f;
    private float currentDuration = 0f;
    private float maxDuration = 3.3f;

    private bool canBeInterracted;
    private bool isInterracted;
    private bool donePlanted;

    public static event Action ActionOnDonePlanted;
    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantCollider = GetComponent<Collider2D>();
        ChangeAlphaValue(quaterAlpha);
    }
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }
    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        isInterracted = true;
        ui.SwitchCanvasToProcess(true);
    }
    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (isInterracted) return;
        if (!canBeInterracted) return;
        if (coreSystem == null)
        {
            isInterracted = false;
            currentDuration = 0f;
            ChangeAlphaValue(quaterAlpha);
            ui.ToggleAllCanvas(false);
        }
        else
        {
            ui.SwitchCanvasToProcess(false);
            ChangeAlphaValue(halfAlpha);
        }
        this.coreSystem = coreSystem;
    }
    private void ChangeAlphaValue(float value)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = value;
        spriteRenderer.color = currentColor;
    }
    private void OnDonePlanted()
    {
        currentDuration = 0f;
        donePlanted = true;
        ChangeAlphaValue(fullAlpha);
        OnDisableFunction();
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
    private void Update()
    {
        if (isInterracted)
        {
            currentDuration += Time.deltaTime;
            ui.ShowProcess(currentDuration / maxDuration);
            if(currentDuration > maxDuration)
            {
                OnDonePlanted();
            }
        }
    }

    public void LoadScene(GameData gameData)
    {
        
    }

    public void SaveScene(ref GameData gameData)
    {
        
    }
}
