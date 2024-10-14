using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DayAndNightCircle : MonoBehaviour
{
    [SerializeField] private Texture2D Daylight;
    [SerializeField] private Texture2D Sunset;
    [SerializeField] private Texture2D Night;
    [SerializeField] private Texture2D Sunrise;
    [SerializeField] private Skybox SkyBox;

    [SerializeField] private Gradient ColorDaylight;
    [SerializeField] private Gradient ColorSunset;
    [SerializeField] private Gradient ColorNight;
    [SerializeField] private Gradient ColorSunrise;
    [SerializeField] private Light Light;

    public Transform DayNightCircle;
    public float Minute;
    public int Hours;
    [Header("Day & Night Circle Per min")]
    public int HoursMultiplier;

    private float Second;

    private void Update()
    {
        Second += Time.deltaTime * (1440 /  HoursMultiplier);
        if (Second >= 60)
        {
            Minute += 1;
            Second = Second - 60;
            if (Minute >= 60)
            {
                Hours += 1;
                Minute = 0;
                if (Hours >= 24)
                {
                    Hours = 0;
                }
            }
        }
        DayNightCircle.transform.eulerAngles = Quaternion.Euler(((Hours - 6) * 15f) + (Minute * 0.25f) + (0.0041666666666667f * Second), 0, 0).eulerAngles;
        //Debug.Log($"{Hours}, {Minute}, {Second} ---- {DayNightCircle.transform.eulerAngles.x}");
    }

    /*private void OnHoursChange(int hours)
    {
        if (hours == 6)
        {
            StartCoroutine(SkyBoxLerp(NightToSunrise, SunriseToDaylight, 0));
            StartCoroutine(SunColorLerp(ColorNightToSunrise, ColorSunriseToDaylight, 0));
        }
        else if (hours == 8)
        {
            StartCoroutine(SkyBoxLerp(SunriseToDaylight, DaylightToSunset, 0));
            StartCoroutine(SunColorLerp(ColorSunriseToDaylight, ColorDaylightToSunset, 0));
        }
        else if (hours == 18)
        {
            StartCoroutine(SkyBoxLerp(DaylightToSunset, SunsetToNight, 0));
            StartCoroutine(SunColorLerp(ColorDaylightToSunset, ColorSunsetToNight, 0));

        }
        else if (hours == 22)
        {
            StartCoroutine(SkyBoxLerp(SunsetToNight, NightToSunrise, 0));
            StartCoroutine(SunColorLerp(ColorSunsetToNight, ColorNightToSunrise, 0));
        }
    }

    private IEnumerator SkyBoxLerp(Texture2D a, Texture2D b, float time)
    {
        
        yield return new WaitForSeconds(time);
    }*/

    private IEnumerator SunColorLerp(Gradient a, Gradient b, float time)
    {
        yield return new WaitForSeconds(time);
    }
}
