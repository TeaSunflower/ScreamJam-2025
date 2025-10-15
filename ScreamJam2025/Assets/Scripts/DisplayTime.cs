using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    [SerializeField]
    Text display;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        display.text = "Time: " + (Time.timeSinceLevelLoad).ToString("F1");    
    }
}
