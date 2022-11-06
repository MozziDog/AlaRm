using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffectSpawner : MonoBehaviour
{
    public GameObject effectPrefab;

    private List<GameObject> effectPool = new List<GameObject> ();
    const int MAX_POOL_COUNT = 5;

    // Start is called before the first frame update
    void Start()
    {
        effectPool.Clear ();
        for(int i=0; i<MAX_POOL_COUNT; i++)
        {
            GameObject effectInstance = GameObject.Instantiate(effectPrefab, gameObject.transform);
            effectInstance.SetActive (false);
            effectPool.Add (effectInstance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                int i = GetNextIndex();
                effectPool[i].transform.position = Input.GetTouch(0).position;
                effectPool[i].SetActive(true);
            }

            if(Input.touchCount > 1)
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    int i = GetNextIndex();
                    effectPool[i].transform.position = Input.GetTouch(1).position;
                    effectPool[i].SetActive(true);
                }
        }
    }

    int GetNextIndex()
    {
        int val = nextIndex;
        nextIndex = (nextIndex + 1) % MAX_POOL_COUNT;
        return val;
    }
    int nextIndex = 0;
}
