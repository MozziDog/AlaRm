using QuantumTek.QuantumUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 윈도우를 스택으로 관리하지 않으면 뒤로가기 눌렀을 때 적절하게 반응하기 어려울 듯.
public class UIWindowManager : MonoBehaviour
{
    // push와 pop을 통해 관리하는 UI 윈도우들
    Stack<QUI_Window> windowStack;
    [SerializeField]
    List<QUI_Window> windowList; // 디버그용

    // push와 pop을 통해 관리하지 않는 UI 윈도우들
    [SerializeField]
    List<QUI_Window> defaultUI;

    [SerializeField]
    List<QUI_Window> alarmSequenceUI;

    private void Start()
    {
        if(windowStack == null)
            windowStack = new Stack<QUI_Window>();
        SetUIMode(GameManager.instance.appMode);
    }

    public void SetUIMode(Situation appMode)
    {
        Debug.Log("appmode: " + appMode);
        if (appMode == Situation.NormalSituation)
        {
            // value가 true이면 alarmMode
            foreach (QUI_Window window in defaultUI)
            {
                window.startActive = true;
                window.SetActive(true); ;
            }
            foreach (QUI_Window window in alarmSequenceUI)
            {
                window.startActive = false;
                window.SetActive(false);
            }
        }
        else 
        {
            // value가 true이면 alarmMode
            foreach (QUI_Window window in defaultUI)
                window.SetActive(false);
            foreach (QUI_Window window in alarmSequenceUI)
                window.SetActive(true);
        }
    }

    public void OpenWindow(QUI_Window window)
    {
        if(window == null)
        {
            Debug.LogError("잘못된 UI창 열기 요청");
            return;
        }
        window.SetActive(true);
        windowStack.Push(window);
        refreshList();
    }

    public void OpenWindow(GameObject UIObject)
    {
        OpenWindow(UIObject.GetComponent<QUI_Window>());
    }

    public void CloseWindow()
    {
        if(windowStack.Count > 0)
        {
            windowStack.Pop().SetActive(false);
        }
        else
        {
            Debug.LogWarning("더 이상 닫을 UI 창이 없습니다.");
        }
        refreshList();
    }

    public void CloseWindow(QUI_Window window)
    {
        if (window == null)
        {
            Debug.LogError("잘못된 UI창 닫기 요청");
            return;
        }
        if (windowStack.Contains(window))
        {
            while(windowStack.Count > 0)
            {
                var w = windowStack.Pop();
                w.SetActive(false);
                if (w == window)
                    break;
            }
        }
        else
            Debug.LogWarning("닫을 대상 UI창이 없음!");
        refreshList();
    }

    public void CloseWindow(GameObject UIObject)
    {
        CloseWindow(UIObject.GetComponent<QUI_Window>());
    }
    
    public void CloseAllWindow()
    {
        if (windowStack.Count == 0)
        {
            Debug.LogWarning("CloseAllWindow : UI 스택이 비어있음");
            return;
        }
        while(windowStack.Count > 0)
        {
            windowStack.Pop().SetActive(false);
        }
        refreshList();
    }

    // 디버그용
    void refreshList()
    {
        windowList = new List<QUI_Window>(windowStack); 
    }

}
