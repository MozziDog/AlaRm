using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharListUI : MonoBehaviour
{
    UIWindowManager windowManager;
    [SerializeField] GameObject loginUI;
    List<GameObject> charListElement;
    [SerializeField] Transform charListHolder;

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
        for(int i=0; i<charListHolder.childCount; i++)
        {
            charListHolder.GetChild(i).GetComponent<CharListElement>().CheckOwned();
        }
    }

    public void OnRefreshFailure()
    {
        AndroidPluginLoader.Instance.ShowToast("정보를 가져오는데 실패했습니다.");
        Debug.LogWarning("Refesh Charlist Failure");
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
        else
        {
            Refresh();
        }
    }

    public void OnClickExitButton()
    {
        windowManager.CloseWindow(gameObject);
    }

}
