using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    bool isAlarmMode = false;

    // �ȵ���̵���� ��ſ� �Լ�
    void SetUnityMode(bool value)
    {
        isAlarmMode = value;
    }

    // �� Ÿ�ֿ̹� �ȵ���̵�� �޽��� ��ȯ�� �߻���.
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
        yield return new WaitForSeconds(1.5f);  // �� ���� ���� �ð� �ּ� 1.5��
        while(operation.progress < 0.9f)
        {
            Debug.Log(string.Format("loading Main scene : {0}%", operation.progress));
            yield return 0;
        }
        Debug.Log("Load Main scene almost Complete!!");
        operation.allowSceneActivation = true;
    }

}
