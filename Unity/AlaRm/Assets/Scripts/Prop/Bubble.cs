using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : Prop
{
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

    public override void DestroySelf()
    {
        StartCoroutine(BubblePop());
    }
}
