using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines.Interpolators;

public class EndGameScript : MonoBehaviour
{
    [Header("Animaitons")]
    public Animator Animator;

    [Header("Events")]
    public CinemachinePanTilt CinemachinePanTilt;
    public DayAndNightCircle DayAndNightCircle;
    public GameObject ZombieHorde;

    [Header("Player Location Referances")]
    public Transform PlayerLocation;
    public Transform PlayerStatueLocation;
    public Transform PlayerBedLocation;

    [Header("Camera")]
    public Volume volume;
    private UnityEngine.Rendering.HighDefinition.ColorAdjustments _ColorAdjustments;
    private UnityEngine.Rendering.HighDefinition.Vignette _Vignette;
    public GameObject _Camera;
    public GameObject CameraFinalLoc;

    void Start()
    {
        if (volume.profile.TryGet<UnityEngine.Rendering.HighDefinition.Vignette>(out _Vignette))
        {
            _Vignette.intensity.overrideState = true;
        }

        if (volume.profile.TryGet<UnityEngine.Rendering.HighDefinition.ColorAdjustments>(out _ColorAdjustments))
        {
            _ColorAdjustments.colorFilter.overrideState = true;
        }
    }

    public void OnRayHit(PlayerData playerData)
    {
        Debug.Log("1");
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerData.enabled = false;
            CinemachinePanTilt.PanAxis.Wrap = false;
            CinemachinePanTilt.PanAxis.Range = new Vector2(-45, 45);
            PlayerLocation.position = PlayerStatueLocation.position;
            PlayerLocation.rotation = PlayerStatueLocation.rotation;
            Animator.Play("TouchStatue");
            ZombieHorde.SetActive(true);
            Destroy(playerData);
        }
    }

    public void ClosingEyesAndWaking()
    {
        StartCoroutine(CloseEyesRoutine());
    }

    private IEnumerator CloseEyesRoutine()
    {
        while (_Vignette.intensity.value < 0.99f)
        {
            _Vignette.intensity.value = Mathf.Lerp(_Vignette.intensity.value, 1, 0.1f);
            yield return null;
        }
        _Vignette.intensity.value = 1;
        _ColorAdjustments.colorFilter.value = Color.black;
        DayAndNightCircle.Hours = 22;
        DayAndNightCircle.OnHoursChange(22);
        PlayerLocation.transform.position = PlayerBedLocation.position;
        PlayerLocation.transform.rotation = PlayerBedLocation.rotation;
        _Camera.transform.rotation = PlayerBedLocation.rotation;
        Destroy(CinemachinePanTilt);
        StartCoroutine(Waking());
    }

    private IEnumerator Waking()
    {
        while (_Vignette.intensity.value > 0.259f)
        {
            _Vignette.intensity.value = Mathf.Lerp(_Vignette.intensity.value, 0.256f, 0.05f);
            yield return null;
        }
        _ColorAdjustments.colorFilter.value = Color.white;
        _Vignette.intensity.value = 0.256f;
    }

    public void LastStand()
    {
        StartCoroutine(RotateYOverTime(60f, 2f)); // Hedef açý 60, 2 saniye içinde tamamlanacak
    }

    private IEnumerator RotateYOverTime(float targetY, float duration)
    {
        // Baþlangýç rotasyonunu alýn
        Vector3 startRotation = _Camera.transform.rotation.eulerAngles;

        // Zaman takibi
        float elapsed = 0f;

        // Mevcut y rotasyonu
        float currentY = startRotation.y;

        // Hedef açýyý normalize et (0-360 arasý)
        targetY = (targetY + 360) % 360;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Mevcut açýyý normalize et
            currentY = (currentY + 360) % 360;

            // Kendi y ekseni ile hedef açý arasýndaki farký kontrol et
            float shortestPath = Mathf.DeltaAngle(currentY, targetY);

            // Yumuþak geçiþ
            float newY = currentY + shortestPath * (elapsed / duration);

            // Yeni rotasyonu uygula
            _Camera.transform.rotation = Quaternion.Euler(startRotation.x, newY, startRotation.z);

            yield return null;
        }

        // Hedef açýya tam olarak ulaþ
        _Camera.transform.rotation = Quaternion.Euler(startRotation.x, targetY, startRotation.z);
    }

}
