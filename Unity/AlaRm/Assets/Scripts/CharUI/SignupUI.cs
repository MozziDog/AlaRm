using QuantumTek.QuantumUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignupUI : MonoBehaviour
{
    UIWindowManager windowManager;

    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField pwInput;
    [SerializeField] TMP_InputField pwCheckInput;

    // Start is called before the first frame update
    void Start()
    {
        if (windowManager == null)
            windowManager = GetComponentInParent<UIWindowManager>();
    }

    public void OnClickSignupButton()
    {
        Debug.Log("회원가입 시도. ID: " + idInput.text);
        string id = idInput.text;
        string name = nameInput.text;
        string pw = pwInput.text;
        string pwCheck = pwCheckInput.text;

        if(id.Length < 5)
        {
            AndroidPluginLoader.Instance.ShowToast("ID가 너무 짧습니다.");
            idInput.text = "";
            return;
        }
        if (name.Length < 1)
        {
            AndroidPluginLoader.Instance.ShowToast("이름을 입력해주세요");
            nameInput.text = "";
            return;
        }
        if (pw != pwCheck)
        {
            AndroidPluginLoader.Instance.ShowToast("비밀번호 확인이 올바르지 않습니다.");
            pwInput.text = "";
            pwCheckInput.text = "";
            return;
        }

        // api 호출
        WebRequestManager.instance.callbackSuccess += OnSignUpSuccess;
        WebRequestManager.instance.callbackFailure += OnSignUpFailure;
        StartCoroutine(WebRequestManager.instance.API_SignUp(id, name, pw));
    }

    public void OnSignUpSuccess()
    {
        Debug.Log("SignupSuccess Callback!");
        windowManager.CloseWindow(gameObject);
    }

    public void OnSignUpFailure()
    {
        Debug.Log("SignupFailure Callback");
        AndroidPluginLoader.Instance.ShowToast("잘못된 로그인 정보입니다");
        pwInput.text = "";
    }

    public void OnClickExitButton()
    {
        windowManager.CloseWindow(gameObject);
    }
}
