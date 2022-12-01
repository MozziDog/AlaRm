using QuantumTek.QuantumUI;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public struct AlarmData
{
    public bool active;
    public int alarmID;
    public string alarmTitle;
    public int hour;
    public int minute;
    public bool[] repeatDayInWeek; // 0~6, 순서대로 월화수목금토일
    public AlarmDialogueType dialogueType;
}

public enum AlarmDialogueType
{
    School,                 // 등교
    Company,                // 출근
    ImportantEvent,         // 중요한 일
    Appointment,            // 약속
    WeekendOrHolidays,      // 휴일
}

[CreateAssetMenu(fileName = "Alarm Define", menuName = "AlaRm Project/UI/Alarm Type Icon Preset")]
public class AlarmDefine : ScriptableObject
{
    // TODO: 시간 되면 여기를 수정해서 라이트모드/다크모드 테마 전환기능 만들기.
    public Color fontColor_active;              // = Color.white;
    public Color fontColor_unactive;            // = new Color(.6f, .6f, .6f, 1);
    public Color fontColor_selected;            // = new Color(.6f, .6f, 1, 1);
    public Color fontColor_selected_unactive;   // = new Color(.45f, .45f, .6f, 1);
    public List<Sprite> alarmTypeImages = new List<Sprite> ();
}