using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class CharacterTouchManager : MonoBehaviour
{
    public UI_test ui; // 목소리 나오기 전까지 테스트용 출력
    //float minStrokeDistance = 0.3f;
    private UnityEngine.Touch theTouch;
    private float timeTouchStarted;
    private float timeTouchEnded;   // 터치 종료 후 0.5초까지 디버그 메시지 출력용
    private float clickTimeLimit = 0.3f; // 이 이상 터치 지속되면 클릭이 아니라 홀드로 판정
    private float displayTime = 0.5f; // 디버깅용
    private bool touchedAtChara = false;
    //Vector2 screenTouchStartPoint;

    // Start is called before the first frame update
    void Start()
    {
        TouchSimulation.Enable();   // 마우스로 터치 시뮬레이션 활성화
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == UnityEngine.TouchPhase.Began)
            {
                Vector2 screenPoint = theTouch.position;
                Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                LayerMask layerMask = LayerMask.GetMask("Touchable");
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask) && hit.collider.tag.CompareTo("Character") == 0)
                {
                    touchedAtChara = true;
                    timeTouchStarted = Time.time;
                }
            }
            if (touchedAtChara == true)
            {
                if (theTouch.phase == UnityEngine.TouchPhase.Ended)
                {
                    touchedAtChara = false;
                    if (Time.time - timeTouchStarted < clickTimeLimit) // 0.5초간 디버그 메시지 출력
                    {
                        ui.printUI("click");
                    }
                    else
                    {
                        ui.printUI("touch ended");
                    }
                }
                else if (Time.time - timeTouchEnded > displayTime) // 디버그 메시지 한 번 출력되면 0.5초간 변경 x
                {
                    ui.printUI(theTouch.phase.ToString());
                    switch(theTouch.phase)
                    {
                        case UnityEngine.TouchPhase.Began:

                            break;
                        case UnityEngine.TouchPhase.Stationary:

                            break;
                        case UnityEngine.TouchPhase.Moved:

                            break;
                    }
                    timeTouchEnded = Time.time;
                }
            }
        }
        else if(Time.time - timeTouchEnded > displayTime)
        {
            ui.printUI("no touch");
        }
    }
}
