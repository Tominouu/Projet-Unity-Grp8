using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowWalk : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Facteur de ralentissement appliqué au joueur (valeurs inférieures à 1 ralentissent)")]
    [Range(0.1f, 10f)]
    public float slowFactor = 0.5f;
    
    [Tooltip("Vitesse normale du joueur, restaurée quand il quitte la zone")]
    public float normalSpeed = 5f;
    
    // Le joueur n'a pas besoin d'avoir un tag spécifique
    // On utilise plutôt une vérification de composant
    
    private void OnCollisionEnter(Collision collision)
    {
        // Vérifier si l'objet qui entre en collision a un PlayerController
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        
        // Si c'est le joueur (possède un PlayerController), ralentir sa vitesse
        if (playerController != null)
        {
            playerController.speed *= slowFactor;
            Debug.Log("Joueur ralenti: " + playerController.speed);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        // Vérifier si l'objet qui quitte la collision a un PlayerController
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        
        // Restaurer la vitesse normale du joueur
        if (playerController != null)
        {
            playerController.speed = normalSpeed;
            Debug.Log("Vitesse normale restaurée: " + playerController.speed);
        }
    }
}
