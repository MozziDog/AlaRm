using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class CharacterTouchManager : MonoBehaviour
{
    public UI_test ui; // ��Ҹ� ������ ������ �׽�Ʈ�� ���
    //float minStrokeDistance = 0.3f;
    private UnityEngine.Touch theTouch;
    private float timeTouchStarted;
    private float timeTouchEnded;   // ��ġ ���� �� 0.5�ʱ��� ����� �޽��� ��¿�
    private float clickTimeLimit = 0.3f; // �� �̻� ��ġ ���ӵǸ� Ŭ���� �ƴ϶� Ȧ��� ����
    private float displayTime = 0.5f; // ������
    private bool touchedAtChara = false;
    //Vector2 screenTouchStartPoint;

    // Start is called before the first frame update
    void Start()
    {
        TouchSimulation.Enable();   // ���콺�� ��ġ �ùķ��̼� Ȱ��ȭ
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
                    if (Time.time - timeTouchStarted < clickTimeLimit) // 0.5�ʰ� ����� �޽��� ���
                    {
                        ui.printUI("click");
                    }
                    else
                    {
                        ui.printUI("touch ended");
                    }
                }
                else if (Time.time - timeTouchEnded > displayTime) // ����� �޽��� �� �� ��µǸ� 0.5�ʰ� ���� x
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
