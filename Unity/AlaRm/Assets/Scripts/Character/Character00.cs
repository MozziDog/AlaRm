using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character00 : Character
{
    [SerializeField] private GameObject BubblePrefab;
    public Character00()
    {
        alarmInteractionCount = 1;
    }

    // Alarm Interaction #1 비누방울
    protected override IEnumerator AlarmInteraction0()
    {
        Debug.Log("started AlarmInteraction #0");
        animator.SetTrigger("alarmInteraction0");
        yield return new WaitForSeconds(2f);
        for(int i= 0; i < 3; i++)
        {
            Vector3 instancePosition = this.transform.position;
            instancePosition += new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            SpawnProp(BubblePrefab, instancePosition, Quaternion.identity);
        }
        while(true)
        {
            int leftBubble = Prop.propList.Count;
            if(leftBubble == 0) break;
            yield return null;
        }
        alarmInteractionClear = true; // 이 플래그가 서면 좋은하루 보내세요 하고 끝남.
    }

    protected override IEnumerator AlarmInteraction1()
    {
        Debug.Log("started AlarmInteraction #1");
        yield return 0;
        // alarmInteractionClear = true;
    }

    protected override IEnumerator AlarmInteraction2()
    {
        Debug.Log("started AlarmInteraction #2");
        yield return 0;
        // alarmInteractionClear = true;
    }
}
