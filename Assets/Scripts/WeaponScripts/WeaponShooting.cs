using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    private WeaponType weaponType;

    private void Awake()
    {
        weaponType = GetComponent<WeaponType>();
    }

    public void SingleShoot()
    {
        Shoot();
        weaponType.ReadyToShoot = false;
    }

    public void BurstShoot()
    {
        if (weaponType.BurstCount-- > 0)
        {
            Invoke("Shoot", 1f);
            BurstShoot();
        }
        else
        {
            weaponType.ReadyToShoot = false;
            weaponType.BurstCount = weaponType.BurstCountData;
        }
    }

    public void AutoShoot()
    {
        Shoot();
    }

    public void Shoot()
    {
        Transform Deneme = Instantiate(weaponType.AmmoPrefab, weaponType.AmmoExitLoc.transform.position, weaponType.AmmoExitLoc.transform.localRotation, null);
    }
}
