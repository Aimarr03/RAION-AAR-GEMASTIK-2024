using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TrashInterractable : TrashBase, IInterractable
{
    [SerializeField] private AudioClip OnTakenAudio;
    [SerializeField] private AudioClip OnCannotBeTaken;
    [SerializeField] private RectTransform DisplayCannotBeInterract;
    public void AltInterracted(PlayerInterractionSystem playerInterractionSystem)
    {
        return;
    }

    public void Interracted(PlayerInterractionSystem playerInterractionSystem)
    {
        WeightSystem weightSystem = playerCoreSystem.GetSustainabilitySystem(SustainabilityType.Capacity) as WeightSystem;
        if (!weightSystem.canAddWeight(weight))
        {
            AudioManager.Instance?.PlaySFX(OnCannotBeTaken);
            DisplayCannotBeInterracted();
            return;
        }
        OnTakenByPlayer();
        AudioManager.Instance?.PlaySFX(OnTakenAudio);
    }

    public void OnDetectedAsTheClosest(PlayerCoreSystem coreSystem)
    {
        playerCoreSystem = coreSystem;
    }

    private async void DisplayCannotBeInterracted()
    {
        DisplayCannotBeInterract.gameObject.SetActive(true);
        await Task.Delay(1200);
        DisplayCannotBeInterract.gameObject.SetActive(false);
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
