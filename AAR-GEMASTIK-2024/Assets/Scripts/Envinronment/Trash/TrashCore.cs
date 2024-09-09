using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TrashCore : MonoBehaviour, IInterractable, IDataPersistance
{
    [SerializeField] protected string id;
    [SerializeField] private float intervalDuration;
    [SerializeField] private float currentIntervalDuration;
    [SerializeField] private int Phase = 0;
    [SerializeField] private UI_TrashCore UI_TrashCore;
    [SerializeField] private List<Sprite> phaseRenderers;
    [SerializeField] private SpriteRenderer visual;
    private Queue<Sprite> queueSpriteRenderers;
    public bool collected;
    private bool canBeTaken;
    private float currentDuration;
    private float percentageDuration => currentDuration/currentIntervalDuration;
    
    private float ConstraintMaxValue = 0.4f;
    private float currentMinValue;
    private float currentMaxValue;

    private PlayerCoreSystem player;
    [SerializeField] private List<AudioClip> audioClipList;
    [SerializeField] private AudioClip startRemoving, failedRemoving;
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
    private void Start()
    {
        UI_TrashCore.OnEnableView(player);
    }
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }
    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        if (canBeTaken)
        {
            bool failed = percentageDuration < currentMinValue || percentageDuration > currentMinValue + currentMaxValue;
            Debug.Log("Taken Success ? " + !failed);
            if (failed)
            {
                player.TakeDamage(10);
                CooldownToBeTaken();
                return;
            }
            if (queueSpriteRenderers.Count == 0)
            {
                AudioManager.Instance.PlaySFX(audioClipList[UnityEngine.Random.Range(0, audioClipList.Count)]);
                OnTaken();
                return;
            }
            AudioManager.Instance.PlaySFX(audioClipList[UnityEngine.Random.Range(0, audioClipList.Count)]);
            visual.sprite = queueSpriteRenderers.Dequeue();
            Phase++;
            CalculateMaxAndMinValue();
        }
    }
    private void CalculateMaxAndMinValue()
    {
        currentDuration = 0f;
        currentMaxValue = ConstraintMaxValue - (0.065f * Phase);
        currentMinValue = UnityEngine.Random.Range(0.2f, 1 - currentMaxValue);
        UI_TrashCore.SetIndicationValue(currentMinValue, currentMaxValue);
        currentIntervalDuration = intervalDuration - (0.65f * Phase);
        canBeTaken = true;
    }
    public void LoadScene(GameData gameData)
    {
        SubLevelData subLevelData = DataManager.instance.gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (subLevelData == null) return;
        if (!subLevelData.additionalCollectableObjects.ContainsKey("Trash Core"))
        {
            subLevelData.additionalCollectableObjects.Add("Trash Core", new SerializableDictionary<string, bool>());
        }
        SerializableDictionary<string, bool> trashCoreDictionary = subLevelData.additionalCollectableObjects["Trash Core"];
        if (trashCoreDictionary.TryGetValue(id, out bool value))
        {
            if (value)
            {
                trashCoreCollider.enabled = false;
                for (int index = 0; index < transform.childCount; index++)
                {
                    Transform childCurrentTransform = transform.GetChild(index);
                    childCurrentTransform.gameObject.SetActive(false);
                }
                collected = true;
                canBeTaken = false;
            }
        }
    }
    public void SaveScene(ref GameData gameData)
    {
        SubLevelData subLevelData = DataManager.instance.gameData.GetSubLevelData(GameManager.Instance.currentLevelChoice);
        if (subLevelData == null) return;
        if(!subLevelData.additionalCollectableObjects.ContainsKey("Trash Core"))
        {
            subLevelData.additionalCollectableObjects.Add("Trash Core", new SerializableDictionary<string, bool>());
        }
        SerializableDictionary<string, bool> trashCoreDictionary = subLevelData.additionalCollectableObjects["Trash Core"];
        if (trashCoreDictionary.ContainsKey(id))
        {
            trashCoreDictionary.Remove(id);
        }
        trashCoreDictionary.Add(id, collected);
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
            AudioManager.Instance.PlaySFX(startRemoving);
            CalculateMaxAndMinValue();
        }
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
        canBeTaken = false;
        TrashCoreCollected?.Invoke();
    }
    private void Update()
    {
        if (player == null || !canBeTaken) return;
        currentDuration += Time.deltaTime;
        UI_TrashCore.SetProcessLoadingValue(currentDuration / currentIntervalDuration);
        if(currentDuration >= currentIntervalDuration)
        {
            currentDuration = 0f;
            UI_TrashCore.OnEnableView(null);
            CooldownToBeTaken();
        }
    }
    private async void CooldownToBeTaken()
    {
        canBeTaken = false;
        AudioManager.Instance.PlaySFX(failedRemoving);
        await Task.Delay(1500);
        canBeTaken = true;
        UI_TrashCore.OnSwitchingUI(false);
    }
}
