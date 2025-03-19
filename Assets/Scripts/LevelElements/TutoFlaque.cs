using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler l'UI

public class tutoflaque : MonoBehaviour
{
    public Text hudMessageText; // Référence au composant Text de l'UI
    private string message = "Sprintez pour sortir de la flaque"; 

    void Start()
    {
        if (hudMessageText != null)
        {
            hudMessageText.text = ""; // Assurez-vous que le texte est vide au départ
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur
        {
            if (hudMessageText != null)
            {
                hudMessageText.text = message ;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur
        {
            if (hudMessageText != null)
            {
                hudMessageText.text = ""; // Efface le message
            }
        }
    }
}