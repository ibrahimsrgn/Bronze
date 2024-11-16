using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    #region Variables
    private bool GUIActivater;
    private Rigidbody RigidBody;
    private PlayerData _PlayerData;

    [SerializeField] private FireMode _FireMode;
    [SerializeField] private int BurstCount;
    [SerializeField] private float BurstModeDelay;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float BulletVelocity;
    [SerializeField] private int BulletDamage;
    [SerializeField] private LayerMask mask;

    [Header("Prefabs")]
    [SerializeField] private Transform AmmoExitLoc;
    [SerializeField] private Light MuzzleLight;
    [SerializeField] private ParticleSystem MuzzleFlash;
    [SerializeField] private ParticleSystem BulletImpact;
    [SerializeField] private TrailRenderer BulletTrail;

    [Header("Referances")]
    [SerializeField] private Transform RightHandRigRef;
    [SerializeField] private Transform LeftHandRigRef;
    [SerializeField] private Transform AimCamLocRef;

    private bool ReadyToShoot = true;
    private float RateOfFireData;
    private int BurstCountData;
    private bool BurstCoroutineOn;
    private Ray _Ray;
    private RaycastHit Hit;
    private Vector3 TargetPoint;
    private Vector3 StartPosition;

    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }

    #endregion

    #region Update Methods
    private void Awake()
    {
        RateOfFireData = RateOfFire;
        BurstCountData = BurstCount;
        _PlayerData = FindFirstObjectByType<PlayerData>();
    }

    private void Update()
    {
        if (transform.parent != null && transform.parent.name == "WeaponLoc")
        {
            _Ray = new Ray(AmmoExitLoc.position, AmmoExitLoc.TransformDirection(Vector3.forward));
            RateOfFire -= Time.deltaTime;
            if (_PlayerData.MouseClickInput && RateOfFire <= 0 && ReadyToShoot)
            {
                switch (_FireMode)
                {
                    case FireMode.Single:
                        SingleShoot();
                        break;
                    case FireMode.Burst:
                        StartCoroutine(BurstShoot());
                        break;
                    case FireMode.Auto:
                        Shoot();
                        break;
                }
                RateOfFire = RateOfFireData;
            }
            else if (!_PlayerData.MouseClickInput && !BurstCoroutineOn)
            {
                ReadyToShoot = true;
            }
        }
    }
    #endregion

    #region BulletScript
    public void RayShoot()
    {
        if (Physics.Raycast(_Ray, out Hit, Mathf.Infinity, mask))
        {
            TargetPoint = Hit.point;
        }
        else
        {
            TargetPoint = _Ray.GetPoint(1000);
        }

        TrailRenderer Trail = Instantiate(BulletTrail, AmmoExitLoc.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(Trail, TargetPoint));
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 HitLocation)
    {
        MuzzleEffect();
        StartPosition = trail.transform.position;
        Vector3 dirToEnemy = (HitLocation - StartPosition).normalized;
        float distance = Vector3.Distance(StartPosition, HitLocation);
        float travelTime = distance / BulletVelocity;
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
            Instantiate(BulletImpact, HitLocation, Quaternion.Inverse(AmmoExitLoc.rotation), Hit.transform.parent);
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
                Deneme.TakeDamage(BulletDamage);
                if (Hit.collider.TryGetComponent<Rigidbody>(out Rigidbody hitBody))
                {
                    hitBody.AddForce(dirToEnemy * BulletDamage, ForceMode.Impulse);
                }
            }
        }
    }

    private void MuzzleEffect()
    {
        MuzzleFlash.Emit(50);
        MuzzleLight.enabled = true;
        StartCoroutine(KillMuzzleLight());
    }

    public IEnumerator KillMuzzleLight()
    {
        yield return new WaitForSeconds(0.1f);
        MuzzleLight.enabled = false;
    }
    #endregion

    #region WeaponShooting
    public void SingleShoot()
    {
        Shoot();
        ReadyToShoot = false;
    }

    public IEnumerator BurstShoot()
    {
        ReadyToShoot = false;
        BurstCoroutineOn = true;
        for (int i = 0; i < BurstCountData; i++)
        {
            Shoot();
            yield return new WaitForSeconds(BurstModeDelay);
            if (i + 1 == BurstCountData)
            {
                BurstCoroutineOn = false;
            }
        }
    }

    public void Shoot()
    {
        RayShoot();
    }
    #endregion

    #region Weapon Interaction
    public void OnRayHit(PlayerData playerData)
    {
        if (playerData != null && transform.parent == null && playerData.ItemOnHand == null)
        {
            RigidBody = GetComponent<Rigidbody>();
            GUIActivater = true;
            if (Input.GetKeyDown(KeyCode.E))
            {

                Destroy(RigidBody);
                transform.position = playerData.WeaponLoc.transform.position;
                transform.rotation = playerData.WeaponLoc.transform.rotation;

                playerData.LeftHandRigData = LeftHandRigRef;
                playerData.RightHandRigData = RightHandRigRef;

                playerData.CamPosRef2 = AimCamLocRef;

                transform.SetParent(playerData.WeaponLoc.transform);
                playerData.ItemOnHand = transform;
            }
        }
    }

    void OnGUI()
    {
        if (GUIActivater)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Press 'E' to collect gun");
        }
    }
    private void OnMouseExit()
    {
        GUIActivater = false;
    }
    #endregion
}
