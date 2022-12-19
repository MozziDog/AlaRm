using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Android;

[Serializable] public struct SaveData
{
    public List<AlarmData> alarms;
    public int characterCode;
    public bool[] characterOwn;
    public string loginToken;
    public int coin;
}

public class SaveManager : MonoBehaviour
{
    // savePath를 Awake에서 초기화해야 하기 때문에 non-static 기반으로 만듦.
    public static SaveManager instance { get; private set; }
    public static string savePath; 

    public SaveData saveData;

    private void Awake()
    {
        instance = this;
        // MonoBehaviour 생성자에서 Application.persistentDataPath 접근 불가, Awake에서 초기화함.
        savePath = Application.persistentDataPath + "/savedata.json";
    }

    public void Save()
    {
        string savejson = JsonUtility.ToJson(saveData, true); // prettyPrint ON
        Debug.Log(savejson);
        File.WriteAllText(savePath, savejson);
        Debug.Log("data saved!\n" + saveData.alarms.ToString());
    }

    public void Load()
    {
        saveData = new SaveData();
        saveData.alarms = new List<AlarmData>();
        saveData.characterCode = 0;
        saveData.characterOwn = new bool[] { true, false };
        saveData.loginToken = "";
        saveData.coin = 200;
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Savedata not exist : Creating new one...");
            Save();
            return;
        }
        string savejson = File.ReadAllText(savePath);
        saveData = JsonUtility.FromJson<SaveData>(savejson);
        JsonUtility.FromJsonOverwrite(savejson, saveData);
        Debug.Log("data loaded!\n"+savejson);
    }

}
