using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class WakingAnimationController : MonoBehaviour
{
    public PlayerData PlayerData;
    public CinemachinePanTilt CinemachinePanTilt;
    void Start()
    {
        StartCoroutine(Waking());
    }

    IEnumerator Waking()
    {
        yield return new WaitForSeconds(5);

        Waked();
    }

    private void Waked()
    {
        CinemachinePanTilt.enabled = true;
        PlayerData.enabled = true;
    }
}
