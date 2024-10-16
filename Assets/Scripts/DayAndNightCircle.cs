using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DayAndNightCircle : MonoBehaviour
{
    [SerializeField] private Texture2D Sunrise;
    [SerializeField] private Texture2D Daylight;
    [SerializeField] private Texture2D Sunset;
    [SerializeField] private Texture2D Night;

    [SerializeField] private Gradient ColorSunrise;
    [SerializeField] private Gradient ColorDaylight;
    [SerializeField] private Gradient ColorSunset;
    [SerializeField] private Gradient ColorNight;
    [SerializeField] private Light Sun;

    public Transform DayNightCircle;
    public float Minute;
    public int Hours;
    [Header("Day & Night Circle Per min")]
    public int HoursMultiplier;

    private float Second;
    private float AngleManager;

    private void Update()
    {
        Second += Time.deltaTime * (1440 / HoursMultiplier);
        if (Second >= 60)
        {
            Minute += 1;
            Second = Second - 60;
            if (Minute >= 60)
            {
                Hours += 1;
                Minute = 0;
                OnHoursChange(Hours);
                if (Hours >= 24)
                {
                    Hours = 0;
                }
            }
        }
        AngleManager = ((Hours - 6) * 15f) + (Minute * 0.25f) + (0.0041666666666667f * Second);
        DayNightCircle.transform.eulerAngles = Quaternion.Euler(AngleManager, AngleManager / 4, 0).eulerAngles;
        Debug.Log($"{Hours}, {Minute}, {Second} ---- {DayNightCircle.transform.eulerAngles.x}");
    }

    private void OnHoursChange(int hours)
    {
        if (hours == 6)
        {
            StartCoroutine(SkyBoxLerp(Night, Sunrise, 10));
            StartCoroutine(SunColorLerp(ColorSunrise, 10));
        }
        else if (hours == 8)
        {
            StartCoroutine(SkyBoxLerp(Sunrise, Daylight, 10));
            StartCoroutine(SunColorLerp(ColorDaylight, 10));
        }
        else if (hours == 18)
        {
            StartCoroutine(SkyBoxLerp(Daylight, Sunset, 10));
            StartCoroutine(SunColorLerp(ColorSunset, 10));

        }
        else if (hours == 22)
        {
            StartCoroutine(SkyBoxLerp(Sunset, Night, 10));
            StartCoroutine(SunColorLerp(ColorNight, 10));
        }
    }

    private IEnumerator SkyBoxLerp(Texture2D a, Texture2D b, float time)
    {

        yield return null;
    }

    private IEnumerator SunColorLerp(Gradient LightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            Sun.color = LightGradient.Evaluate(i / time);
            yield return null;
        }
    }
}
