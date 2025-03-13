using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
private bool isPlayerInZone = false;

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
        }
    }

}
