using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlugin : MonoBehaviour
{
    public void Start()
    {
        SaveManager.instance.Load();
    }
    public void OnClickTestButton()
    {
        Debug.Log("Clicked OnClickShowToast");
        SaveManager.instance.saveData.alarms.Add(new AlarmData());
    }
}
