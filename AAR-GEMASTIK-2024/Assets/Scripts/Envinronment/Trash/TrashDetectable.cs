using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDetectable : TrashBase, IDetectable
{
    [SerializeField] protected float movementSpeed;
    public void DetectedByPlayer(PlayerCoreSystem playerCoreSystem)
    {
        this.playerCoreSystem = playerCoreSystem;
        //MoveTowards();
    }

    private void Update()
    {
        if(!collected) MoveTowards();
    }
    private void MoveTowards()
    {
        if (playerCoreSystem == null) return;
        Vector3 direction = (playerCoreSystem.transform.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCoreSystem playerCoreSystem))
        {
            OnTakenByPlayer();
        }
    }
}
