using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectActive : MonoBehaviour
{
    bool isActiveOnLastFrame;
    // Start is called before the first frame update
    void Awake()
    {
        isActiveOnLastFrame = gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActiveOnLastFrame != gameObject.activeSelf)
        {
            Debug.Log("Activated : " + gameObject.activeSelf);
            isActiveOnLastFrame = gameObject.activeSelf;
        }
    }
}
