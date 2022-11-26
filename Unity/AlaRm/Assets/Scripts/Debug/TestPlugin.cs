using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlugin : MonoBehaviour
{
    public void OnClickShowToast()
    {
        Debug.Log("Clicked OnClickShowToast");
        AndroidPluginTest.Instance.ShowToast("Hi my name is lou.");
    }

    public void OnClickGetRandomNumber()
    {
        int value = AndroidPluginTest.Instance.GetRandomNumber();
        AndroidPluginTest.Instance.ShowToast(string.Format("value : {0}", value));
    }
}
