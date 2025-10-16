using UnityEngine;
using UnityEngine.UI;

public class ChangeScore : MonoBehaviour
{
    [SerializeField]
    Text HighScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetFloat("HighScore") == null)
        {
            PlayerPrefs.SetFloat("HighScore", 0);
        }

        HighScore.text = "High Score: " + PlayerPrefs.GetFloat("HighScore", 0).ToString("F1") + "s";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
