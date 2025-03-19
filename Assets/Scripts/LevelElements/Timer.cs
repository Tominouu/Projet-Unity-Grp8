using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public Text timerText;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // Convert remaining time to minutes and seconds
            int minutes = Mathf.Max(0,Mathf.FloorToInt(timeRemaining / 60));
            int seconds = Mathf.Max(0,Mathf.FloorToInt(timeRemaining % 60));

            // Display in format: minutes:seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            /*timerText.text = "00:00";
            SceneManager.LoadScene("GameOver");*/
        }
    }
}
