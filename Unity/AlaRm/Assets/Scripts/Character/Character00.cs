using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character00 : Character
{
    public Character00()
    {
        alarmInteractionCount = 1;
    }

    protected override IEnumerator AlarmInteraction0()
    {
        Debug.Log("started AlarmInteraction #0");
        animator.SetTrigger("alarmInteraction0");
        yield return new WaitForSeconds(5f);
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
