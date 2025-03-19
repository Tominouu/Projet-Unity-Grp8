using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler l'UI

public class tutopickup : MonoBehaviour
{
    public Text messageText; // Référence au composant Text de l'UI
    private string message = "Vous êtes dans la zone !";

    void Start()
    {
        if (messageText != null)
        {
            messageText.text = ""; // Assurez-vous que le texte est vide au départ
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur
        {
            if (messageText != null)
            {
                messageText.text = message; // Affiche le message
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur
        {
            if (messageText != null)
            {
                messageText.text = ""; // Efface le message
            }
        }
    }
}
