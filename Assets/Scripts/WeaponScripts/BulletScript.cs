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
        if (Physics.Raycast(weaponType._Ray, out Hit, Mathf.Infinity, weaponType.mask))
        {
            TargetPoint = Hit.point;
        }
        else
        {
            TargetPoint = weaponType._Ray.GetPoint(1000);
        }

        TrailRenderer Trail = Instantiate(weaponType.BulletTrail, weaponType.AmmoExitLoc.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(Trail, TargetPoint));
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 HitLocation)
    {
        MuzzleEffect();
        StartPosition = trail.transform.position;
        Vector3 dirToEnemy = (HitLocation - StartPosition).normalized;
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
        if (Hit.transform != null)
        {
            Instantiate(weaponType.BulletImpact, HitLocation, Quaternion.Inverse(weaponType.AmmoExitLoc.rotation), Hit.transform.parent);
        }

        GiveDamageToEnemy(dirToEnemy);

        Destroy(trail.gameObject, trail.time);
    }

    private void GiveDamageToEnemy(Vector3 dirToEnemy)
    {
        if (Hit.collider != null)
        {
            HealthManager Deneme = Hit.collider.gameObject.GetComponentInParent<HealthManager>();
            if (Deneme != null)
            {
                Deneme.TakeDamage(weaponType.BulletDamage);
                if (Hit.collider.TryGetComponent<Rigidbody>(out Rigidbody hitBody))
                {
                    hitBody.AddForce(dirToEnemy * weaponType.BulletDamage, ForceMode.Impulse);
                }
            }
        }
    }

    private void MuzzleEffect()
    {
        weaponType.MuzzleFlash.Emit(50);
        weaponType.MuzzleLight.enabled = true;
        StartCoroutine(KillMuzzleLight());
    }

    public IEnumerator KillMuzzleLight()
    {
        yield return new WaitForSeconds(0.1f);
        weaponType.MuzzleLight.enabled = false;
    }
}
