using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Prop
{
    private void Start()
    {
        StartCoroutine(BubbleFloating());
    }

    protected override void OnTouchBegan()
    {
        StartCoroutine(BubblePop());
    }

    IEnumerator BubblePop()
    {
        // TODO: 여기에 비누방울 터지는 연출 만들기
        yield return new WaitForSeconds(0.3f);
        bool removed = propList.Remove(this);
        Debug.Log("Bubble Removed : " + removed);
        Destroy(gameObject);
    }

    IEnumerator BubbleFloating()
    {
        float moveInterval;
        while (!propParameter.TryGetValue("propMovementInterval", out moveInterval))
        {
            Debug.LogWarning("BubbleFloating waiting for propMovementInterval");
            yield return null;
        }

        while(true)
        {
            float intervalStartTime = Time.time;
            Vector3 randomVector = new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1));
            randomVector.Normalize();
            randomVector *= 2; // 이 값 테스트 후 변경 필요
            while(intervalStartTime + moveInterval > Time.time)
            {
                transform.position += (randomVector / moveInterval) * Time.deltaTime;
                yield return null;
            }
        }
    }

    public override void DestroySelf()
    {
        StopAllCoroutines();
        StartCoroutine(BubblePop());
    }
}
