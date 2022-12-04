using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AndroidPluginLoader : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_ANDROID
    // private static AndroidJavaClass alarmAndoidPlugin = new AndroidJavaClass("com.example.plugin.UnityPlugin");
    private static AndroidJavaClass alarmAndoidPlugin = null;
#else
    private static AndroidJavaClass alarmAndoidPlugin = null;
#endif

    static AndroidPluginLoader instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    #if !UNITY_EDITOR && UNITY_ANDROID
        alarmAndoidPlugin = new AndroidJavaClass("com.example.plugin.UnityPlugin");
    #endif
}

public static AndroidPluginLoader Instance
    {
        get
        {
            if (instance == null)
            {
                //GameObject obj = new GameObject("Playio");
                //instance = obj.AddComponent<AndroidPluginLoader>();
                Debug.LogError("AndroidPluginLoader Instance should not be NULL");
            }
            return instance;
        }
    }

    public void ShowToast(string message)
    {
        AndroidJavaObject alarmManagerPlugin = alarmAndoidPlugin?.CallStatic<AndroidJavaObject>("instance");
        Debug.Assert(alarmManagerPlugin!=null,"alarmManagerPlugin is null!");
        if (alarmManagerPlugin != null)
        {
            alarmManagerPlugin.Call("showToast", message);
        }
        
        //testAndroidPlugin?.CallStatic("showToast", message);
    }

    /// <summary>
    /// dayOfWeek는 0~6, 순서대로 일월화수목금토
    /// hour는 0~23
    /// min은 0~59
    /// 설정하고자 하는 시간이 현재 시간보다 과거일 경우 
    /// 설정할 시간에 하루를 더해주는 것은 Java 코드의 책임
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dayOfWeek"></param>
    /// <param name="hour"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public bool SetAlarm(int id, int hour, int min) // true면 성공
    {
        AndroidJavaObject alarmManagerPlugin = alarmAndoidPlugin?.CallStatic<AndroidJavaObject>("instance");
        if (alarmManagerPlugin != null)
        {
            // 디버깅을 위해 플러그인에 return value가 존재하도록 다시 만들까?
            // java는 dayOfWeek의 범위가 1~7이라서 +1해줘야 함.
            alarmManagerPlugin.Call("setAlarm", id, hour, min); 
            return true;
        }
        else return false;
    }

    public bool SetAlarmWithDayOfWeek(int id, int dayOfWeek, int hour, int min)
    {
        Debug.Log("SetAlarmWithDayOfWeek:" + dayOfWeek);
        AndroidJavaObject alarmManagerPlugin = alarmAndoidPlugin?.CallStatic<AndroidJavaObject>("instance");
        if (alarmManagerPlugin != null)
        {
            // 디버깅을 위해 플러그인에 return value가 존재하도록 다시 만들까?
            // java는 dayOfWeek의 범위가 1~7이라서 +1해줘야 함.
            // java는 일요일이 1주일의 시작인데 스크립트에서 월요일이 1주일의 시작으로 설정해서 +1 해줘야 함.
            alarmManagerPlugin.Call("setAlarmWithDayOfWeek", id, (dayOfWeek + 1)%7 + 1, hour, min);
            return true;
        }
        else return false;
    }

    public bool CancelAlarm(int id)
    {
        AndroidJavaObject alarmManagerPlugin = alarmAndoidPlugin?.CallStatic<AndroidJavaObject>("instance");
        if (alarmManagerPlugin != null)
        {
            // 디버깅을 위해 플러그인에 return value가 존재하도록 다시 만들까?
            alarmManagerPlugin.Call("cancelAlarm", id); 
            return true;
        }
        else return false;
    }
}
