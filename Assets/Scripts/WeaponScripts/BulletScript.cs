using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private WeaponType weaponType;
    private RaycastHit Hit;
    private Vector3 TargetPoint;
    private Vector3 StartPosition;

    private void Awake()
    {
        weaponType = GetComponent<WeaponType>();
    }

    public void RayShoot()
    {
        weaponType.MuzzleFlash.Play();
        if (Physics.Raycast(weaponType._PlayerData.Ray, out Hit, Mathf.Infinity, weaponType.mask))
        {
            TargetPoint = Hit.point;
        }
        else
        {
            TargetPoint = weaponType._PlayerData.Ray.GetPoint(1000);
        }

        TrailRenderer Trail = Instantiate(weaponType.BulletTrail, weaponType.AmmoExitLoc.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(Trail, TargetPoint));
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 HitLocation)
    {
        StartPosition = trail.transform.position;

        float distance = Vector3.Distance(StartPosition, HitLocation);
        float travelTime = distance / weaponType.BulletVelocity;
        float elapsedTime = 0f;

        while (elapsedTime < travelTime)
        {
            float progress = elapsedTime / travelTime;
            trail.transform.position = Vector3.Lerp(StartPosition, HitLocation, progress);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        trail.transform.position = HitLocation;
        Instantiate(weaponType.BulletImpact, HitLocation, Quaternion.LookRotation(HitLocation));

        GiveDamageToEnemy();

        Destroy(trail.gameObject, trail.time);
    }

    private void GiveDamageToEnemy()
    {
        HealthManager Deneme = Hit.collider.GetComponent<HealthManager>();
        if (Deneme != null )
        {
            Deneme.TakeDamage(weaponType.BulletDamage);
        }
    }
}
