using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DayAndNightCircle : MonoBehaviour
{
    [SerializeField] private Light Sun;
    [SerializeField] private Light Moon;


    public Transform SunDayNightCircle;
    public Transform MoonDayNightCircle;
    public float Minute;
    public int Hours;
    [Header("Day & Night Circle Per min")]
    public int HoursMultiplier;

    private float Second;
    private float AngleManager;

    private void Awake()
    {
        if (Hours >= 6 && Hours <= 18)
            OnHoursChange(6);
        else
            OnHoursChange(18);
    }
    private void FixedUpdate()
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
        MoonDayNightCircle.transform.eulerAngles = Quaternion.Euler(180 + AngleManager, 0, 0).eulerAngles;
        SunDayNightCircle.transform.eulerAngles = Quaternion.Euler(AngleManager, AngleManager / 4, 0).eulerAngles;
        //Debug.Log($"{Hours}, {Minute}, {Second} ---- {SunDayNightCircle.transform.eulerAngles.x}");
    }

    private void OnHoursChange(int hours)
    {
        if (hours == 6)
        {
            Moon.shadows = LightShadows.None;
            Sun.shadows = LightShadows.Soft;
            
        }
        if (hours == 18)
        {
            Sun.shadows = LightShadows.None;
            Moon.shadows = LightShadows.Soft;
        }
    }
}
