using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class signup
{
    public string loginId;
    public string password;
    public string name;
}

public class login
{
    public string loginId;
    public string password;
}

public class WebRequestManager : MonoBehaviour
{
    public static WebRequestManager instance;
    public string token = null;
    private string url = "http://54.180.167.202:80";
    private bool APICalling = false;
    public delegate void Callback();
    public Callback callbackSuccess;
    public Callback callbackFailure;

    private void Start()
    {
        instance = this; 
        token = SaveManager.instance.saveData.loginToken;
        if (token == "")
            token = null;
    }

    #region API_Func
    public  IEnumerator API_SignUp(string ID, string name, string PW)
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }

        signup signupData = new signup();
        signupData.loginId = ID;
        signupData.password = PW;
        signupData.name = name;
        byte[] jsonToSend = GetJSONencoded(signupData);

        UnityWebRequest request;
        using (request = UnityWebRequest.Post(GetApiPath(url, "user"), ""))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.uploadHandler.contentType = "application/json";
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                if (request.responseCode == 201)
                    AndroidPluginLoader.Instance.ShowToast("회원가입 성공!");
                else
                {
                    AndroidPluginLoader.Instance.ShowToast("회원가입에 실패했습니다");
                    Debug.LogWarning("회원가입 실패: " + request.responseCode);
                }
            }
        }

        APICalling = false;
    }

    /// <summary>
    /// API로 로그인하여 토큰을 가져오는 함수
    /// 이때 가져온 토큰은 token 변수에 저장
    /// </summary>
    /// <returns>token = Gettoken</returns>
    public  IEnumerator API_SignIn(string ID, string PW)
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }

        login loginData = new login();
        loginData.loginId = ID;
        loginData.password = PW;
        byte[] jsonToSend = GetJSONencoded(loginData);

        UnityWebRequest request;
        using (request = UnityWebRequest.Post(GetApiPath(url, "user/login"), ""))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.uploadHandler.contentType = "application/json";
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                Debug.Log(request.uri);
            }
            else
            {
                if (request.responseCode == 201)
                {
                    string token = request.GetResponseHeader("token");
                    SetToken(token);

                    Debug.Log("받아온 토큰: " + token);
                    AndroidPluginLoader.Instance.ShowToast("로그인 성공!");
                    callbackSuccess.Invoke();
                    ClearCallback();
                }
                else
                {
                    callbackFailure.Invoke();
                    ClearCallback();
                    Debug.LogWarning("로그인 실패: " + request.responseCode);
                }
            }
        }

        APICalling = false;
    }

    /// <summary>
    /// API로 Logout을 하는 함수.
    /// 로그아웃시 가지고 있던 토큰값은 초기화됨.
    /// </summary>
    /// <returns>token = null</returns>
    public  IEnumerator API_Logout()
    {
        AndroidPluginLoader.Instance.ShowToast("로그아웃되었습니다.");
        SetToken(null);
        yield return null;
    }

    public  IEnumerator API_character(int charCode)
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }

        UnityWebRequest request;
        using (request = UnityWebRequest.Post(GetApiPath(url, String.Format("character/{0:D2}", charCode)), ""))
        {
            request.SetRequestHeader("token", token);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                Debug.Log(request.uri);
            }
            else
            {
                if (request.responseCode == 201)
                {
                    // TODO: 구매가 효력 발휘하게 만들기
                    SaveManager.instance.saveData.characterOwn[charCode] = true;
                    callbackSuccess.Invoke();
                    ClearCallback();
                    AndroidPluginLoader.Instance.ShowToast("구매 성공!");
                }
                else if(request.responseCode == 400)
                {
                    callbackFailure.Invoke();
                    ClearCallback();
                    AndroidPluginLoader.Instance.ShowToast("구매 실패");
                }
                else
                {
                    callbackFailure.Invoke();
                    ClearCallback();
                    Debug.LogWarning("캐릭터 구매 실패: " + request.responseCode);
                }
            }
        }
        APICalling = false;
    }

    public  IEnumerator API_change_character(int charCode)
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }
        UnityWebRequest request;
        using (request = UnityWebRequest.Post(GetApiPath(url, String.Format("user/change_character/{0:D2}", charCode)), ""))
        {
            request.SetRequestHeader("token", token);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                Debug.Log(request.uri);
            }
            else
            {
                if (request.responseCode == 200 || request.responseCode == 201)
                {
                    callbackSuccess.Invoke();
                    Debug.Log("캐릭터 교체 성공");
                }
                else if (request.responseCode == 403)
                {
                    callbackFailure.Invoke();
                    ClearCallback();
                    Debug.LogWarning("캐릭터 교체 거절됨");
                    AndroidPluginLoader.Instance.ShowToast("잘못된 요청입니다.");
                }
                else
                {
                    ClearCallback();
                    Debug.LogWarning("캐릭터 교체 실패: " + request.responseCode);
                }
            }
        }
        APICalling = false;
    }

    public  IEnumerator API_character_own()
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(GetApiPath(url, "character/own")))
        {
            request.SetRequestHeader("token", token);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                Debug.Log(request.uri);
            }
            else
            {
                if (request.responseCode == 200)
                {
                    // TODO: 캐릭터 보유값 받아온 거 어떻게 전달하기
                    Debug.Log("캐릭터 보유 정보: "+request.downloadHandler.text);
                }
                else
                {
                    ClearCallback();
                    Debug.LogWarning("캐릭터 보유 정보 가져오기 실패: " + request.responseCode);
                }
            }
        }
        APICalling = false;
    }

    public  IEnumerator API_set_usercoin(int value)
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }
        UnityWebRequest request;
        using (request = UnityWebRequest.Post(GetApiPath(url, "user/coin"), ""))
        {
            request.SetRequestHeader("token", token);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                Debug.Log(request.uri);
            }
            else
            {
                if (request.responseCode == 200)
                {
                    Debug.Log("코인 보유량 업데이트 성공");
                }
                else
                {
                    ClearCallback();
                    Debug.LogWarning("코인 보유량 업데이트 실패: " + request.responseCode);
                }
            }
        }
        APICalling = false;
    }

    public  IEnumerator API_get_usercoin()
    {
        if (!CheckAPISingleton())
        {
            Debug.LogWarning("이미 진행중인 API 호출이 존재함");
            yield break;
        }
        UnityWebRequest request;
        using (request = UnityWebRequest.Get(GetApiPath(url, "user/coin")))
        {
            request.SetRequestHeader("token", token);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                Debug.Log(request.uri);
            }
            else
            {
                if (request.responseCode == 200)
                {
                    // TODO: 코인 보유값 받아온 거 어떻게 전달하기
                    Debug.Log("코인 보유량 받아오기 성공: " + request.downloadHandler.text);
                }
                else
                {
                    Debug.LogWarning("코인 보유량 받아오기 실패: " + request.responseCode);
                }
            }
        }
        APICalling = false;
    }

    /*
    /// <summary>
    /// 파일 다운로드
    /// </summary>
    public IEnumerator API_File_Download_ForImageUpdate(int id, Image _img)
    {
        UnityWebRequest request;
        using (request = UnityWebRequest.Get("http://___/download?id=" + id))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);

            string path = Path.Combine(Application.persistentDataPath, id + ".png");
            request.downloadHandler = new DownloadHandlerFile(path);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                // 이미지를 텍스처에 적용
                Texture2D tex = new Texture2D(64, 64, TextureFormat.DXT5, false);
                tex.LoadImage(request.downloadHandler.data);
                _img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                if (request.responseCode != 200)
                    ErrorCheck(-(int)request.responseCode, "API_File_Download" + request.url);
            }
        }
    }
    */

    public  byte[] GetJSONencoded(object obj)
    {
        return new System.Text.UTF8Encoding().GetBytes(JsonUtility.ToJson(obj));
    }

    int SetToken(string _input)
    {
        // 로그아웃시 토큰 초기화
        if (_input == null)
        {
            token = null;
            SaveManager.instance.saveData.loginToken = token;
            return 0;
        }

        // 로그인시 토큰 설정
        /*
        string[] temp = _input.Split('"');

        if (temp.Length != 5 || temp[1] != "token")
            ErrorCheck(-1001); // 토큰 형식 에러

        token = temp[3];
        */
        token = _input;
        SaveManager.instance.saveData.loginToken = token;
        return 0;
    }

    string GetApiPath(string addr, string api)
    {
        return addr + "/" + api;
    }

    bool CheckAPISingleton()
    {
        if (APICalling)
            return false;
        APICalling = true;
        return true;
    }

    public void ClearCallback()
    {
        Delegate[] delegates = callbackSuccess.GetInvocationList();
        foreach(var d in delegates)
        {
            callbackSuccess -= (Callback)d;
        }
        delegates = callbackFailure.GetInvocationList();
        foreach (var d in delegates)
        {
            callbackFailure -= (Callback)d;
        }
    }
    #endregion

    #region Occur Error
     int ErrorCheck(int _code)
    {
        if (_code > 0) return 0;
        else if (_code == -1000) Debug.LogError(_code + ", Internet Connect Error");
        else if (_code == -1001) Debug.LogError(_code + ", Occur token type Error");
        else if (_code == -1002) Debug.LogError(_code + ", Category type Error");
        else if (_code == -1003) Debug.LogError(_code + ", Item type Error");
        else Debug.LogError(_code + ", Undefined Error");
        return _code;
    }
    #endregion
}
