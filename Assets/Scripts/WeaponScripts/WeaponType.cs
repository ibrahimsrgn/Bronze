using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public FireRate _FireRate;
    public float BurstCount;
    public float ShootingDelay;

    [HideInInspector]
    public PlayerData _PlayerData;
    [HideInInspector]
    public float ShootingDelayData;

    private void Awake()
    {
        _PlayerData = FindObjectOfType<PlayerData>();
        ShootingDelayData = ShootingDelay;
    }
    public enum FireRate
    {
        Single,
        Burst,
        Auto
    }
}
