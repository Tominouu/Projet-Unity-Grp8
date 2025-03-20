using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 20f; // 3 minutes en secondes
    public Text timerText;

    // Variable statique pour stocker le temps restant pour l'écran de victoire
    public static float finalTimeRemaining = 0f;
    private bool isTiming = true;

    void Start()
    {
        // Initialiser le temps restant à 3 minutes
        timeRemaining = 20f;
    }

    void Update()
    {
        if (isTiming)
        {
            if (timeRemaining > 0)
            {
                // Décrémenter le temps restant
                timeRemaining -= Time.deltaTime;

                // S'assurer que le temps ne descend pas en dessous de zéro
                timeRemaining = Mathf.Max(0, timeRemaining);

                // Convertir le temps en minutes et secondes
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);

                // Afficher au format: minutes:secondes
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                // Si le temps est écoulé, vous pouvez ajouter une logique ici
                // Par exemple, game over ou pénalité
                isTiming = false;
            }
        }
    }

    // Méthode pour arrêter le timer
    public void StopTimer()
    {
        isTiming = false;
        finalTimeRemaining = timeRemaining;
        Debug.Log("Timer arrêté avec " + finalTimeRemaining + " secondes restantes");
    }
}
