using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public static List<Prop> propList = new List<Prop>();
    Collider col;
    public Dictionary<string, float> propParameter = new Dictionary<string, float>();

    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch theTouch = Input.GetTouch(0);
            Vector2 screenPoint = theTouch.position;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            Debug.DrawLine(ray.origin, ray.direction);
            LayerMask layerMask = LayerMask.GetMask("Touchable");
            RaycastHit[] hits = Physics.RaycastAll(ray, float.MaxValue, layerMask);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == transform)
                    switch(theTouch.phase)
                    {
                        case TouchPhase.Began:
                            OnTouchBegan();
                            break;
                        case TouchPhase.Moved:
                        case TouchPhase.Stationary:
                            OnTouchStay();
                            break;
                        case TouchPhase.Ended:
                            OnTouchEnded();
                            break;
                    }
            }

        }
    }

    protected virtual void OnTouchBegan()
    {

    }

    protected virtual void OnTouchStay()
    {

    }

    protected virtual void OnTouchEnded()
    {

    }

    public void SetPropParameter(string key, float value)
    {
        if (!propParameter.TryAdd(key, value))
        {
            propParameter[key] = value;
        }
    }
    
    // 모든 Prop은 DestroySelf를 구현하여 삭제 명령이 들어오면 스스로를 삭제할 수 있어야 함.
    public virtual void DestroySelf()
    {
        if (propList.Find((el) => el == this))
            propList.Remove(this);
        Destroy(gameObject);
    }

    public static GameObject SpawnProps(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject spawned = Instantiate(prefab, pos, rot);
        propList.Add(spawned.GetComponent<Prop>());
        return spawned;
    }

    public static void ClearAllProps()
    {
        foreach(var prop in propList)
        {
            prop.DestroySelf();
        }
        propList.Clear();
    }
}
