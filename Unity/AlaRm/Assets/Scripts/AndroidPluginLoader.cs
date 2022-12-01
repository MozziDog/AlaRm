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

    public bool SetAlarm(int id, int hour, int min) // true면 성공
    {
        AndroidJavaObject alarmManagerPlugin = alarmAndoidPlugin?.CallStatic<AndroidJavaObject>("instance");
        if (alarmManagerPlugin != null)
        {
            // 디버깅을 위해 플러그인에 return value가 존재하도록 다시 만들까?
            alarmManagerPlugin.Call("setAlarm", id, hour, min); 
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
