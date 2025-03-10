using InfimaGames.LowPolyShooterPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    #region Variables
    public int id;
    public int usableAmmoId;
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
    public int CurrentAmmoCount;

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
    [SerializeField] public Animator Animator;

    public AudioSource Shooting_Sound;
    public AudioSource Reload_Sound;
    private bool ReadyToShoot = true;
    private float RPMData;
    private int BurstCountData;
    private bool BurstCoroutineOn;
    private Ray _Ray;
    private RaycastHit Hit;
    private RaycastHit WallChecker;
    private Vector3 TargetPoint;
    private Vector3 StartPosition;
    private Recoil Recoil_Script;
    private int usableAmmoCount2;
    private bool readyToReload = true;

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
    private void Start()
    {
        if(gameObject.GetComponentInParent<PlayerData>() != null) 
            { 
                return;
            }
        InventoryManager.instance.RefreshMaxAmmoUI();
        InventoryManager.instance.RefreshCurrentAmmoUI(CurrentAmmoCount);
    }

    private void FixedUpdate()
    {
        if (transform.parent != null && transform.parent.name == "WeaponLoc")
        {
            //Debug.Log(MagazineCap + " / " + CurrentAmmoCount);
            _Ray = new Ray(AmmoExitLoc.position, AmmoExitLoc.TransformDirection(Vector3.forward));
            if (Physics.Raycast(_Ray, out WallChecker, 0.75f, mask))
            {
                Recoil_Script.WeaponTargetLocalizer(true);
            }
            else
            {
                Recoil_Script.WeaponTargetLocalizer(false);
            }
            RPM -= Time.deltaTime;
            if (_PlayerData.MouseClickInput && RPM <= 0 && ReadyToShoot && CurrentAmmoCount > 0 && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.ReloadAnimation"))
            {
                Shooting_Sound.PlayOneShot(Shooting_Sound.clip);
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

            if (_PlayerData.OnReloadBool && CurrentAmmoCount < MagazineCap&&readyToReload&&InventoryManager.instance.totalAmmoCount>0)
            {
                readyToReload=false;
                ReadyToShoot = false;
                Reload();
                PlayerData.Instance.OnReloadBool=false;
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
        Animator.SetTrigger("Shooting");
        CurrentAmmoCount--;
        InventoryManager.instance.RefreshCurrentAmmoUI(CurrentAmmoCount);
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
        if (playerData != null)
        {
            RigidBody = GetComponent<Rigidbody>();
            GUIActivater = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                InventoryManager.instance.AddItem(ItemPool.instance.items[id].itemSO,out GameObject addedItem);
                addedItem.GetComponent<InventoryItem>().itemPrefab = gameObject;
                Debug.Log(addedItem);

                Destroy(RigidBody);
                transform.SetParent(playerData.WeaponLoc.transform);
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
        Reload_Sound.Play();
        PlayerData.Instance.canChangeWeapon = false;
        int requiredAmmo = MagazineCap - CurrentAmmoCount;
        int usableAmmoCount = InventoryManager.instance.ReloadMagazine(MagazineCap, requiredAmmo);
        if ( usableAmmoCount== 0)
        {
            return;
        }
        else
        {
            usableAmmoCount2 = usableAmmoCount;
            Animator.SetTrigger("Reload");
        }
        
    }
    public void ReloadEnd()
    {
        InventoryManager.instance.RefreshCurrentAmmoUI(usableAmmoCount2);
        CurrentAmmoCount = usableAmmoCount2;
        ReadyToShoot = true;
        readyToReload =true;
        InventoryManager.instance.RefreshMaxAmmoUI();
        PlayerData.Instance.canChangeWeapon = true;
    }


    //Shotgun i�in animasyonun 40. karesine at�lacak event ile +1 ammo yap�lacak
    //Oyuncu bozmad��� s�rece +1 ammodan sonra 10 kare geri sar�lacak animasyon ve mermiyi koydu�u yerde yine +1 ammo olacak 
    //Tamam� dolana kadar animasyon tekrar edecek
    #endregion
}
