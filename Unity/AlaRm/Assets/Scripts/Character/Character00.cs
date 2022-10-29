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
        yield return new WaitForSeconds(3f);
        alarmInteractionClear = true;
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
