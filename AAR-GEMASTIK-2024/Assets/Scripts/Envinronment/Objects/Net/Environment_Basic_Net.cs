using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Basic_Net : MonoBehaviour
{
    [SerializeField] protected int maxAttemptToRecover;
    [SerializeField] protected float disabledDuration;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected AudioClip onGrabPlayer;
    [SerializeField] protected AudioClip onReleasedPlayer;
    protected PlayerCoreSystem coreSystem;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter something");
        if (collision.gameObject.TryGetComponent<PlayerCoreSystem>(out PlayerCoreSystem coreSystem))
        {
            this.coreSystem = coreSystem;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            AudioManager.Instance?.PlaySFX(onGrabPlayer);
            coreSystem.OnDisableMove(disabledDuration, maxAttemptToRecover);
            this.coreSystem.OnBreakingFree += CoreSystem_OnBreakingFree;
            transform.position = this.coreSystem.transform.position;
        }
    }
    private void CoreSystem_OnBreakingFree()
    {
        AudioManager.Instance?.PlaySFX(onReleasedPlayer);
        this.coreSystem.OnBreakingFree -= CoreSystem_OnBreakingFree;
        coreSystem = null;
        Destroy(gameObject);
    }
    private void Update()
    {
        if(coreSystem != null) transform.position = coreSystem.transform.position;
    }
}
