using UnityEngine;
using System.Collections;

public class PlayerLightBlink : MonoBehaviour
{
    public Light playerLight;  // Lumière attachée au joueur
    public float intensityOn = 1.5f;
    public float intensityOff = 0f;
    public float blinkInterval = 5f;

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
            playerLight.intensity = intensityOff; // Éteindre la lumière
            yield return new WaitForSeconds(blinkInterval / 2); // Attendre 2.5 sec
            playerLight.intensity = intensityOn; // Rallumer la lumière
            yield return new WaitForSeconds(blinkInterval / 2); // Attendre 2.5 sec
        }
    }
}