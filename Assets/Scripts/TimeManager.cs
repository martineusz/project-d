using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int day = 1;
    private float timer = 0f;
    private const float dayDuration = 10f;
    public DayType currentDayType = DayType.Normal;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= dayDuration)
        {
            day++;
            timer = 0f;

            currentDayType = day % 7 == 0 ? DayType.BloodMoon : DayType.Normal;
        }
    }

    public enum DayType
    {
        Normal,
        BloodMoon,
    }
}