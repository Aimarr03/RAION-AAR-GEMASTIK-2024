using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;
    [SerializeField] private WeaponBase baseWeapon;
    [SerializeField] private Transform weaponHolderPosition;
    private PlayerCoreSystem playerCoreSystem;
    private void Awake()
    {
        playerCoreSystem = GetComponent<PlayerCoreSystem>();
        if(weaponSo != null)
        {
            Transform weaponInstantiate = Instantiate(weaponSo.weapon, weaponHolderPosition); 
            baseWeapon = weaponInstantiate.GetComponent<WeaponBase>();
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
        baseWeapon.Fire(this);
    }

    public void SetWeaponSO(WeaponSO weaponSo)
    {
        this.weaponSo = weaponSo;
    }
}
