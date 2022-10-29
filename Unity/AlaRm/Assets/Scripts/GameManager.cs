using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] // 개발 완료되면 readonly로 수정할 것. 
    public Situation appMode = Situation.AlarmSituation;

    [SerializeField] GameObject characterToSpawn; // 개발용. 일단 임시로 스폰할 캐릭터를 인스펙터에서 명시
    GameObject character; // 현재 스폰되어있는 캐릭터.

    // 이 타이밍에 안드로이드와 메시지 교환이 발생함.
    void Awake()
    {
        appMode = GetAppSituation();
    }

    Situation GetAppSituation()
    {
        // TODO: 여기에 안드로이드와 통신하여 situation 가져오는 기능 만들기
        return appMode; // 일단 임시로 사용자 지정 값 사용
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        instance = this;
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
