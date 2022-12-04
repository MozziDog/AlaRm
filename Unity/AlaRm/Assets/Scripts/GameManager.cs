using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.HID.HID;

public enum Situation
{
    AlarmSituation,
    NormalSituation,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] List<GameObject> NoDestroyList = new List<GameObject>();

    [SerializeField] // TODO: 개발 완료되면 readonly로 수정할 것. 
    public Situation appMode = Situation.AlarmSituation;

    [SerializeField] GameObject characterToSpawn; // 개발용. 일단 임시로 스폰할 캐릭터를 인스펙터에서 명시
    GameObject character; // 현재 스폰되어있는 캐릭터.


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        foreach(var obj in NoDestroyList)
            DontDestroyOnLoad(obj);

        // 세이브데이터 로드
        SaveManager.instance.Load();

        int? nowAlarmID;
        appMode = GetAppSituation(out nowAlarmID);
        if(appMode == Situation.AlarmSituation)
        {
            Debug.Assert(nowAlarmID != null, "Cannot get now Alarm ID!");
            CheckAndSetAlarmRepeat((int)nowAlarmID);
        }

        StartCoroutine(startMainScene());
    }

    Situation GetAppSituation(out int? alarmID)
    {
        // AlarmManager로부터 호출된 거 검사 안하고
        // 저장된 알람 중에 현재 시각으로 맞춰놓은 것이 있는지만 검사
        foreach(var alarm in SaveManager.instance.saveData.alarms)
        {
            // 비활성화된 알람은 넘김.
            if (alarm.active == false)
                continue;

            // 일단 활성화된 알람일 경우
            DateTime now = DateTime.Now;
            if(now.Hour == alarm.hour && now.Minute == alarm.hour)
            {
                // 오늘 요일이 true인 경우
                if (alarm.repeatDayInWeek[((int)now.DayOfWeek)] == true)
                {
                    alarmID = alarm.alarmID;
                    return Situation.AlarmSituation;
                }
                else
                {
                    bool alarmFlag = true;
                    // 오늘 요일은 false인데 다른 요일은 true라면?
                    // → alarm은 지금 울려야 하는 알람이 아님.
                    for(int i=0; i< alarm.repeatDayInWeek.Length; i++)
                    {
                        if (i != ((int)now.DayOfWeek) && alarm.repeatDayInWeek[i] == true)
                        {
                            alarmFlag = false;
                            break;
                        }
                    }
                    // 오늘 요일도 false이고 다른 요일도 false라면 1회성 알람임.
                    if (alarmFlag == true)
                    {
                        alarmID = alarm.alarmID;
                        return Situation.AlarmSituation;
                    }
                    else
                        continue;
                }
            }
        }
        alarmID = null;
        return Situation.NormalSituation;
    }

    void CheckAndSetAlarmRepeat(int alarmID)
    {
        AlarmData? alarmToRepeat = SaveManager.instance.saveData.alarms.Find((alarm) => { return alarm.alarmID == alarmID; });
        Debug.Assert(alarmToRepeat == null, "Cannot Find Alarm to Repeat!");
        AlarmData alarm = alarmToRepeat.Value;

        if (alarm.repeatDayInWeek[((int)DateTime.Now.DayOfWeek)] == true)
        {
            AndroidPluginLoader.Instance.SetAlarmWithDayOfWeek(alarm.alarmID, ((int)DateTime.Now.DayOfWeek), alarm.hour, alarm.minute);
            Debug.Log("Alarm Repeat set");
        }
        else
            Debug.Log("Alarm Repeat not set because it doen't needed");
    }

    IEnumerator startMainScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(1.5f);  // 앱 부팅 연출 시간 최소 1.5초
        while(operation.progress < 0.9f)
        {
            Debug.Log(string.Format("loading Main scene : {0}%", operation.progress));
            yield return 0;
        }
        Debug.Log("Load Main scene almost Complete!!");
        operation.allowSceneActivation = true;
    }

    public void OnFinishedAlarmSequence()
    {
        appMode = Situation.NormalSituation;

        UIWindowManager windowManager = GameObject.FindGameObjectWithTag("UICanvas")?.GetComponent<UIWindowManager>();
        if (windowManager == null)
        {
            Debug.LogError("Cannot find UIWindowManager!!!");
            return;
        }
        windowManager.SetUIMode(appMode);
    }
}
