using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private WeaponType weaponType;
    private void Awake()
    {
        weaponType = GetComponent<WeaponType>();
    }
    void Update()
    {
        Ray Ray = new Ray(weaponType.AmmoExitLoc.position, weaponType.AmmoExitLoc.TransformDirection(Vector3.forward));
        RaycastHit Hit;

        if (Physics.Raycast(Ray, out Hit, 100))
        {
            Debug.DrawRay(Ray.origin, Ray.direction * Hit.distance, Color.yellow);
        }
    }
}
