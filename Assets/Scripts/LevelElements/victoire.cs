using UnityEngine;
using UnityEngine.UI;

public class VictoryTimeDisplay : MonoBehaviour
{
    public Text timeDisplayText;

    void Start()
    {
        if (timeDisplayText != null)
        {
            // Récupérer le temps restant depuis la variable statique du Timer
            float remainingTime = Timer.finalTimeRemaining;

            // Convertir en minutes et secondes
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Calculer le temps utilisé (3 minutes moins le temps restant)
            float timeUsed = 180f - remainingTime;
            int minutesUsed = Mathf.FloorToInt(timeUsed / 60);
            int secondsUsed = Mathf.FloorToInt(timeUsed % 60);

            // Afficher le temps restant et le temps utilisé
            timeDisplayText.text = string.Format("Temps : {0:00}:{1:00}",
                                                 minutes, seconds, minutesUsed, secondsUsed);

            Debug.Log("Affichage du temps restant: " + remainingTime);
        }
        else
        {
            Debug.LogError("Le composant Text pour l'affichage du temps n'est pas assigné!");
        }
    }
}
