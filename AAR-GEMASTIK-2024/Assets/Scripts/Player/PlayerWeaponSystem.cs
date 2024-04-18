using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;
    [SerializeField] private WeaponBase baseWeapon;
    [SerializeField] private Transform weaponHolderPosition;
    private PlayerCoreSystem playerCoreSystem;
    public event Action<float> DoneFire;
    private void Awake()
    {
        playerCoreSystem = GetComponent<PlayerCoreSystem>();
        if(weaponSo != null && baseWeapon == null)
        {
            Transform weaponInstantiate = Instantiate(weaponSo.weapon, weaponHolderPosition); 
            baseWeapon = weaponInstantiate.GetComponent<WeaponBase>();
            baseWeapon.SetPlayerCoreSystem(playerCoreSystem);
            baseWeapon.weaponSO = weaponSo;
            baseWeapon.SetObjectPooling(weaponSo);
            baseWeapon.SetUpData();
        }
    }

    private void Start()
    {
        PlayerInputSystem.InvokeWeaponUsage += PlayerInputSystem_InvokeWeaponUsage;
        playerCoreSystem.OnDead += PlayerCoreSystem_OnDead;
    }

    private void PlayerCoreSystem_OnDead()
    {
        PlayerInputSystem.InvokeWeaponUsage -= PlayerInputSystem_InvokeWeaponUsage;
    }

    private void PlayerInputSystem_InvokeWeaponUsage()
    {
        if (baseWeapon == null) return;
        bool isOnRightDirection = playerCoreSystem.moveSystem.GetIsOnRightDirection();
        baseWeapon.Fire(this, isOnRightDirection);
    }

    public void SetWeaponSO(WeaponSO weaponSo)
    {
        this.weaponSo = weaponSo;
        for (int index = 0; index < weaponHolderPosition.childCount; index++)
        {
            Transform currentChild = weaponHolderPosition.GetChild(index);
            Destroy(currentChild);
        }
        Transform weaponInstantiate = Instantiate(weaponSo.weapon, weaponHolderPosition);
        baseWeapon = weaponInstantiate.GetComponent<WeaponBase>();
        baseWeapon.SetPlayerCoreSystem(playerCoreSystem);
        baseWeapon.weaponSO = weaponSo;
        baseWeapon.SetObjectPooling(weaponSo);
        baseWeapon.SetUpData();
    }
    public WeaponSO GetWeaponSO() => weaponSo;
    public void TriggerDoneFire(float duration)
    {
        DoneFire?.Invoke(duration);
    }
}
