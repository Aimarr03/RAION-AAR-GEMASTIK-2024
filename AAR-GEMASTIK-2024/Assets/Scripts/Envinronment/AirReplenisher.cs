using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirReplenisher : MonoBehaviour
{
    private bool isPlayerWithinTheSurface;
    private Coroutine replenishAction;
    private PlayerCoreSystem playerCoreSystem;

    public AudioClip OnGoToSurface;
    public AudioClip OnEnterOcean;

    public static event Action<bool> OnResurface;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out PlayerCoreSystem coreSystem))
        {
            isPlayerWithinTheSurface = true;
            playerCoreSystem = coreSystem;
            replenishAction = StartCoroutine(ReplenishAction());
            Debug.Log("On replenish Oxygen Player");
            AudioManager.Instance.OnGraduallyStopUnderwaterSFX(0.4f);
            AudioManager.Instance.PlaySFX(OnGoToSurface);
            OnResurface?.Invoke(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerCoreSystem coreSystem))
        {
            playerCoreSystem = null;
            isPlayerWithinTheSurface = false;
            StopCoroutine(replenishAction);
            AudioManager.Instance.OnGraduallyStartUnderwaterSFX(0.3f);
            AudioManager.Instance.PlaySFX(OnEnterOcean);
            OnResurface?.Invoke(false);
            Debug.Log("On stop replenish Oxygen Player");
        }
    }
    private IEnumerator ReplenishAction()
    {
        while(isPlayerWithinTheSurface)
        {
            yield return new WaitForSeconds(1f);
            playerCoreSystem.OnReplenishOxygen(1);
            playerCoreSystem.OnReplenishEnergy(2);
            Debug.Log("Player Replenished Oxygen");
        }
    }
}
