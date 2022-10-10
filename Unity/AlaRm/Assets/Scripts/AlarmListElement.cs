using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct RepeatDayInWeek
{
    public bool mon;
    public bool tue;
    public bool wed;
    public bool thu;
    public bool fri;
    public bool sat;
    public bool sun;
}
public struct alarmData
{
    public int alarmID;
    public string alarmTitle;
    public DateTime time;  // date 부분은 무시하고 time만 사용함.
    public RepeatDayInWeek repeatDayInWeek;
}

public class AlarmListElement : MonoBehaviour
{
    private int alarmID;
    [SerializeField]
    TextMeshProUGUI text_Title;
    [SerializeField]
    TextMeshProUGUI text_ampm;
    [SerializeField]
    TextMeshProUGUI text_time;
    [SerializeField]
    TextMeshProUGUI[] repeatDayInWeek;


    // Start is called before the first frame update
    void Start()
    {
        // 테스트
        var test = new alarmData();
        test.alarmID = 1233;
        test.alarmTitle = "hi";
        test.time = DateTime.Now;
        test.repeatDayInWeek.mon = true;
        test.repeatDayInWeek.tue = true;
        test.repeatDayInWeek.wed = true;
        SetAlarmListElementData(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetAlarmListElementData(alarmData inputData)
    {
        alarmID = inputData.alarmID;
        text_Title.text = inputData.alarmTitle;
        text_ampm.text = inputData.time.Hour < 12? "AM" : "PM";
        text_time.text = string.Format("{0:D2}:{1:D2}",inputData.time.Hour < 12 ? inputData.time.Hour : inputData.time.Hour-12, inputData.time.Minute);
        repeatDayInWeek[0].color = inputData.repeatDayInWeek.mon == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
        repeatDayInWeek[1].color = inputData.repeatDayInWeek.tue == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
        repeatDayInWeek[2].color = inputData.repeatDayInWeek.wed == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
        repeatDayInWeek[3].color = inputData.repeatDayInWeek.thu == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
        repeatDayInWeek[4].color = inputData.repeatDayInWeek.fri == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
        repeatDayInWeek[5].color = inputData.repeatDayInWeek.sat == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
        repeatDayInWeek[6].color = inputData.repeatDayInWeek.sun == true ? new Color(1, 1, 1, 1) : new Color(.6f, .6f, .6f, 1);
    }

    public void OnClickAlarmListElement()
    {
        Debug.Log("click");
    }
}
