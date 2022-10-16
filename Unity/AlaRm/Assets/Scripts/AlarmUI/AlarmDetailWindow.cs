using QuantumTek.QuantumUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class AlarmDetailWindow : MonoBehaviour
{
    [SerializeField]
    TMP_Text text_ampm;
    [SerializeField]
    TMP_InputField text_hour;
    [SerializeField]
    TMP_InputField text_min;
    [SerializeField]
    TMP_Dropdown dropdown_dialogueType;
    [SerializeField]
    GameObject[] go_repeatDays;
    [SerializeField]
    TMP_InputField text_alarmTitle;
    // TODO: 스누즈 정보 관리 기능 추가

    Color color_normalText;
    Color color_inputText;
    Color color_inputBackground;
    Color color_highlightText;
    Color color_highlightBackground;

    AlarmData currentAlarmData;

    private void Start()
    {
        AlarmData testData = new();
        testData.repeatDayInWeek = new bool[] { true, true, false, false, false, true, false};
        OpenAlarmDetailWindow(testData);
    }

    public void OpenAlarmDetailWindow(AlarmData alarm)
    {
        currentAlarmData = alarm;
        GetUIColorData();
        InitializeWindow();
        gameObject.SetActive(true);
        // TODO: 여기에 알람상세 창 열기 연출 효과 만들기
    }

    void GetUIColorData()
    {
        color_normalText = Color.white;
        color_inputText = Color.black;
        color_inputBackground = Color.white;
        color_highlightText = Color.white;
        color_highlightBackground = new Color(.6f, .6f, 1, 1);
    }

    void InitializeWindow()
    {
        // value 설정
        UpdateTimeDisplay();
        dropdown_dialogueType.value = 0;
        for (var i = 0; i < 7; i++)
        {
            go_repeatDays[i].GetComponent<Toggle>().isOn = currentAlarmData.repeatDayInWeek[i];
        }
        text_alarmTitle.text = currentAlarmData.alarmTitle;
        // TODO: 여기에 스누즈 정보 표시 기능 추가
        UpdateToggleColor();
        // TODO: 여기에 컬러 테마 변경 기능 삽입
    }

    void UpdateTimeDisplay()
    {
        text_ampm.text = currentAlarmData.time.Hour < 12 ? "AM" : "PM";
        text_hour.text = string.Format("{0:D2}", currentAlarmData.time.Hour < 12 ? currentAlarmData.time.Hour : currentAlarmData.time.Hour - 12);
        if (text_hour.text == "00")
            text_hour.text = "12";
        text_min.text = string.Format("{0:D2}", currentAlarmData.time.Minute);
        Debug.Log(string.Format("바뀐 시간: {0}", currentAlarmData.time));
    }

    void UpdateToggleColor()
    {
        foreach(var toggleObejct in go_repeatDays)
        {
            var toggle = toggleObejct.GetComponent<Toggle>();
            ColorBlock colorOptions = toggle.colors;
            if (toggle.isOn)
            {
                colorOptions.normalColor = color_highlightBackground;
                colorOptions.selectedColor = color_highlightBackground;
            }
            else
            {
                colorOptions.normalColor = color_inputBackground;
                colorOptions.selectedColor = color_inputBackground;
            }
            toggle.colors = colorOptions;
        }
    }

    public void OnClickButtonAMPM()
    {
        /*
        if(currentAlarmData.time.Hour < 12) // 오전일 경우 오후로 바꾸기
            currentAlarmData.time.AddHours(12);
        if (currentAlarmData.time.Hour < 12) // 오후일 경우 오전으로 바꾸기
            currentAlarmData.time.AddHours(-12);
        */
        currentAlarmData.time.AddHours(12);
        UpdateTimeDisplay();
    }

    public void OnHourTextChanged(TMP_InputField input)
    {
        int newHour;
        try
        {
            newHour = int.Parse(input.text);
            if (newHour < 0 || newHour > 23)
                throw new FormatException();
        }
        catch(ArgumentException)
        {
            newHour = currentAlarmData.time.Hour;
        }
        catch (FormatException)
        {
            newHour = currentAlarmData.time.Hour;
        }
        catch (OverflowException)
        {
            newHour = currentAlarmData.time.Hour;
        }
        DateTime now = DateTime.Now;
        currentAlarmData.time = new DateTime(
            now.Year,
            now.Month,
            now.Day,
            newHour,
            currentAlarmData.time.Minute,
            now.Second);
        UpdateTimeDisplay();
    }

    public void OnClickButtonHourUp()
    {
        if (currentAlarmData.time.Hour < 23)
            currentAlarmData.time = currentAlarmData.time.AddHours(1);
        else
            currentAlarmData.time = currentAlarmData.time.AddHours(-23);
        UpdateTimeDisplay();
    }

    public void OnClickButtonHourDown()
    {
        if (currentAlarmData.time.Hour > 0)
            currentAlarmData.time = currentAlarmData.time.AddHours(-1);
        else
            currentAlarmData.time = currentAlarmData.time.AddHours(23);
        UpdateTimeDisplay();
    }

    public void OnMinuteTextChanged(TMP_InputField input)
    {
        int newMinute;
        try
        {
            newMinute = int.Parse(input.text);
            if (newMinute < 0 || newMinute > 59)
                throw new FormatException();
        }
        catch (ArgumentException)
        {
            newMinute = currentAlarmData.time.Hour;
        }
        catch (FormatException)
        {
            newMinute = currentAlarmData.time.Hour;
        }
        catch (OverflowException)
        {
            newMinute = currentAlarmData.time.Hour;
        }
        DateTime now = DateTime.Now;
        currentAlarmData.time = new DateTime(
            now.Year,
            now.Month,
            now.Day,
            currentAlarmData.time.Hour,
            newMinute,
            now.Second);
        UpdateTimeDisplay();
    }

    public void OnClickButtonMinuteUp()
    {
        if (currentAlarmData.time.Minute < 59)
            currentAlarmData.time = currentAlarmData.time.AddMinutes(1);
        else
        {
            if (currentAlarmData.time.Hour < 23)
                currentAlarmData.time = currentAlarmData.time.AddHours(1).AddMinutes(-59);
            else
                currentAlarmData.time = currentAlarmData.time.AddHours(-23).AddMinutes(-59);
        }
            
        UpdateTimeDisplay();
    }

    public void OnClickButtonMinuteDown()
    {
        if (currentAlarmData.time.Minute > 0)
            currentAlarmData.time = currentAlarmData.time.AddMinutes(-1);
        else
        {
            if (currentAlarmData.time.Hour > 0)
                currentAlarmData.time = currentAlarmData.time.AddHours(-1).AddMinutes(59);
            else
                currentAlarmData.time = currentAlarmData.time.AddHours(23).AddMinutes(59);
        }
        UpdateTimeDisplay();
    }

    public void OnDialogueTypeChange(TMP_Dropdown input)
    {
        currentAlarmData.dialogueType = (AlarmDialogueType)input.value;
        Debug.Log("바뀐 알람 타입 : " + currentAlarmData.dialogueType.ToString());
    }

    public void OnRepeatDaysToggled(Toggle input)
    {
        int toggleIndex = -1;
        for(int i=0; i<7; i++)
        {
            if (go_repeatDays[i] == input.gameObject)
            {
                toggleIndex = i;
                break;
            }
        }
        if(toggleIndex < 0 && toggleIndex >= 7)
        {
            Debug.Assert(false, "잘못된 토글 인덱스!");
            return;
        }
        if(currentAlarmData.repeatDayInWeek != null)
            currentAlarmData.repeatDayInWeek[toggleIndex] = input.isOn;
        else
        {
            Debug.Assert(false, "현재 알람 정보가 존재하지 않습니다!");
        }
        UpdateToggleColor();
    }


    public void OnTitleTextChanged(TMP_InputField input)
    {
        currentAlarmData.alarmTitle = input.text;
        Debug.Log("바뀐 알람 제목: "+currentAlarmData.alarmTitle);
    }

    public void OnClickExitButton()
    {
        // TODO: 여기에 창 닫기 연출 만들기
        gameObject.GetComponent<QUI_Window>().SetActive(false);
    }

    public void OnClickSubmitButton()
    {
        // TODO: 여기에 자바랑 통신하는 코드 넣기
        // TODO: 여기에 창 닫기 연출 만들기
        gameObject.GetComponent<QUI_Window>().SetActive(false);
        // TODO: 여기에 X시 XX분 후에 알람이 울립니다 메시지 띄우는 코드 넣기
    }
}
