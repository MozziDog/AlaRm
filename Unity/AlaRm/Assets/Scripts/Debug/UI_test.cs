using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_test : MonoBehaviour
{

    public TextMeshProUGUI console;
    // Start is called before the first frame update
    void Start()
    {
        console.text = "Hello, World!";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void printUI(string outText)
    {
        console.text = outText;
    }
}
