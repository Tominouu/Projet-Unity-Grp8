using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import pour changer de scène

public class TakeItem : MonoBehaviour
{
    public RectTransform ListeUI; // Panel UI contenant la liste et les traits
    public List<GameObject> Traits; // Liste des traits associés aux items
    public string sceneVictoire = "Victoire"; // Nom de la scène de victoire

    private bool isPlayerInZone = false;
    private string itemTag; // Tag de l'objet en cours
    private static Dictionary<string, GameObject> traitsDictionary = new Dictionary<string, GameObject>();
    private bool isFullscreen = false; // État de l'affichage

    private Vector2 defaultPosition;
    private Vector2 defaultSize;
    private Vector2 defaultAnchorMin;
    private Vector2 defaultAnchorMax;
    private Vector2 defaultPivot;
    private Dictionary<GameObject, Vector2> defaultTraitSizes = new Dictionary<GameObject, Vector2>();

    private void Start()
    {
        // Sauvegarde des paramètres initiaux de la liste UI
        if (ListeUI != null)
        {
            defaultPosition = ListeUI.anchoredPosition;
            defaultSize = ListeUI.sizeDelta;
            defaultAnchorMin = ListeUI.anchorMin;
            defaultAnchorMax = ListeUI.anchorMax;
            defaultPivot = ListeUI.pivot;
        }

        // Sauvegarde de la taille initiale des traits
        foreach (GameObject trait in Traits)
        {
            if (trait.TryGetComponent(out RectTransform rt))
            {
                defaultTraitSizes[trait] = rt.sizeDelta;
            }
            trait.SetActive(false); // Tous les traits sont cachés au début
        }

        // Initialiser le dictionnaire au démarrage
        if (traitsDictionary.Count == 0)
        {
            for (int i = 0; i < Traits.Count; i++)
            {
                string tagName = "Item" + (i + 1); // Exemple : "Item1", "Item2", ...
                traitsDictionary[tagName] = Traits[i];
            }
        }

        itemTag = gameObject.tag; // Récupérer le tag de l'objet
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
        // Récupérer un objet avec la touche "E"
        if (isPlayerInZone && Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Objet ramassé: " + itemTag);

            // Vérifier si un trait correspond à cet item
            if (traitsDictionary.ContainsKey(itemTag))
            {
                traitsDictionary[itemTag].SetActive(true);
                CheckVictory(); // Vérifier si tous les traits sont activés
            }

            Destroy(gameObject);
        }

        // Passer en plein écran avec "Q"
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isFullscreen = !isFullscreen; // Inverser l'état

            if (ListeUI != null)
            {
                if (isFullscreen)
                {
                    // Centrer et agrandir la liste (60% de la largeur, 70% de la hauteur)
                    ListeUI.anchorMin = new Vector2(0.5f, 0.5f);
                    ListeUI.anchorMax = new Vector2(0.5f, 0.5f);
                    ListeUI.pivot = new Vector2(0.5f, 0.5f);
                    ListeUI.anchoredPosition = Vector2.zero; // Centre exact
                    ListeUI.sizeDelta = new Vector2(Screen.width * 0.08f, Screen.height * 0.18f); // Taille raisonnable

                    // Agrandir les traits proportionnellement
                    foreach (GameObject trait in Traits)
                    {
                        if (trait.TryGetComponent(out RectTransform rt))
                        {
                            rt.sizeDelta = defaultTraitSizes[trait] * 1.5f; // Augmenter la taille de 50%
                        }
                    }
                }
                else
                {
                    // Rétablir la position et taille de base
                    ListeUI.anchorMin = defaultAnchorMin;
                    ListeUI.anchorMax = defaultAnchorMax;
                    ListeUI.pivot = defaultPivot;
                    ListeUI.anchoredPosition = defaultPosition;
                    ListeUI.sizeDelta = defaultSize;

                    // Rétablir la taille des traits
                    foreach (GameObject trait in Traits)
                    {
                        if (trait.TryGetComponent(out RectTransform rt))
                        {
                            rt.sizeDelta = defaultTraitSizes[trait]; // Retour à la taille normale
                        }
                    }
                }
            }
        }
    }

    // Vérifie si tous les traits sont affichés, et si oui, bascule vers la scène de victoire
    void CheckVictory()
    {
        foreach (GameObject trait in Traits)
        {
            if (!trait.activeSelf) return; // Si un trait est encore caché, on ne fait rien
        }

        // Si on arrive ici, tous les traits sont activés
        Debug.Log("Tous les objets sont ramassés ! Chargement de la scène de victoire...");
        SceneManager.LoadScene(sceneVictoire);
    }
}
