using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public int day = 1;
    private float _timer = 0f;
    public float dayDuration = 10f;
    public DayType currentDayType = DayType.Normal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= dayDuration)
        {
            day++;
            _timer = 0f;
            currentDayType = day % 7 == 0 ? DayType.BloodMoon : DayType.Normal;
        }
    }
    public enum DayType
    {
        Normal,
        BloodMoon,
    }
}

