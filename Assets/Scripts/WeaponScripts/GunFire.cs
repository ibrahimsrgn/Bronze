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
        if (_PlayerData.MouseClickInput && ShootingDelay <= 0)
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
        }
    }

    private void SingleShoot()
    {
        Transform Deneme = Instantiate(AmmoPrefab, AmmoExitLoc.transform.position, AmmoExitLoc.transform.localRotation, null);
    }

    private void BurstShoot()
    {
        Debug.Log("TakTakTak");
    }

    private void AutoShoot()
    {
        Debug.Log("Tatatatatata");
    }
}
