using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character00 : Character
{
    [Header("Props")]
    [SerializeField] private GameObject BubblePrefab;
    [SerializeField] private GameObject ToothbrushPrefab;
    public Character00()
    {
        ALARM_INTERACTION_COUNT = 2;
    }

    // Alarm Interaction #0 비누방울
    protected override IEnumerator AlarmInteraction0(int difficulty)
    {
        int PROP_COUNT = 2 + difficulty;
        float PROP_MOVEMENT_INTERVAL = 3f / (1+ (difficulty-1)/10f);

        Debug.Log("started AlarmInteraction #0");
        animator.SetTrigger("alarmInteraction0");
        yield return new WaitForSeconds(2f);
        for(int i= 0; i < PROP_COUNT; i++)
        {
            Vector3 instancePosition = this.transform.position;
            instancePosition += new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            Prop prop = SpawnProp(BubblePrefab, instancePosition, Quaternion.identity).GetComponent<Prop>();
            prop.SetPropParameter("propMovementInterval", PROP_MOVEMENT_INTERVAL);
        }

        while(true)
        {
            int leftBubble = Prop.propList.Count;
            if(leftBubble == 0) break;
            yield return null;
        }
        alarmInteractionClear = true; // 이 플래그가 서면 좋은하루 보내세요 하고 끝남.
    }

    // Alarm Interaction #1 칫솔
    protected override IEnumerator AlarmInteraction1(int difficulty)
    {
        float ALLOWED_DISTANCE = 0.3f - 0.03f * (difficulty - 1);
        float REQ_TIME = 3f + difficulty;
        Debug.Log("started AlarmInteraction #1");
        animator.SetTrigger("alarmInteraction1");
        yield return new WaitForSeconds(2f);
        Vector3 instancePosition = this.transform.position;
        instancePosition += new Vector3(1, -1, -1.8f);
        Prop prop = SpawnProp(ToothbrushPrefab, instancePosition, Quaternion.Euler(-50, 30, 60)).GetComponent<Prop>();
        prop.SetPropParameter("allowedDistance", ALLOWED_DISTANCE);
        prop.SetPropParameter("reqTime", REQ_TIME);

        while (true)
        {
            if(Prop.propList.Count == 0) break; // 칫솔은 일정 시간 양치되고 나면 알아서 사라짐
            yield return null;
        }
        alarmInteractionClear = true; // 이 플래그가 서면 좋은하루 보내세요 하고 끝남.
    }

    // Alarm Interaction #2 끄덕끄덕
    protected override IEnumerator AlarmInteraction2(int difficulty)
    {
        int tiltCount = 0, reqCount = 2 + difficulty;
        // TODO: 이 값 실험으로 적정값 찾아서 고쳐야 함.
        float reqRotationSpeed = 4 * (0.8f + 0.2f * difficulty); 
        float insensitivityTime = 0.5f;    // 핸드폰 흔들때 너무 빠르게 카운트 올라가는 거 방지용
        float lastCountTime = Time.time;

        // 실험용
        float maxRotationSpeed = 0;

        Debug.Log("started AlarmInteraction #2");
        Input.gyro.enabled = true;  // 자이로 센서 활성화

        animator.SetTrigger("alarmInteraction2");
        yield return new WaitForSeconds(2f);

        // 디버깅용, 실험용 출력 창 준비
        var debugConsole = GameObject.Find("Prototype Manager").GetComponent<UI_test>();

        bool up = false, down = false;
        while (true)
        {
            Vector3 tiltRotation = Input.gyro.rotationRateUnbiased;
            // 디버깅용
            if(maxRotationSpeed < Mathf.Abs(tiltRotation.x))
                maxRotationSpeed = Mathf.Abs(tiltRotation.x);
            debugConsole.printUI(string.Format("TiltEnabled: {0}\nTilt Angle: {1}\nMaxRotationSpeed: {2}\ntileCount: {3}", Input.gyro.enabled,tiltRotation, maxRotationSpeed,tiltCount));
            // 디버깅용 끝
            if(tiltRotation.x > reqRotationSpeed) // 충분한 세기로 흔들고
            {
                up = true;
            }
            if(tiltRotation.x < -reqRotationSpeed)
            {
                down = true;
            }
            if(up && down && (lastCountTime + insensitivityTime < Time.time)) // 이전 끄덕끄덕 인정으로부터 충분한 시간이 지났을 때
            {
                tiltCount++;
                lastCountTime = Time.time;
                up = false; down = false;
                // TODO: 여기에 끄덕끄덕 한번에 대한 리액션 연출 넣기
            }
            if (tiltCount >= reqCount)
                break;
            yield return null;
        }
        alarmInteractionClear = true; // 이 플래그가 서면 좋은하루 보내세요 하고 끝남.
    }
}
