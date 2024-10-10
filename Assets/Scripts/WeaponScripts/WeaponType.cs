using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public FireRate _FireRate;
    public int BurstCount;
    public float ShootingDelay;

    public bool ReadyToShoot = true;
    [HideInInspector]
    public PlayerData _PlayerData;
    [HideInInspector]
    public float ShootingDelayData;
    [HideInInspector]
    public int BurstCountData;

    private void Awake()
    {
        _PlayerData = FindObjectOfType<PlayerData>();
        ShootingDelayData = ShootingDelay;
        BurstCountData = BurstCount;
    }
    public enum FireRate
    {
        Single,
        Burst,
        Auto
    }
}
