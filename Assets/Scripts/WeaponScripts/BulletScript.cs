using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private WeaponType weaponType;
    Ray Ray;
    private void Awake()
    {
        weaponType = GetComponent<WeaponType>();
    }
    void FixedUpdate()
    {
        Ray = new Ray(weaponType.AmmoExitLoc.position, weaponType.AmmoExitLoc.TransformDirection(Vector3.forward));
    }

    public void RayShoot()
    {
        if (Physics.Raycast(Ray, out RaycastHit Hit))
        {
            Debug.DrawRay(Ray.origin, Hit.distance * Ray.direction, Color.yellow);
            Instantiate(weaponType.AmmoPrefab, Hit.point, weaponType.AmmoExitLoc.rotation, null);
        }
    }
}
