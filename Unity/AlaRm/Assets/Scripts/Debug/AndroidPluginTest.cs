using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidPluginTest : MonoBehaviour
{
    private static AndroidJavaClass androidPlugin = new AndroidJavaClass("com.example.unityplugin.UnityPluginWrapper");

    static AndroidPluginTest instance;

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
    }

    public static AndroidPluginTest Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("Playio");
                instance = obj.AddComponent<AndroidPluginTest>();
            }
            return instance;
        }
    }

    public void ShowToast(string message)
    {
//#if !UNITY_EDITOR && UNITY_ANDROID
    androidPlugin.CallStatic("showToast", message);
//#endif
    }
    public int GetRandomNumber()
    {
//#if !UNITY_EDITOR && UNITY_ANDROID
        return androidPlugin.CallStatic<int>("getRandomNumber");
//#endif
    }

    public void OnReceiveRandomNumber(string number)
    {
        Debug.Log("Got random number:" + number);
    }
}
