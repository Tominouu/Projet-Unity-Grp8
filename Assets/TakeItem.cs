using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    public List<GameObject> Traits; 
    private bool isPlayerInZone = false;
    private string itemTag; 
    private static Dictionary<string, GameObject> traitsDictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        if (traitsDictionary.Count == 0)
        {
            for (int i = 0; i < Traits.Count; i++)
            {
                string tagName = "Item" + (i + 1); 
                traitsDictionary[tagName] = Traits[i];
                Traits[i].SetActive(false); 
            }
        }

        itemTag = gameObject.tag; 
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
            Debug.Log("Objet ramassé: " + itemTag);

            
            if (traitsDictionary.ContainsKey(itemTag))
            {
                traitsDictionary[itemTag].SetActive(true);
            }

            Destroy(gameObject);
        }
    }
}
