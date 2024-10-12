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

    public IEnumerator BurstShoot()
    {
        weaponType.ReadyToShoot = false;
        weaponType.BurstCoroutineOn = true;
        for (int i = 0; i < weaponType.BurstCountData; i++)
        {
            Shoot();
            yield return new WaitForSeconds(weaponType.BurstModeDelay);
            if (i + 1 == weaponType.BurstCountData)
            {
                weaponType.BurstCoroutineOn = false;
            }
        }
    }

    public void AutoShoot()
    {
        Shoot();
    }

    public void Shoot()
    {
        weaponType._BulletScript.RayShoot();
        //Transform Deneme = Instantiate(weaponType.AmmoPrefab, weaponType.AmmoExitLoc.transform.position, weaponType.AmmoExitLoc.transform.localRotation, null);
    }
}
