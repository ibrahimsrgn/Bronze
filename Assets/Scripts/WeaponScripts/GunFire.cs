using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : WeaponType
{
    private void Update()
    {
        RateOfFire -= Time.deltaTime;
        if (_PlayerData.MouseClickInput && RateOfFire <= 0 && ReadyToShoot)
        {
            switch (_FireMode)
            {
                case FireMode.Single:
                    _Shooting.SingleShoot();
                    break;
                case FireMode.Burst:
                    _Shooting.BurstShoot();
                    break;
                case FireMode.Auto:
                    _Shooting.AutoShoot();
                    break;
            }
            RateOfFire = RateOfFireData;
        }
        else if (!_PlayerData.MouseClickInput)
        {
            ReadyToShoot = true;
        }
    }
}
