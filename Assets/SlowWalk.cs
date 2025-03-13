using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowWalk : MonoBehaviour
{
    public float slowSpeed = 2f;  // Vitesse réduite
    private float normalSpeed;    // Vitesse normale du joueur
    private PlayerController playerMovement; // Référence au script de mouvement

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur
        {
            playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                normalSpeed = playerMovement.speed; // Sauvegarde la vitesse normale
                playerMovement.speed = slowSpeed;  // Ralentit le joueur
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si le joueur quitte la zone
        {
            if (playerMovement != null)
            {
                playerMovement.speed = normalSpeed; // Rétablit la vitesse
            }
        }
    }
}
