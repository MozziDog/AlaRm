using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharListUI : MonoBehaviour
{
    UIWindowManager windowManager;
    [SerializeField] GameObject loginUI;
    List<GameObject> charListElement;

    // Start is called before the first frame update
    void Start()
    {
        if(windowManager == null)
            windowManager = GetComponentInParent<UIWindowManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        WebRequestManager.instance.callbackSuccess += OnRefreshSuccess;
        WebRequestManager.instance.callbackFailure += OnRefreshFailure;
        StartCoroutine(WebRequestManager.instance.API_character_own());
        // TODO: 여기에 로딩 연출 만들기
    }

    public void OnRefreshSuccess()
    {
        // TODO: 여기에 로딩 연출 끝내고 element 표시하는 것 구현하기.
    }

    public void OnRefreshFailure()
    { 
        // TODO: 여기에 refresh 실패 안내 띄우는 코드 작성
    }


    public void OpenCharListUI()
    {
        windowManager.OpenWindow(gameObject);
        string currentLoginToken = SaveManager.instance.saveData.loginToken;
        if (currentLoginToken == null || currentLoginToken == "")
        {
            // open login UI
            windowManager.OpenWindow(loginUI);
        }
    }

    public void OnClickExitButton()
    {
        windowManager.CloseWindow(gameObject);
    }

}
