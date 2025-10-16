using UnityEngine;
using UnityEngine.UI;

public class DisplayTime : MonoBehaviour
{
    [SerializeField]
    Text display;

    float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.timeSinceLevelLoad;
        display.text = "Time: " + time.ToString("F1") + "s";    
    }
}
