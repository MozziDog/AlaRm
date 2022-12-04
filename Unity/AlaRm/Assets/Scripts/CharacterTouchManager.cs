using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class CharacterTouchManager : MonoBehaviour
{
    [SerializeField] UIWindowManager uiManager;
    public UI_test ui; // 목소리 나오기 전까지 테스트용 출력

    public TouchPhase? touchPhase = null;
    [SerializeField]
    private float stateHoldTime = 0.3f; // touchPhase 너무 자주 바뀌는 거 완충 시간

    //float minStrokeDistance = 0.3f;
    private UnityEngine.Touch theTouch;
    private float timeTouchStarted;
    private float timeTouchEnded;
    private float clickTimeLimit = 0.3f; // 이 이상 터치 지속되면 클릭이 아니라 홀드로 판정
    private bool touchedAtChara = false;
    //Vector2 screenTouchStartPoint;

    // Start is called before the first frame update
    void Start()
    {
        TouchSimulation.Enable();   // 마우스로 터치 시뮬레이션 활성화
        Debug.Assert(uiManager != null, "UI Manager refernce in CharacterTouchManager is null");
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.isUIStackEmpty && Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
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
                if (theTouch.phase == TouchPhase.Ended)
                {
                    this.touchPhase = TouchPhase.Ended;
                    touchedAtChara = false;
                    if (Time.time - timeTouchStarted < clickTimeLimit) // displayTime초간 디버그 메시지 출력
                    {
                        ui.printUI("click");
                    }
                    else
                    {
                        ui.printUI("touch ended");
                    }
                }
                else if (Time.time - timeTouchEnded > stateHoldTime) // 디버그 메시지 한 번 출력되면 displayTime간 변경 x
                {
                    this.touchPhase = theTouch.phase;
                    ui.printUI(theTouch.phase.ToString());
                    switch(theTouch.phase)
                    {
                        case TouchPhase.Began:

                            break;
                        case TouchPhase.Stationary:

                            break;
                        case TouchPhase.Moved:

                            break;
                    }
                    timeTouchEnded = Time.time;
                }
            }
        }
        else if(Time.time - timeTouchEnded > stateHoldTime)
        {
            ui.printUI("no touch");
            this.touchPhase = null;
        }
    }
}
