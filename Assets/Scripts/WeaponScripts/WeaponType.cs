using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public FireMode _FireMode;
    public int BurstCount;
    public float BurstModeDelay;
    public float RateOfFire;

    [Header("Prefabs")]
    [SerializeField] public Transform AmmoExitLoc;
    [SerializeField] public Transform AmmoPrefab;

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
    private void Awake()
    {
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
