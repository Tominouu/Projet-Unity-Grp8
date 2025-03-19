using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Nécessaire pour manipuler l'UI
using UnityEngine.SceneManagement;

public class SortieDidac : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
           SceneManager.LoadScene("SortieDidac");
        }
    }

}