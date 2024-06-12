using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpedictionManager : MonoBehaviour, IInterractable
{
    public static ExpedictionManager Instance;
    public event Action<bool, PlayerCoreSystem> OnDoneExpediction;
    private PlayerCoreSystem _playerCoreSystem;
    [SerializeField] private Transform UI_OnWantToFinish;
    [SerializeField] private TextMeshProUGUI UI_Text_OnWantToFinish;
    [SerializeField] private string ekspedisiSelesai = "Usai Ekspediksi?";
    [SerializeField] private string masukkanIkan = "Klik E untuk Masukkan Ikan";
    public List<EnemyBase> CaughtFish;
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
        UI_OnWantToFinish.gameObject.SetActive(false);
        CaughtFish = new List<EnemyBase>();
    }
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        /*if (playerInterractionSystem.IsHolding())
        {
            playerInterractionSystem.SetIsHolding(false);
        }*/
        if(!playerInterractionSystem.IsHolding())
        {
            OnDoneExpediction?.Invoke(true, _playerCoreSystem);
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
        }
        else
        {
            if (_playerCoreSystem.interractionSystem.IsHolding()) UI_Text_OnWantToFinish.GetComponent<TextMeshProUGUI>().text = masukkanIkan;
            else UI_Text_OnWantToFinish.GetComponent<TextMeshProUGUI>().text = ekspedisiSelesai;
            UI_OnWantToFinish.gameObject.SetActive(true);
        }
    }

    public void OnGainCaughtFish(EnemyBase fish)
    {
        CaughtFish.Add(fish);
        fish.OnDelivered();
    }

    
}
