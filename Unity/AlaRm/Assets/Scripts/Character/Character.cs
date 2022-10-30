using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] protected float backToSleepTimeLimit = 10f;
    [SerializeField] protected List<KeyValuePair<string, AudioClip>> audioClips;

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
        while (true)
        {
            if (status == CharacterStatus.PrepareToWakingUp)
            {
                while (true)
                {
                    // 깨우기
                    if (status != CharacterStatus.WakingUp)
                    {
                        status = CharacterStatus.WakingUp;
                        SetAnimationTrigger("wakeUp");
                    }
                    if (Input.touchCount > 0) // 터치 입력이 있으면 기상미션으로 이행
                    {
                        status = CharacterStatus.AlarmInteraction;
                        SetAnimationTrigger("wakeUpCheck");
                        break;
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            else if (status == CharacterStatus.AlarmInteraction)
            {
                // 타이머 설정
                status = CharacterStatus.AlarmInteraction;
                float lastTouchStartTime = Time.time;
                CharacterTouchManager touchManager = gameObject.GetComponent<CharacterTouchManager>();

                // 기상 미션 제시
                Coroutine interactionCoroutine = null;
                string alarmInteraction = SelectAlarmInteraction();
                interactionCoroutine = StartCoroutine(alarmInteraction);
                alarmInteractionClear = false;

                // 기상미션 하는 동안 대기
                while (true)
                {
                    if (alarmInteractionClear)
                    {
                        break;
                    }
                    if (touchManager.touchPhase == TouchPhase.Began)
                    {
                        lastTouchStartTime = Time.time;
                    }
                    // 사용자가 다시 잠든 경우
                    if (Time.time - lastTouchStartTime > backToSleepTimeLimit)
                    {
                        status = CharacterStatus.PrepareToWakingUp;
                        StopCoroutine(interactionCoroutine);
                        break;
                    }
                    else
                        yield return null;
                }
            }
            else
            {
                yield return null;  // 무한 루프로 인한 freeze 방지용
            }
            if (alarmInteractionClear) break;
        }

        // 기상 미션 클리어
        // 좋은 하루 되세요 인사
        PlayGreetings();

        // 일반 상태로 되돌아가기
        GameManager.instance.OnFinishedAlarmSequence();
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

    public void SetAnimationTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void SetRandomAnimationTrigger(string baseString, int variationCount)
    {
        animator.SetTrigger(baseString + Random.Range(0, variationCount));
    }

    public void SetAnimationRepeatTrigger()
    {
        animator.SetTrigger("repeat");
    }

    public void SetAnimationRandomInt()
    {
        animator.SetInteger("random", Random.Range(0, 2));
    }

    // 사운드는 스크립트 상에서 직접 호출하지 말고 애니메이션클립에서 호출하도록 할 것.
    public void PlaySound(AudioClip clip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.PlayOneShot(clip);
    }

    public virtual void PlayRandomSound(string clipCategory)
    {
        List<KeyValuePair<string, AudioClip>> clips = audioClips.FindAll((el) => el.Key == clipCategory);
        PlaySound(clips[Random.Range(0, clips.Count)].Value);
    }
}