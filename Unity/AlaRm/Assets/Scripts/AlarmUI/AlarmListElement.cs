using QuantumTek.QuantumUI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmListElement : MonoBehaviour
{
    public AlarmDefine alarmConfig;

    [SerializeField]
    TextMeshProUGUI text_Title;
    [SerializeField]
    TextMeshProUGUI text_ampm;
    [SerializeField]
    TextMeshProUGUI text_time;
    [SerializeField]
    TextMeshProUGUI[] repeatDayInWeek;
    [SerializeField]
    Image alarmTypeIcon;
    [SerializeField]
    Toggle toggle;

    AlarmData alarmData;

    Color fontColor_active;              // = Color.white;
    Color fontColor_unactive;            // = new Color(.6f, .6f, .6f, 1);
    Color fontColor_selected;            // = new Color(.6f, .6f, 1, 1);
    Color fontColor_selected_unactive;   // = new Color(.45f, .45f, .6f, 1);

    public void SetAlarmListElementData(AlarmData inputData)
    {
        this.alarmData = inputData;
        GetUIColorData();
        UpdateAlarmListElementDisplay();
    }

    private void GetUIColorData()
    {
        // TODO: 여기를 수정해서 컬러 테마 바꾸는 기능 만들기
        fontColor_active              = Color.white;
        fontColor_unactive            = new Color(.6f, .6f, .6f, 1);
        fontColor_selected            = new Color(.6f, .6f, 1, 1);
        fontColor_selected_unactive   = new Color(.45f, .45f, .6f, 1);
    }

    private void UpdateAlarmListElementDisplay()
    {
        text_Title.text = alarmData.alarmTitle;
        text_ampm.text = alarmData.time.Hour < 12 ? "AM" : "PM";
        int hour = alarmData.time.Hour < 12 ? alarmData.time.Hour : alarmData.time.Hour - 12;
        if (hour == 0)
            hour = 12;
        text_time.text = string.Format("{0:D2}:{1:D2}", hour, alarmData.time.Minute);
        if(alarmConfig.alarmTypeImages.Count > (int)alarmData.dialogueType)
            alarmTypeIcon.sprite = alarmConfig.alarmTypeImages[(int)alarmData.dialogueType];

        // 폰트 컬러 설정
        if (this.alarmData.active)
        {
            text_Title.color = fontColor_active;
            text_ampm.color = fontColor_active;
            text_time.color = fontColor_active;
            for(int i=0; i<7; i++)
            {
                repeatDayInWeek[i].color = alarmData.repeatDayInWeek[i] == true ? fontColor_selected : fontColor_active;
            }
        }
        else
        {
            text_Title.color = fontColor_unactive;
            text_ampm.color = fontColor_unactive;
            text_time.color = fontColor_unactive;
            for (int i = 0; i < 7; i++)
            {
                repeatDayInWeek[i].color = alarmData.repeatDayInWeek[i] == true ? fontColor_selected_unactive : fontColor_unactive;
            }
        }

        toggle.isOn = this.alarmData.active;
    }

    public void OnClickAlarmListElement()
    {
        var alarmDetailWindow = GameObject.FindGameObjectWithTag("AlarmDetailWindow")?.GetComponent<AlarmDetailWindow>();
        alarmDetailWindow.OpenAlarmDetailWindow(alarmData);
    }

    public void OnToggleValueChanged(Toggle change)
    {
        this.alarmData.active = change.isOn;
        // TODO: 여기에 자바랑 통신하도록 하는 코드 넣기
        UpdateAlarmListElementDisplay();
    }
}
