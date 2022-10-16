using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    bool isAlarmMode = false;

    // 안드로이드와의 통신용 함수
    void SetUnityMode(bool value)
    {
        isAlarmMode = value;
    }

    // 이 타이밍에 안드로이드와 메시지 교환이 발생함.
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        StartCoroutine(startMainScene());
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

}
