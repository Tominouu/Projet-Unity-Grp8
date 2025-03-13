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
            timerText.text = Mathf.Ceil(timeRemaining).ToString(); 
        }
        else
        {
            timerText.text = "0"; 
            SceneManager.LoadScene("GameOver");
        }
    }
}
