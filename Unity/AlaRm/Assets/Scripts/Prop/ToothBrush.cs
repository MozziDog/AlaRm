using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ToothBrush : Prop
{
    [ReadOnly, SerializeField]
    bool isTouchStay = false;
    [SerializeField] 
    float offsetDisplay;

    float allowedDistance = 0.3f, reqTime;


    // TODO : 디버깅 완료되면 private로 만들기
    [SerializeField] Transform charaMouth;
    [SerializeField] Transform brushHead;

    public float duration = 0f;

    private void Start()
    {
        Debug.Assert(propParameter.TryGetValue("allowedDistance", out allowedDistance));
        Debug.Assert(propParameter.TryGetValue("reqTime", out reqTime));
        Debug.Log("allowedDistance: " + allowedDistance);
        Debug.Log("reqTime: " + reqTime);
    }

    protected override void OnTouchBegan()
    {
        isTouchStay = true;
        if (charaMouth == null)
            charaMouth = GameObject.Find("Mouth").transform;
    }

    protected override void Update()
    {
        base.Update();
        if(isTouchStay)
        {
            Touch theTouch = Input.GetTouch(0);
            if(theTouch.phase == TouchPhase.Ended)
            {
                isTouchStay = false;
                return;
            }
            var touchPosition = theTouch.position;
            var touchRay = Camera.main.ScreenPointToRay(touchPosition);
            Vector3 touchDirection = touchRay.direction;
            Vector3 targetPosition = Camera.main.gameObject.transform.position + touchDirection * 4;
            targetPosition += new Vector3(0.55f, -0.15f, 0); // 적절한 손잡이 위치 오프셋
            gameObject.transform.position = targetPosition;

            // 칫솔 머리가 적절한 위치인지 체크
            Vector3 toothbrushHeadPosition = brushHead.position;
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 charaMouthPosition = charaMouth.position;
            Debug.DrawLine(cameraPosition, toothbrushHeadPosition);
            Debug.DrawLine(cameraPosition, charaMouthPosition);

            Vector3 cam2mouth = charaMouthPosition - cameraPosition;
            Vector3 cam2brush = toothbrushHeadPosition - cameraPosition;
            Vector3 projection = (Vector3.Dot(cam2mouth, cam2brush) / cam2mouth.magnitude) * cam2mouth.normalized;
            float offset = (cam2brush - projection).magnitude;

            // 디버깅용
            offsetDisplay = offset;

            if(offset < allowedDistance)
            {
                duration += Time.deltaTime;

                if(duration > 5f)
                {
                    DestroySelf();
                }
            }
        }
    }

    public override void DestroySelf()
    {
        StartCoroutine(DestroyEffect());
    }

    IEnumerator DestroyEffect()
    {
        // TODO: 여기에 칫솔 없어지는 연출 만들기
        yield return new WaitForSeconds(1f);
        bool removed = propList.Remove(this);
        Debug.Log("ToothBrush Removed : " + removed);
        Destroy(gameObject);
    }
        
}
