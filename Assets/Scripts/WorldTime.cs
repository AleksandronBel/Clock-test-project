using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public enum ChooseAPI
{
    WorldTimeAPI,
    WorldClockAPI
}

public class WorldTime : MonoBehaviour
{
    [SerializeField] private Clock _clock;
    [SerializeField] private ChooseAPI _chooseAPI;

    private float _secondsInHour = 3600f;

    private DateTime _timeFromInternet;

    private static readonly Dictionary<ChooseAPI, string> APIsUrls = new Dictionary<ChooseAPI, string>()
    {
        { ChooseAPI.WorldTimeAPI, "https://worldtimeapi.org/api/timezone/Europe/Moscow" },
        { ChooseAPI.WorldClockAPI, "http://worldclockapi.com/api/json/utc/now" }
    };

    private void Start()
    {
        StartCoroutine(GetTimeFromAPI());
        InvokeRepeating(nameof(CheckAndUpdateTime), _secondsInHour, _secondsInHour);
    }

    private void CheckAndUpdateTime()
    {
        StartCoroutine(CheckAndCorrectTime());
    }

    private IEnumerator GetTimeFromAPI()
    {
        string url = APIsUrls[_chooseAPI];
        Debug.Log($"You have chosen {url}");

        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning($"Failed to get time from {url}: {webRequest.error}");
        }
        else
        {
            Debug.Log($"Time data from {url}: {webRequest.downloadHandler.text}");
            string timeData = webRequest.downloadHandler.text;
            _timeFromInternet = ParseTimeFromInternet(timeData);
            _clock.CurrentTime = _timeFromInternet;

            _clock.UpdateAnalogClock(_clock.CurrentTime, updateMinuteAndHour: true);
            _clock.UpdateTableClock(_clock.CurrentTime);
        }

        Debug.Log($"Time from Internet: {_timeFromInternet}");
    }

    private DateTime ParseTimeFromInternet(string dateTime)
    {
        string time = Regex.Match(dateTime, @"\d{2}:\d{2}:\d{2}").Value;

        if (!string.IsNullOrEmpty(time))
        {
            if (DateTime.TryParseExact(time, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
            {
                return parsedTime;
            }
            else
            {
                Debug.LogError("Failed to parse time from API response.");
                return DateTime.MinValue;
            }
        }
        else
        {
            Debug.LogError("Failed to parse time from API response.");
            return DateTime.MinValue;
        }
    }

    private IEnumerator CheckAndCorrectTime()
    {
        Debug.Log("Checking and updating time with server...");
        yield return StartCoroutine(GetTimeFromAPI());

        TimeSpan timeDifference = _clock.CurrentTime - _timeFromInternet;

        if (Math.Abs(timeDifference.TotalSeconds) > 1)
        {
            Debug.Log($"Time difference detected: {timeDifference.TotalSeconds} seconds. Correcting time.");
            _clock.CurrentTime = _timeFromInternet;
            _clock.UpdateAnalogClock(_clock.CurrentTime, updateMinuteAndHour: true);
        }
    }
}
