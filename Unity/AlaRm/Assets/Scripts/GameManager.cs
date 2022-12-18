

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEditor;
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
    public bool isDebug;    // TODO: 개발 완료되면 삭제

    public static GameManager instance;

    [SerializeField] // TODO: 개발 완료되면 readonly로 수정할 것. 
    public Situation appMode = Situation.AlarmSituation;
    [SerializeField] GameObject[] characterPrefabs;
    [SerializeField] GameObject nowLoadingUI;

    [SerializeField] List<GameObject> NoDestroyList = new List<GameObject>();

    GameObject character; // 현재 스폰되어있는 캐릭터.


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        foreach (var obj in NoDestroyList)
            DontDestroyOnLoad(obj);

        // 세이브데이터 로드
        SaveManager.instance.Load();
        if (isDebug)
        {
            // do nothing
        }
        else
        {
            int? nowAlarmID;
            appMode = GetAppSituation(out nowAlarmID);
            if (appMode == Situation.AlarmSituation)
            {
                Debug.Assert(nowAlarmID != null, "Cannot get now Alarm ID!");
                CheckAndSetAlarmRepeat((int)nowAlarmID);
            }
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
            if(now.Hour == alarm.hour && now.Minute == alarm.minute)
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
        Debug.Assert(alarmToRepeat != null, "Cannot Find Alarm to Repeat!");
        AlarmData alarm = alarmToRepeat.Value;

        if (alarm.repeatDayInWeek[((int)DateTime.Now.DayOfWeek)] == true)
        {
            AndroidPluginLoader.Instance.SetAlarmWithDayOfWeek(alarm.alarmID * 10 + (int)DateTime.Now.DayOfWeek, ((int)DateTime.Now.DayOfWeek), alarm.hour, alarm.minute);
            Debug.Log("Alarm Repeat set");
        }
        else
            Debug.Log("Alarm Repeat not set because it doen't needed");
    }

    private GameObject getCharacterToSpawn()
    {
        Debug.Assert(SaveManager.instance.saveData.characterCode < characterPrefabs.Length, "Character code in savedata is out of range!!!");
        return characterPrefabs[SaveManager.instance.saveData.characterCode];
    }

    IEnumerator startMainScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");
        operation.allowSceneActivation = false;
        while(operation.progress < 0.9f)
        {
            Debug.Log(string.Format("loading Main scene : {0}%", operation.progress));
            yield return 0;
        }
        Debug.Log("Load Main scene almost Complete!!");

        SceneManager.activeSceneChanged += ChangedActiveScene;  // 씬 전환시 코루틴 끊어지는 문제로 인해 나머지는 ChangedActiveScene에서 이어서 실행.

        operation.allowSceneActivation = true;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (next.name != "MainScene")
            return;

        StartCoroutine(InitMainScene());
    }

    IEnumerator InitMainScene()
    {
        Debug.Log("InitMainScene");
        GameObject spawned = SpawnCharacter();
        spawned.SetActive(false);
        yield return new WaitForSeconds(1f);  // 앱 부팅 연출 시간 최소 1초
        spawned.SetActive(true);
        Destroy(nowLoadingUI);
    }

    private GameObject SpawnCharacter()
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Character").transform;
        Debug.Assert(spawnPoint != null, "Cannot find Character SpawnPoint!");
        if(spawnPoint.childCount > 0)   // 기존에 스폰된 캐릭터 있다면 정리
        {
            for(int i=0; i<spawnPoint.childCount; i++)
            {
                Destroy(spawnPoint.GetChild(i).gameObject);
            }
        }
        character = Instantiate(getCharacterToSpawn(), spawnPoint);
        return character;
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
