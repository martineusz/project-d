using System;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalLightManager : MonoBehaviour
{
    public Volume bloodMoonVolume;

    private void Awake()
    {
        if (bloodMoonVolume != null)
            bloodMoonVolume.gameObject.SetActive(false);
    }

    private void Update()
    {
        var timeManager = TimeManager.Instance;
        if (!timeManager) return;

        if (timeManager.currentDayType == TimeManager.DayType.BloodMoon)
        {
            bloodMoonVolume.gameObject.SetActive(true);
        }
        else
        {
            bloodMoonVolume.gameObject.SetActive(false);
        }
    }
}