using QuantumTek.QuantumUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmListWindow : MonoBehaviour
{
    UIWindowManager windowManager;

    [SerializeField]
    GameObject listElementPrefab;
    [SerializeField][Tooltip("list Element들을 묶어줄 부모 오브젝트의 트랜스폼")]
    Transform listHolder;

    private void Start()
    {
        if(windowManager == null)
            windowManager = GetComponentInParent<UIWindowManager>();

        // TODO: 테스트 제거
        var test = new AlarmData();
        test.active = true;
        test.alarmID = 1233;
        test.alarmTitle = "hi";
        test.time = DateTime.Now;
        test.repeatDayInWeek = new bool[] {true,true,true,false,false,true,true};

        var test2 = test;
        test2.active = false;

        AlarmData[] testdata = { test, test2 };

        UpdateAlarmList(testdata);
    }

    public void OpenAlarmListWindow()
    {
        UpdateAlarmWindow();
        windowManager.OpenWindow(gameObject);
    }

    public void CloseAlarmListWindow()
    {
        windowManager.CloseWindow(gameObject);
    }

    public void UpdateAlarmWindow()
    {
        AlarmData[] alarms = GetAlarmData();
        UpdateAlarmList(alarms);
    }

    AlarmData[] GetAlarmData()
    {
        // TODO: 여기에 자바와 통신하는 코드 넣기
        Debug.Log("자바와 통신하는 부분");
        return null;
    }

    void UpdateAlarmList(AlarmData[] alarmDatas)
    {
        if (alarmDatas != null)
        {
            foreach (AlarmData alarm in alarmDatas)
            {
                GameObject el = GameObject.Instantiate(listElementPrefab, listHolder);
                el.GetComponent<AlarmListElement>().SetAlarmListElementData(alarm);
            }
        }
    }
}
