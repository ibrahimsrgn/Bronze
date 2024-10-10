using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public FireMode _FireMode;
    public int BurstCount;
    public float BurstModeDelay;
    public float RateOfFire;

    public bool ReadyToShoot = true;
    [HideInInspector]
    public PlayerData _PlayerData;
    [HideInInspector]
    public float RateOfFireData;
    [HideInInspector]
    public int BurstCountData;

    private void Awake()
    {
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
