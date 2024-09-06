using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCore : MonoBehaviour, IInterractable, IDataPersistance
{
    [SerializeField] protected string id;
    [SerializeField] private float intervalDuration;
    [SerializeField] private UI_TrashCore UI_TrashCore;
    [SerializeField] private List<Sprite> phaseRenderers;
    [SerializeField] private SpriteRenderer visual;
    private Queue<Sprite> queueSpriteRenderers;
    public bool collected;
    private bool canBeTaken;
    private float currentDuration;

    private PlayerCoreSystem player;
    
    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    public static event Action TrashCoreCollected;

    private Collider2D trashCoreCollider;
    private void Awake()
    {
        trashCoreCollider = GetComponent<Collider2D>();
        queueSpriteRenderers = new Queue<Sprite>(phaseRenderers);
        visual.sprite = queueSpriteRenderers.Dequeue();
    }
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }
    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        if (canBeTaken)
        {
            canBeTaken = false;
            UI_TrashCore.OnSwitchingUI(canBeTaken);
            visual.sprite = queueSpriteRenderers.Dequeue();
            if(queueSpriteRenderers.Count == 0)
            {
                OnTaken();
            }
        }
        
    }
    public void LoadScene(GameData gameData)
    {
        
    }
    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        player = coreSystem;
        canBeTaken = false;
        currentDuration = 0f;
        if (player == null)
        {
            UI_TrashCore.OnEnableView(player);
        }
        else
        {
            UI_TrashCore.OnSwitchingUI(canBeTaken);
        }
    }
    public void SaveScene(ref GameData gameData)
    {
        
    }
    public void OnTaken()
    {
        trashCoreCollider.enabled = false;
        for(int index = 0; index < transform.childCount; index++)
        {
            Transform childCurrentTransform = transform.GetChild(index);
            childCurrentTransform.gameObject.SetActive(false);
        }
        collected = true;
        TrashCoreCollected?.Invoke();
    }
    private void Update()
    {
        if (player == null || canBeTaken) return;
        currentDuration += Time.deltaTime;
        UI_TrashCore.SetProcessLoadingValue(currentDuration / intervalDuration);
        if(currentDuration >= intervalDuration)
        {
            canBeTaken = true;
            currentDuration = 0f;
            UI_TrashCore.OnSwitchingUI(canBeTaken);
        }
    }
    
}
