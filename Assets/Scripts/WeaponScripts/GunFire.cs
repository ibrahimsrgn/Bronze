using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : WeaponType
{
    [Header("Prefabs")]
    [SerializeField] private Transform AmmoExitLoc;
    [SerializeField] private Transform AmmoPrefab;

    private void Update()
    {
        ShootingDelay -= Time.deltaTime;
        if (_PlayerData.MouseClickInput && ShootingDelay <= 0 && ReadyToShoot)
        {
            switch (_FireRate)
            {
                case FireRate.Single:
                    SingleShoot();
                    break;
                case FireRate.Burst:
                    BurstShoot();
                    break;
                case FireRate.Auto:
                    AutoShoot();
                    break;
            }
            ShootingDelay = ShootingDelayData;
        }
        else if (!_PlayerData.MouseClickInput)
        {
            ReadyToShoot = true;
        }
    }

    private void SingleShoot()
    {
        Shoot();
        ReadyToShoot = false;
    }

    private void BurstShoot()
    {
        if (BurstCount-- > 0)
        {
            Shoot();
            BurstShoot();
        }
        else
        {
            ReadyToShoot = false;
            BurstCount = BurstCountData;
        }
    }

    private void AutoShoot()
    {
        Shoot();
    }

    private void Shoot()
    {
        Transform Deneme = Instantiate(AmmoPrefab, AmmoExitLoc.transform.position, AmmoExitLoc.transform.localRotation, null);
    }
}
