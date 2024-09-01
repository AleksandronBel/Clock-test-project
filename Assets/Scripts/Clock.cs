using System;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform _hourArrow;
    [SerializeField] private Transform _minuteArrow;
    [SerializeField] private Transform _secondArrow;
    [SerializeField] private TextMeshProUGUI _digitalClock;

    private DateTime _previousTime;

    public DateTime CurrentTime { get; set; }

    public DateTime AlarmTime { get; set; }

    private void OnEnable()
    {
        CurrentTime = DateTime.Now;
        _previousTime = DateTime.Now;
        UpdateAnalogClock(CurrentTime, updateMinuteAndHour: true);
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        TimeSpan elapsedTime = DateTime.Now - _previousTime;

        DateTime currentTime = CurrentTime.Add(elapsedTime);

        if (currentTime.Second != _previousTime.Second)
        {
            bool updateMinuteAndHour = CurrentTime.Second == 0;

            UpdateAnalogClock(currentTime, updateMinuteAndHour);
            UpdateTableClock(currentTime);

            CurrentTime = currentTime;

            _previousTime = DateTime.Now;

            if (AlarmTime.Hour == currentTime.Hour && AlarmTime.Minute == currentTime.Minute)
            {
                PlayAlarm();
            }
        }
    }

    public void UpdateTableClock(DateTime currentTime)
    {
        _digitalClock.text = currentTime.ToString("HH:mm:ss");
    }

    public void UpdateAnalogClock(DateTime currentTime, bool updateMinuteAndHour)
    {
        float secondsAngle = currentTime.Second * 6;
        _secondArrow.localRotation = Quaternion.Euler(0, 0, -secondsAngle);

        if (updateMinuteAndHour)
        {
            float minutesAngle = currentTime.Minute * 6;
            float hoursAngle = currentTime.Hour % 12 * 30 + currentTime.Minute * 0.5f;

            _minuteArrow.localRotation = Quaternion.Euler(0, 0, -minutesAngle);
            _hourArrow.localRotation = Quaternion.Euler(0, 0, -hoursAngle);
        }
    }

    private void PlayAlarm()
    {
        Debug.Log("The alarm clock rang!");
    }
}
