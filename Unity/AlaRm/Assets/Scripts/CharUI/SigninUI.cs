using QuantumTek.QuantumUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SigninUI : MonoBehaviour
{
    UIWindowManager windowManager;
    [SerializeField] QUI_Window signupWindow;
    [SerializeField] CharListUI charListUI;

    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField pwInput;

    // Start is called before the first frame update
    void Start()
    {
        if (windowManager == null)
            windowManager = GetComponentInParent<UIWindowManager>();
    }

    public void OnClickSigninButton()
    {
        Debug.Log("로그인 시도. ID: " + idInput.text);
        string id = idInput.text;
        string pw = pwInput.text;

        // api 호출
        WebRequestManager.instance.callbackSuccess += OnLoginSuccess;
        WebRequestManager.instance.callbackFailure += OnLoginFailure;
        StartCoroutine( WebRequestManager.instance.API_SignIn(id, pw) );
    }

    public void OnLoginSuccess()
    {
        Debug.Log("LoginSucess Callback!");
        windowManager.CloseWindow(gameObject);
        charListUI.Refresh();
    }

    public void OnLoginFailure()
    {
        AndroidPluginLoader.Instance.ShowToast("잘못된 로그인 정보입니다");
        pwInput.text = "";
    }

    public void OnClickSignupButton()
    {
        windowManager.OpenWindow(signupWindow);
    }

    public void OnClickExitButton()
    {
        windowManager.CloseWindow(gameObject);
        charListUI.OnClickExitButton();
    }
}
