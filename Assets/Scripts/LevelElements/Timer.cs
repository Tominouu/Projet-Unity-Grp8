using UnityEngine;
using UnityEngine.UI; 

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60f; 
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
            Debug.Log("Temps écoulé !");
        }
    }
}
