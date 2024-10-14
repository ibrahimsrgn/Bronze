using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public FireMode _FireMode;
    public int BurstCount;
    public float BurstModeDelay;
    public float RateOfFire;
    public float BulletVelocity;
    public int BulletDamage;
    public LayerMask mask;

    [Header("Prefabs")]
    [SerializeField] public Transform AmmoExitLoc;
    [SerializeField] public Light MuzzleLight;
    [SerializeField] public ParticleSystem MuzzleFlash;
    [SerializeField] public ParticleSystem BulletImpact;
    [SerializeField] public TrailRenderer BulletTrail;

    public bool ReadyToShoot = true;
    [HideInInspector]
    public PlayerData _PlayerData;
    [HideInInspector]
    public float RateOfFireData;
    [HideInInspector]
    public int BurstCountData;
    [HideInInspector]
    public WeaponShooting _Shooting;
    [HideInInspector]
    public bool BurstCoroutineOn;
    [HideInInspector]
    public BulletScript _BulletScript;

    private void Awake()
    {
        _BulletScript = FindAnyObjectByType<BulletScript>();
        _Shooting = FindObjectOfType<WeaponShooting>();
        _PlayerData = FindObjectOfType<PlayerData>();
        RateOfFireData = RateOfFire;
        BurstCountData = BurstCount;
    }
    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }
}
