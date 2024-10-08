using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpedictionManager : MonoBehaviour, IInterractable
{
    public static ExpedictionManager Instance;
    public event Action<bool, PlayerCoreSystem> OnDoneExpediction;
    public event Action<float> OnCollectedTrash;
    public event Action<IDelivarable> OnCaughtFishDelivarable;
    public event Action<string> OnLose;
    private bool isLosing;
    private PlayerCoreSystem _playerCoreSystem;
    
    [SerializeField] private Transform UI_OnWantToFinish;
    [SerializeField] private TextMeshProUGUI UI_Text_OnWantToFinish;
    [SerializeField] private string ekspedisiSelesai = "Usai Ekspediksi?";
    [SerializeField] private string masukkanIkan = "Klik E untuk Masukkan Ikan";
    [SerializeField] private string memindahkanSampah = "Dalam Proses memindahkan Sampah";
    public List<EnemyBase> CaughtFish;

    [Header("UI Progress")]
    [SerializeField] private Image foregroundProcess;
    [SerializeField] private RectTransform ProcessRectUI;
    private float currentProcess = 0f;
    private float maxDuration = 3f;
    private bool canBeInterracted = true;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip OnReceiving;
    [SerializeField] private AudioClip AudioLoseGame;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if(TutorialManager.instance != null)
        {
            canBeInterracted = false;
            TutorialManager.FinishTutorialAction += OnFinishTutorial;
        }
        isLosing = false;
        UI_OnWantToFinish.gameObject.SetActive(false);
        CaughtFish = new List<EnemyBase>();
    }
    private void OnDisable()
    {
        TutorialManager.FinishTutorialAction -= OnFinishTutorial;
    }
    private void OnFinishTutorial()
    {
        canBeInterracted = true;
        TutorialManager.FinishTutorialAction -= OnFinishTutorial;
    }
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        if (!canBeInterracted) return;
        /*if (playerInterractionSystem.IsHolding())
        {
            playerInterractionSystem.SetIsHolding(false);
        }*/
        if(!playerInterractionSystem.IsHolding())
        {
            currentProcess = 0f;
            WeightSystem weightSystem = _playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity) as WeightSystem;
            float currentValue = weightSystem.GetCurrentValue();
            weightSystem.OnDecreaseValue(currentValue);
            ProcessRectUI.gameObject.SetActive(false);
            OnDoneExpediction?.Invoke(true, _playerCoreSystem);
            OnCollectedTrash?.Invoke(currentValue);
            _playerCoreSystem.OnDisableMove();
        }
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        if (_playerCoreSystem != coreSystem)
        {
            _playerCoreSystem = coreSystem;
        }
        if (_playerCoreSystem == null)
        {
            UI_OnWantToFinish.gameObject.SetActive(false);
            ProcessRectUI.gameObject.SetActive(false);
            currentProcess = 0f;
        }
        else
        {
            currentProcess = 0f;
            if (_playerCoreSystem.interractionSystem.IsHolding()) UI_Text_OnWantToFinish.GetComponent<TextMeshProUGUI>().text = masukkanIkan;
            else if(_playerCoreSystem.interractionSystem.IsHolding() && canBeInterracted) UI_Text_OnWantToFinish.GetComponent<TextMeshProUGUI>().text = ekspedisiSelesai;
            UI_OnWantToFinish.gameObject.SetActive(true);
            ProcessRectUI.gameObject.SetActive(false);
        }
    }

    public void OnGainCaughtFish(EnemyBase fish)
    {
        CaughtFish.Add(fish);
        fish.OnDelivered();
    }
    public void OnGainCaughtFish(IDelivarable fish)
    {
        OnCaughtFishDelivarable?.Invoke(fish);
        AudioManager.Instance?.PlaySFX(OnReceiving);
    }
    private void Update()
    {
        if (_playerCoreSystem == null) return;
        if(_playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).GetCurrentValue() > 0 && !_playerCoreSystem.interractionSystem.IsHolding())
        {
            ProcessRectUI.gameObject.SetActive(true);
            currentProcess += Time.deltaTime;
            foregroundProcess.fillAmount = currentProcess / maxDuration;
            UI_Text_OnWantToFinish.GetComponent<TextMeshProUGUI>().text = memindahkanSampah;
            if (currentProcess >= maxDuration)
            {
                currentProcess = 0f;
                WeightSystem weightSystem = _playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity) as WeightSystem;
                float currentValue = weightSystem.GetCurrentValue();
                weightSystem.OnDecreaseValue(currentValue);
                ProcessRectUI.gameObject.SetActive(false);
                OnCollectedTrash?.Invoke(currentValue);
                UI_Text_OnWantToFinish.GetComponent<TextMeshProUGUI>().text = ekspedisiSelesai;
                AudioManager.Instance?.PlaySFX(OnReceiving);
            }
        }
    }
    public void InvokeOnLose(string deskripsi)
    {
        isLosing = true;
        AudioManager.Instance.PlaySFX(AudioLoseGame);
        OnLose?.Invoke(deskripsi);
    }
    public bool IsLosing() => isLosing;
}
