using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWebAPI : MonoBehaviour
{
    public WebRequestManager m_WebRequestManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickTestButton()
    {
        Debug.Log("clicked test button");
        StartCoroutine(m_WebRequestManager.API_SignIn("test", "test"));
    }

    public void OnClickAddUserButton()
    {
        Debug.Log("clicked new user button");
        StartCoroutine(m_WebRequestManager.API_SignUp("User001", "이즈리얼", "내솜씨를제대로보여줄시간이군!"));
    }
}
