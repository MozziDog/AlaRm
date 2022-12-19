using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestJson : MonoBehaviour
{

    public static string savePath;
    SaveData saveData0;
    SaveData saveData1;
    public string saveFile;

    [Serializable]
    public struct SaveData
    {
        public List<AlarmData> alarms;
        public int characterCode;
        public bool[] characterOwn;
        public string loginToken;
    }

    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.persistentDataPath + "/savedata.json";
        string savejson = File.ReadAllText(savePath);
        JsonUtility.FromJsonOverwrite(savejson, saveData0);
        Debug.Log("saveData0.characterCode: " + saveData0.characterCode);
        saveData1 = JsonUtility.FromJson<SaveData>(savejson);
        Debug.Log("saveData1.characterCode: " + saveData1.characterCode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
