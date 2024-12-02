using InfimaGames.LowPolyShooterPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    #region Variables
    public int id;
    private bool GUIActivater;
    private Rigidbody RigidBody;
    private PlayerData _PlayerData;

    [Header("Weapon Stats")]
    [SerializeField] private FireMode _FireMode;
    [SerializeField] private int BurstCount;
    [SerializeField] private float BurstModeDelay;
    [SerializeField] private float RPM;
    [SerializeField] private float BulletVelocity;
    [SerializeField] private int BulletDamage;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int MagazineCap;
    private int CurrentAmmoCount;

    [Header("Prefabs")]
    [SerializeField] private Transform AmmoExitLoc;
    [SerializeField] private Light MuzzleLight;
    [SerializeField] private ParticleSystem MuzzleFlash;
    [SerializeField] private ParticleSystem BulletImpact;
    [SerializeField] private TrailRenderer BulletTrail;

    [Header("Referances")]
    [SerializeField] public Transform RightHandRigRef;
    [SerializeField] public Transform LeftHandRigRef;
    [SerializeField] public Transform AimCamLocRef;
    [SerializeField] public Transform WeaponLocRef;
    [SerializeField] private Animator Animator;

    public AudioSource Shooting_Sound;
    private bool ReadyToShoot = true;
    private float RPMData;
    private int BurstCountData;
    private bool BurstCoroutineOn;
    private Ray _Ray;
    private RaycastHit Hit;
    private Vector3 TargetPoint;
    private Vector3 StartPosition;
    private Recoil Recoil_Script;


    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }

    #endregion

    //-------------------------------------------------------------------

    #region Update Methods

    private void Awake()
    {
        Recoil_Script = FindFirstObjectByType<Recoil>();
        if (transform.parent == null)
        {
            Animator.enabled = false;
        }
        RPMData = 60 / RPM;
        RPM = RPMData;
        BurstCountData = BurstCount;
        _PlayerData = FindFirstObjectByType<PlayerData>();
    }

    private void FixedUpdate()
    {
        if (transform.parent != null && transform.parent.name == "WeaponLoc")
        {
            Debug.Log(MagazineCap + " / " + CurrentAmmoCount);
            _Ray = new Ray(AmmoExitLoc.position, AmmoExitLoc.TransformDirection(Vector3.forward));
            RPM -= Time.deltaTime;
            if (_PlayerData.MouseClickInput && RPM <= 0 && ReadyToShoot && CurrentAmmoCount > 0)
            {
                Shooting_Sound.Play();
                switch (_FireMode)
                {
                    case FireMode.Single:
                        SingleShoot();
                        break;
                    case FireMode.Burst:
                        StartCoroutine(BurstShoot());
                        break;
                    case FireMode.Auto:
                        RayShoot();
                        break;
                }
                RPM = RPMData;
            }
            else if (!_PlayerData.MouseClickInput && !BurstCoroutineOn)
            {
                ReadyToShoot = true;
            }

            if (_PlayerData.OnReloadBool && CurrentAmmoCount < MagazineCap)
            {
                ReadyToShoot = false;
                Reload();
            }
        }
    }
    #endregion

    //-------------------------------------------------------------------

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

        Recoil_Script.RecoilFire();
        CurrentAmmoCount--;
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

    //-------------------------------------------------------------------

    #region WeaponShooting

    public void SingleShoot()
    {
        RayShoot();
        ReadyToShoot = false;
    }

    public IEnumerator BurstShoot()
    {
        ReadyToShoot = false;
        BurstCoroutineOn = true;
        for (int i = 0; i < BurstCountData; i++)
        {
            RayShoot();
            yield return new WaitForSeconds(BurstModeDelay);
            if (i + 1 == BurstCountData)
            {
                BurstCoroutineOn = false;
            }
        }
    }
    #endregion

    //-------------------------------------------------------------------

    #region Weapon Interaction
    public void OnRayHit(PlayerData playerData)
    {
        if (playerData != null && transform.parent == null && playerData.ItemOnHand == null)
        {
            RigidBody = GetComponent<Rigidbody>();
            GUIActivater = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                 InventoryManager.instance.AddItem(ItemPool.instance.items[id].itemSO,out GameObject addedItem);
                Debug.Log(addedItem?.GetComponent<InventoryItem>().item.type);
                Debug.Log(addedItem);
                Destroy(RigidBody);
                transform.SetParent(playerData.WeaponLoc.transform);
                playerData.ItemOnHand = transform;

                playerData.LeftHandLayer.data.target = LeftHandRigRef;
                playerData.RightHandLayer.data.target = RightHandRigRef;

                transform.position = playerData.WeaponLoc.transform.position;
                transform.rotation = playerData.WeaponLoc.transform.rotation;

                playerData.WeaponPosRot.position = WeaponLocRef.position;

                playerData.CamPosRef2 = AimCamLocRef;
                Animator.enabled = true;
                playerData._RigBuilder.Build();
                addedItem.GetComponent<InventoryItem>().item.objPrefab = gameObject;
                gameObject.SetActive(false);
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

    //-------------------------------------------------------------------
    #region Reload

    public void Reload()
    {
        Animator.SetTrigger("Reload");
        CurrentAmmoCount = MagazineCap;
        ReadyToShoot = true;
    }
    //Shotgun için animasyonun 40. karesine atýlacak event ile +1 ammo yapýlacak
    //Oyuncu bozmadýðý sürece +1 ammodan sonra 10 kare geri sarýlacak animasyon ve mermiyi koyduðu yerde yine +1 ammo olacak 
    //Tamamý dolana kadar animasyon tekrar edecek
    #endregion
}
