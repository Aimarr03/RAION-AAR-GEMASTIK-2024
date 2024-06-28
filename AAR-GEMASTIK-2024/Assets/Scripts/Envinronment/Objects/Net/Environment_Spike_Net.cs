using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Spike_Net : MonoBehaviour
{
    [SerializeField] protected int maxAttemptToRecover;
    [SerializeField] protected float disabledDuration;
    [SerializeField] protected int damage;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected AudioClip onGrabPlayer;
    [SerializeField] protected AudioClip onReleasedPlayer;
    protected PlayerCoreSystem coreSystem;
    protected void OnTriggerEnter(Collider collision)
    {
        Debug.Log("enter something");
        if (collision.gameObject.TryGetComponent<PlayerCoreSystem>(out PlayerCoreSystem coreSystem))
        {
            this.coreSystem = coreSystem;
            coreSystem.OnDisableMove(disabledDuration, maxAttemptToRecover);
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            AudioManager.Instance?.PlaySFX(onGrabPlayer);
            this.coreSystem.OnBreakingFree += CoreSystem_OnBreakingFree;
            transform.position = this.coreSystem.transform.position;
        }
    }

    private void CoreSystem_OnBreakingFree()
    {
        AudioManager.Instance?.PlaySFX(onReleasedPlayer);
        this.coreSystem.OnBreakingFree += CoreSystem_OnBreakingFree;
        coreSystem = null;
        Destroy(gameObject);
    }
    private void Update()
    {
        if (coreSystem != null)
        {
            coreSystem.TakeDamage(damage);
            transform.position = coreSystem.transform.position;
        }
    }
}
