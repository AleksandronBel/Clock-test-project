using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetAlarm : MonoBehaviour
{
    [SerializeField] private Clock _clock;

    [SerializeField] private GameObject _buttonTurnOnAlarm;
    [SerializeField] private GameObject _alarmCommonWindow;
    [SerializeField] private GameObject _alarmSettingsWindow;
    [SerializeField] private GameObject _buttonSetAlarmTime;
    [SerializeField] private GameObject _alarms;
    [SerializeField] private GameObject _alarmCloseCommonWindow;

    [SerializeField] private TMP_InputField _alarmInput;
    [SerializeField] private TextMeshProUGUI _alarmTimeToUI;

    [SerializeField] private Button _confirmButton;

    [SerializeField] private string _timeFormat = "HH:mm";

    private DateTime _alarmTime;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        _confirmButton.interactable = false;
        _alarmCommonWindow.SetActive(false);
        _alarmSettingsWindow.SetActive(false);
        _alarms.SetActive(false);
    }

    public void CheckCorrectly()
    {
        string inputText = _alarmInput.text;

        if (DateTime.TryParseExact(inputText, _timeFormat, null, System.Globalization.DateTimeStyles.None, out DateTime inputTime))
        {
            Debug.Log("The time is entered correctly and corresponds to the format");
            _confirmButton.interactable = true;
            _alarmTime = inputTime;
        }
        else
        {
            _confirmButton.interactable = false;
            Debug.Log("The time is entered correctly and does not match the format.");
        }
    }

    public void TurnOnAlarm()
    {
        _buttonTurnOnAlarm.SetActive(false);
        _alarmCommonWindow.SetActive(true);
    }

    public void TurnOnSettingsWindow()
    {
        _buttonSetAlarmTime.SetActive(false);
        _alarmSettingsWindow.SetActive(true);
        _alarmCloseCommonWindow.SetActive(false);
        _alarms.SetActive(false);
    }

    public void ConfirmAlarmsButton()
    {
        _buttonSetAlarmTime.SetActive(true);
        _alarmSettingsWindow.SetActive(false);
        _alarms.SetActive(true);
        _alarmCloseCommonWindow.SetActive(true);

        _clock.AlarmTime = _alarmTime;
        _alarmTimeToUI.text = _alarmInput.text;
        Debug.Log($"The alarm is set to: {_clock.AlarmTime}");
    }

    public void CloseAlarmsButton()
    {
        if (_clock.AlarmTime != null)
            _alarms.SetActive(true);
        else
            _alarms.SetActive(false);

        _buttonSetAlarmTime.SetActive(true);
        _alarmSettingsWindow.SetActive(false);
        _alarmCloseCommonWindow.SetActive(true);
    }

    public void CloseAlarmCommonWindow()
    {
        _alarmCommonWindow.SetActive(false);
        _buttonTurnOnAlarm.SetActive(true);
    }
}