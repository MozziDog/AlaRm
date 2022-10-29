using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum CharacterStatus
{
    Normal,
    PrepareToWakingUp,
    WakingUp,
    AlarmInteraction,
}

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected float alarmInteractionTimeLimit = 10f;

    protected int alarmInteractionCount = 3;

    public CharacterStatus status { get; private set; }
    protected bool alarmInteractionClear = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        status = CharacterStatus.Normal;

        if (GameManager.instance.appMode == Situation.AlarmSituation)
        {
            StartCoroutine(AlarmSequence());
        }
    }
    IEnumerator AlarmSequence()
    {
        status = CharacterStatus.PrepareToWakingUp;
        while(true)
        {
            if (status == CharacterStatus.PrepareToWakingUp)
            {
                while (true)
                {
                    // 깨우기
                    if (status != CharacterStatus.WakingUp)
                    {
                        status = CharacterStatus.WakingUp;
                        PlayWakeUp();
                    }

                    if (Input.touchCount > 0) // 터치 입력이 있으면 기상미션으로 이행
                    {
                        status = CharacterStatus.AlarmInteraction;
                        break;
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            if (status == CharacterStatus.AlarmInteraction)
            {
                // 타이머 설정
                status = CharacterStatus.AlarmInteraction;
                float interactionStartTime = Time.time;

                // 기상 미션 제시
                Coroutine interactionCoroutine = null;
                string alarmInteraction = SelectAlarmInteraction();
                interactionCoroutine = StartCoroutine(alarmInteraction);
                alarmInteractionClear = false;

                // 기상미션 하는 동안 대기
                while (true) 
                {
                    if(alarmInteractionClear)
                    {
                        break;
                    }
                    else if (Time.time - interactionStartTime > alarmInteractionTimeLimit)
                    {
                        StopCoroutine(interactionCoroutine);
                        break;
                    }
                    else
                        yield return null;
                }
            }

            if (alarmInteractionClear) break;
        }

        // 기상 미션 클리어
        // 좋은 하루 되세요 인사
        PlayGreetings();

        // 일반 상태로 되돌아가기
        GameManager.instance.OnFinishedAlarmSequence();
    }

    void PlayWakeUp()
    {
        animator.SetTrigger("wakeUp");
        Debug.Log("일어나세요!");
    }

    string SelectAlarmInteraction()
    {
        int index = Random.Range(0, alarmInteractionCount);
        return "AlarmInteraction" + index;
    }

    protected virtual IEnumerator AlarmInteraction0()
    {
        Debug.Log("started AlarmInteraction0");
        yield return 0;
    }

    protected virtual IEnumerator AlarmInteraction1()
    {
        Debug.Log("started AlarmInteraction1");
        yield return 0;
    }

    protected virtual IEnumerator AlarmInteraction2()
    {
        Debug.Log("started AlarmInteraction2");
        yield return 0;
    }

    void PlayGreetings()
    {
        animator.SetTrigger("greeting");
        Debug.Log("좋은 하루 되세요!");
    }

    // animation clip에서 사운드 재생을 위해 호출하는 함수
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
