using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCoreSub : MonoBehaviour
{
    public TrashCore trashCore;
    [SerializeField] private float weight;
    [SerializeField] private AudioClip WhenTaken;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OnTaken(PlayerCoreSystem coreSystem)
    {
        coreSystem.GetSustainabilitySystem(SustainabilityType.Capacity).OnIncreaseValue(weight);
        AudioManager.Instance?.PlaySFX(WhenTaken);
        spriteRenderer.enabled = false;
    }

}
