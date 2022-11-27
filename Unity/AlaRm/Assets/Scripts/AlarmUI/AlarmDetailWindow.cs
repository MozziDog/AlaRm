using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmDetailWindow : MonoBehaviour
{
    public static AlarmDetailWindow instance { get; private set; }
    bool isNewAlarm = false;
    UIWindowManager windowManager;

    [SerializeField]
    TMP_Text text_ampm;
    [SerializeField]
    TMP_InputField text_hour;
    [SerializeField]
    TMP_InputField text_min;
    [SerializeField]
    TMP_Dropdown dropdown_dialogueType;
    [SerializeField]
    TMP_Text text_repeatDaysInfo;
    [SerializeField]
    Toggle[] toggle_repeatDays;
    [SerializeField]
    TMP_InputField text_alarmTitle;
    // TODO: 스누즈 정보 관리 기능 추가

    Color color_normalText;
    Color color_inputText;
    Color color_inputBackground;
    Color color_highlightText;
    Color color_highlightBackground;

    AlarmData currentAlarmData;

    readonly string[] daysinWeekString = { "월", "화", "수", "목", "금", "토", "일"};

    private void Start()
    {
        instance = this;
        if(windowManager == null)
            windowManager = GetComponentInParent<UIWindowManager>();
    }

    public void OpenAlarmDetailWindow(AlarmData alarm, bool newAlarm = false)
    {
        currentAlarmData = alarm;
        isNewAlarm = newAlarm;
        GetUIColorData();
        InitializeWindow();
        windowManager.OpenWindow(gameObject);
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
            toggle_repeatDays[i].isOn = currentAlarmData.repeatDayInWeek[i];
        }
        text_alarmTitle.text = currentAlarmData.alarmTitle;
        // TODO: 여기에 스누즈 정보 표시 기능 추가
        UpdateRepeatDayText();
        UpdateToggleColor();
        // TODO: 여기에 컬러 테마 변경 기능 삽입
    }

    void UpdateTimeDisplay()
    {
        text_ampm.text = currentAlarmData.hour < 12 ? "AM" : "PM";
        text_hour.text = string.Format("{0:D2}", currentAlarmData.hour < 12 ? currentAlarmData.hour : currentAlarmData.hour - 12);
        if (text_hour.text == "00")
            text_hour.text = "12";
        text_min.text = string.Format("{0:D2}", currentAlarmData.minute);
        // Debug.Log(string.Format("바뀐 시간: {0}", currentAlarmData.time));
    }

    private void UpdateRepeatDayText()
    {
        // Debug.Log("Update Repeat Day text");
        bool everyDay = true;
        string text = "매주 ";
        for (int i = 0; i < 7; i++)
        {
            if (toggle_repeatDays[i].isOn)
                text += daysinWeekString[i] + " ";
            else
                everyDay = false;
        }
        if (everyDay)
            text = "매일 반복";
        else
            text += "반복";
        text_repeatDaysInfo.text = text;
    }

    void UpdateToggleColor()
    {
        foreach(var toggle in toggle_repeatDays)
        {
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
        if (currentAlarmData.hour > 12)
            currentAlarmData.hour -= 12;
        else
            currentAlarmData.hour += 12;
        UpdateTimeDisplay();
    }

    public void OnHourTextChanged(TMP_InputField input)
    {
        int newHour;
        try
        {
            newHour = int.Parse(input.text);
            if (newHour < 0 || newHour > 12)
                throw new FormatException();
            if (newHour == 12)
                newHour = 0;
        }
        catch(ArgumentException)
        {
            UpdateTimeDisplay();
            return;
        }
        catch (FormatException)
        {
            UpdateTimeDisplay();
            return;
        }
        catch (OverflowException)
        {
            UpdateTimeDisplay();
            return;
        }
        if (text_ampm.text == "PM") //(currentAlarmData.hour >= 12)
            newHour += 12;
        Debug.Log("new Hour value: " + newHour);
        currentAlarmData.hour = newHour;
        UpdateTimeDisplay();
    }

    public void OnClickButtonHourUp()
    {
        if (currentAlarmData.hour < 23)
            currentAlarmData.hour++;
        else
            currentAlarmData.hour = 0;
        UpdateTimeDisplay();
    }

    public void OnClickButtonHourDown()
    {
        if (currentAlarmData.hour > 0)
            currentAlarmData.hour--;
        else
            currentAlarmData.hour = 23;
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
            newMinute = currentAlarmData.hour;
        }
        catch (FormatException)
        {
            newMinute = currentAlarmData.hour;
        }
        catch (OverflowException)
        {
            newMinute = currentAlarmData.hour;
        }
        currentAlarmData.minute = newMinute;
        UpdateTimeDisplay();
    }

    public void OnClickButtonMinuteUp()
    {
        if (currentAlarmData.minute < 59)
            currentAlarmData.minute++;
        else
        {
            if (currentAlarmData.hour < 23)
            {
                currentAlarmData.hour++;
                currentAlarmData.minute = 0;
            }
            else
            {
                currentAlarmData.hour = 0;
                currentAlarmData.minute = 0;
            }
        }
            
        UpdateTimeDisplay();
    }

    public void OnClickButtonMinuteDown()
    {
        if (currentAlarmData.minute > 0)
            currentAlarmData.minute--;
        else
        {
            if (currentAlarmData.hour > 0)
            {
                currentAlarmData.hour--;
                currentAlarmData.minute = 59;
            }
            else
            {
                currentAlarmData.hour = 23;
                currentAlarmData.minute = 59;
            }
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
            if (toggle_repeatDays[i] == input)
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
        UpdateRepeatDayText();
    }


    public void OnTitleTextChanged(TMP_InputField input)
    {
        currentAlarmData.alarmTitle = input.text;
        Debug.Log("바뀐 알람 제목: "+currentAlarmData.alarmTitle);
    }

    public void OnClickExitButton()
    {
        // TODO: 여기에 창 닫기 연출 만들기
        windowManager.CloseWindow(gameObject);
    }

    public void OnClickDeleteButton()
    {
        int index = SaveManager.instance.saveData.alarms.FindIndex((alarm) => { return alarm.alarmID == currentAlarmData.alarmID; });
        SaveManager.instance.saveData.alarms.RemoveAt(index);

        AndroidPluginLoader.Instance.CancelAlarm(currentAlarmData.alarmID);

        AlarmListWindow.Instance.UpdateAlarmWindow();
        // TODO: 여기에 창 닫기 연출 만들기
        windowManager.CloseWindow(gameObject);
    }

    public void OnClickSubmitButton()
    {
        // 세이브데이터에 추가하면서 기존에 동일한 ID의 알람이 있는지 확인
        bool isNew = AddOrUpdateAlarm(currentAlarmData);
        SaveManager.instance.Save();
        // 기존에 동일한 ID의 알람이 있었다면 취소 후 다시 등록
        if (!isNew)
            AndroidPluginLoader.Instance.CancelAlarm(currentAlarmData.alarmID);
        AndroidPluginLoader.Instance.SetAlarm(currentAlarmData.alarmID, currentAlarmData.hour, currentAlarmData.minute);

        // TODO: 여기에 창 닫기 연출 만들기
        windowManager.CloseWindow(gameObject);

        AndroidPluginLoader.Instance.ShowToast(String.Format("{0}시 {1}분에 알람이 울립니다", currentAlarmData.hour, currentAlarmData.minute));

        AlarmListWindow.Instance.UpdateAlarmWindow();
    }

    /// <summary>
    /// return true if 'Add', return false if 'Update'
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool AddOrUpdateAlarm(AlarmData input)
    {
        ref List<AlarmData> alarms = ref SaveManager.instance.saveData.alarms;
        for (int i = 0; i < alarms.Count; i++)
        {
            if (alarms[i].alarmID == input.alarmID) // update
            {
                alarms[i] = input;
                Debug.Log("AddOrUpdateAlarm: 기존 알람 업데이트");
                // 여기서 Save가 필요하려나?
                return true;
            }
        }
        alarms.Add(input);
        Debug.Log("AddOrUpdateAlarm: 신규 알람 등록");
        // 여기서 Save가 필요하려나? 22
        Debug.Log("현재 saveData.alarms.Count:" + alarms.Count.ToString());
        return false;
    }
}
