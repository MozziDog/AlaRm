using JetBrains.Annotations;
using QuantumTek.QuantumUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmListWindow : MonoBehaviour
{
    public static AlarmListWindow Instance;
    UIWindowManager windowManager;

    [SerializeField]
    GameObject listElementPrefab;
    [SerializeField][Tooltip("list Element들을 묶어줄 부모 오브젝트의 트랜스폼")]
    Transform listHolder;

    private void Start()
    {
        Instance = this;
        if(windowManager == null)
            windowManager = GetComponentInParent<UIWindowManager>();
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
        return SaveManager.instance.saveData.alarms.ToArray();
    }

    void UpdateAlarmList(AlarmData[] alarmDatas)
    {
        for(int i = listHolder.childCount-1; i>=0; i--)
        {
            Destroy(listHolder.GetChild(i).gameObject);
        }
        if (alarmDatas != null)
        {
            foreach (AlarmData alarm in alarmDatas)
            {
                GameObject el = GameObject.Instantiate(listElementPrefab, listHolder);
                el.GetComponent<AlarmListElement>().SetAlarmListElementData(alarm);
            }
        }
        Debug.Log("updated AlarmList");
    }

    public void CreateNewAlarm()
    {
        AlarmData alarmData = new AlarmData();
        alarmData.active = true;
        alarmData.alarmID = FindAvailableAlarmID();
        alarmData.alarmTitle = "";
        alarmData.hour = DateTime.Now.Hour;
        alarmData.minute = DateTime.Now.Minute;
        alarmData.repeatDayInWeek = new bool[7];
        alarmData.dialogueType = AlarmDialogueType.School;

        AlarmDetailWindow.instance.OpenAlarmDetailWindow(alarmData, true);
    }

    private int FindAvailableAlarmID()
    {
        Debug.Log("FindAvailableAlarmID, SaveData exist:" + SaveManager.instance.saveData != null);
        if (SaveManager.instance.saveData.alarms.Count == 0)
            return 1000;

        // 1,2,3을 쓰고 2를 지웠을 때, 2를 다시 사용해도 될까? 정렬 등에서 문제는 없을까? 없을 것 같긴 한데
        int maxIDusing = 0;
        foreach (var alarm in SaveManager.instance.saveData.alarms)
        {
            if(alarm.alarmID > maxIDusing)
                maxIDusing = alarm.alarmID;
        }
        return maxIDusing + 1;
        // 아무리 알람을 많이 설정해도 MAX_INT 만큼 설정할 일은 없겠지?
    }
}
