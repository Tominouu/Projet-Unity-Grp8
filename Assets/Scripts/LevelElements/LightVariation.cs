using UnityEngine;
using System.Collections;

public class PlayerLightSmoothBlink : MonoBehaviour
{
    public Light playerLight;  // Lumière attachée au joueur
    public float intensityOn = 1.5f;
    public float intensityOff = 0f;
    public float transitionTime = 2.5f; // Temps pour diminuer/remonter la lumière
    public float blinkInterval = 5f; // Intervalle total

    void Start()
    {
        if (playerLight == null)
            playerLight = GetComponent<Light>(); // Récupère la lumière si elle est sur le même objet

        StartCoroutine(BlinkLight());
    }

    IEnumerator BlinkLight()
    {
        while (true) // Boucle infinie
        {
            yield return StartCoroutine(FadeLight(intensityOn, intensityOff, transitionTime)); // Diminue la lumière
            yield return new WaitForSeconds(blinkInterval - transitionTime); // Pause avant de rallumer
            yield return StartCoroutine(FadeLight(intensityOff, intensityOn, transitionTime)); // Remonte la lumière
        }
    }

    IEnumerator FadeLight(float startIntensity, float targetIntensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            playerLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            yield return null; // Attendre la frame suivante
        }

        playerLight.intensity = targetIntensity; // Assurer la valeur finale
    }
}