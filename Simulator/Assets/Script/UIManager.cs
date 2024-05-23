using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button printBtn;
    public Button updateBtn;
    // Start is called before the first frame update
    void Start()
    {
        printBtn.onClick.AddListener(OnPrintValue);
        updateBtn.onClick.AddListener(OnStartSimulation);
    }

    // Update is called once per frame
    void OnPrintValue()
    {
        foreach (var value in TCPServer.instance.parameters)
        {
            print(value.Value);
        }
    }

    void OnStartSimulation()
    {
       
    }
}
