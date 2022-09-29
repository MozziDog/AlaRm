using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public UI_test ui;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.developerConsoleVisible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        //if(Input.GetMouseButtonDown(0))
        {
            Vector2 screenPoint = Input.GetTouch(0).position;
            //Vector2 screenPoint = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            LayerMask layerMask = LayerMask.GetMask("Touchable");
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask) && hit.collider.tag.CompareTo("Character") == 0)
            {
                ui.printUI("hit: "+(++count));
            }
        }
    }
}
