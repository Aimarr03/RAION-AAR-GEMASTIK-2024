using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSo;
    [SerializeField] private BaseWeapon baseWeapon;
    [SerializeField] private Transform weaponHolderPosition;

    private void Start()
    {
        PlayerInputSystem.InvokeWeaponUsage += PlayerInputSystem_InvokeWeaponUsage;
    }

    private void PlayerInputSystem_InvokeWeaponUsage()
    {
        baseWeapon.Fire(this);
    }

    public void SetWeaponSO(WeaponSO weaponSo)
    {
        this.weaponSo = weaponSo;
    }
}
