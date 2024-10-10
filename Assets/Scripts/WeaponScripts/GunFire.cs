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
        RateOfFire -= Time.deltaTime;
        if (_PlayerData.MouseClickInput && RateOfFire <= 0 && ReadyToShoot)
        {
            switch (_FireMode)
            {
                case FireMode.Single:
                    SingleShoot();
                    break;
                case FireMode.Burst:
                    BurstShoot();
                    break;
                case FireMode.Auto:
                    AutoShoot();
                    break;
            }
            RateOfFire = RateOfFireData;
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
            Invoke("Shoot", 1f);
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
