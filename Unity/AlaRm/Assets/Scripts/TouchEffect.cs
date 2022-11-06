using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    static float effectLifetime = 0.6f;

    private bool enabledFlag;

    void Start()
    {

    }

    void Update()
    {
        if (!enabledFlag)
            this.OnEnable();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitAndSetUnactive());
    }

    IEnumerator WaitAndSetUnactive()
    {
        yield return new WaitForSeconds(effectLifetime);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        enabledFlag = false;
    }
}
