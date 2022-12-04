using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gyroTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Debug: 자이로가 처음부터 활성화되어있는지, 그래서 틸트 미션이 끝나면 꺼줘야 하는지 테스트
        Debug.Log("gyro: " + Input.gyro.enabled);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
