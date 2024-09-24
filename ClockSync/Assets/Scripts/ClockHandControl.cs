using DG.Tweening;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClockHandControl : MonoBehaviour
{
    public RectTransform hourHand;
    public RectTransform minuteHand;
    public RectTransform secondHand;
    public TMP_InputField hourInputField;
    public TMP_InputField minuteInputField;
    public TMP_InputField secondInputField;
    [SerializeField] private TimeEditer clockEditer;
    [SerializeField] private Button confirmInputFEdit;

    private DateTime currentTime;

    private float hourDegreesPerSecond = 360f / 43200f; // 360 градусов / 12 часов (в секундах)
    private float minuteDegreesPerSecond = 360f / 3600f; // 360 градусов / 60 минут (в секундах)
    private float secondDegreesPerSecond = 360f / 60f;   // 360 градусов / 60 секунд

    public void SetInitialTime(DateTime time)
    {
        currentTime = time;
        SetClockHandsToTime();
        UpdateInputFields();
        InvokeRepeating(nameof(UpdateClockHands), 1f, 1f); 
    }

    // Обновляем положение стрелок на основании текущего времени
    private void SetClockHandsToTime()
    {
        float hoursAngle = (currentTime.Hour % 12) * 30f + currentTime.Minute * 0.5f; 
        float minutesAngle = currentTime.Minute * 6f; 
        float secondsAngle = currentTime.Second * 6f; 

        RotateHand(hourHand, hoursAngle);
        RotateHand(minuteHand, minutesAngle);
        RotateHand(secondHand, secondsAngle);
    }

    private void RotateHand(RectTransform hand, float angle)
    {
        hand.DORotate(new Vector3(0, 0, -angle), 0.1f); 
    }

    // Обновление стрелок каждую секунду
    private void UpdateClockHands()
    {
        currentTime = currentTime.AddSeconds(1); 

        float hoursAngle = (currentTime.Hour % 12) * 30f + currentTime.Minute * 0.5f;
        float minutesAngle = currentTime.Minute * 6f;
        float secondsAngle = currentTime.Second * 6f;

        RotateHand(hourHand, hoursAngle);
        RotateHand(minuteHand, minutesAngle);
        RotateHand(secondHand, secondsAngle);

        UpdateInputFields(); 
    }

    public void SetHour(int hour)
    {
        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hour, currentTime.Minute, currentTime.Second);
        SetClockHandsToTime();
    }

    public void SetMinute(int minute)
    {
        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, minute, currentTime.Second);
        SetClockHandsToTime();
    }

    public void SetSecond(int second)
    {
        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, second);
        SetClockHandsToTime();
    }

    // Обновляем значения в полях ввода в зависимости от текущего времени
    private void UpdateInputFields()
    {
        if (hourInputField.isFocused || minuteInputField.isFocused || secondInputField.isFocused)
        {
            confirmInputFEdit.gameObject.SetActive(true);
        }
        else
        {
            confirmInputFEdit.gameObject.SetActive(false);
        }

        if (!hourInputField.isFocused)
        {
            hourInputField.text = currentTime.Hour.ToString("D2");
        }
        if (!minuteInputField.isFocused)
        {
            minuteInputField.text = currentTime.Minute.ToString("D2");
        }
        if (!secondInputField.isFocused)
        {
            secondInputField.text = currentTime.Second.ToString("D2");
        }
    }
}
