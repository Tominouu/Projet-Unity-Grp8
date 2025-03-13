using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
public GameObject List; // L'élément UI à afficher
private bool isPlayerInZone = false;

    private void Start()
    {
        // Masquer l'élément au démarrage
        if (List != null)
            List.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Zone touchée");
            Destroy(gameObject);
            List.SetActive(true);
        }
    }

}
