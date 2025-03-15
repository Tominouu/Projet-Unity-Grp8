using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeItem : MonoBehaviour
{
    public RectTransform ListeUI;
    public List<GameObject> Traits;
    public string sceneVictoire = "Victoire";

    private bool isPlayerInZone = false;
    private string itemTag;
    
    // Dictionnaire statique pour conserver les traits entre les différentes instances
    private static Dictionary<string, GameObject> traitsDictionary = new Dictionary<string, GameObject>();
    
    // Variable statique pour savoir si le dictionnaire a été réinitialisé pour cette session
    private static bool isDictionaryReset = false;
    
    private bool isFullscreen = false;

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
            if (trait != null && trait.TryGetComponent(out RectTransform rt))
            {
                defaultTraitSizes[trait] = rt.sizeDelta;
            }
            if (trait != null)
            {
                trait.SetActive(false); // Tous les traits sont cachés au début
            }
        }

        // Réinitialiser le dictionnaire au début de chaque nouvelle partie
        if (!isDictionaryReset)
        {
            traitsDictionary.Clear();
            isDictionaryReset = true;
            Debug.Log("Dictionnaire réinitialisé");
        }

        // Reconstruire le dictionnaire avec les objets actuels
        for (int i = 0; i < Traits.Count; i++)
        {
            string tagName = "Item" + (i + 1);
            if (Traits[i] != null)
            {
                traitsDictionary[tagName] = Traits[i];
                Debug.Log("Dictionnaire initialisé avec la clé: " + tagName + " et l'objet: " + Traits[i].name);
            }
            else
            {
                Debug.LogWarning("Trait null à l'index " + i);
            }
        }

        itemTag = gameObject.tag;
        Debug.Log("Tag de l'objet: " + itemTag);
    }

    // Réinitialiser le dictionnaire quand on change de scène
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Réinitialiser le dictionnaire uniquement quand on retourne au Level1
        if (scene.name == "Level1")
        {
            isDictionaryReset = false;
            Debug.Log("Flag de réinitialisation du dictionnaire mis à false");
        }
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
                // Vérifier que l'objet existe avant de l'activer
                if (traitsDictionary[itemTag] != null)
                {
                    traitsDictionary[itemTag].SetActive(true);
                    Debug.Log("Trait pour " + itemTag + " activé.");
                    CheckVictory();
                }
                else
                {
                    Debug.LogError("Le trait pour l'item " + itemTag + " est introuvable (null).");
                }
            }
            else
            {
                Debug.LogWarning("La clé " + itemTag + " n'existe pas dans le dictionnaire.");
            }

            // Désactive l'objet au lieu de le détruire
            gameObject.SetActive(false);
        }

        // Passer en plein écran avec "Q"
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isFullscreen = !isFullscreen;

            if (ListeUI != null)
            {
                if (isFullscreen)
                {
                    // Centrer et agrandir la liste
                    ListeUI.anchorMin = new Vector2(0.5f, 0.5f);
                    ListeUI.anchorMax = new Vector2(0.5f, 0.5f);
                    ListeUI.pivot = new Vector2(0.5f, 0.5f);
                    ListeUI.anchoredPosition = Vector2.zero;
                    ListeUI.sizeDelta = new Vector2(Screen.width * 0.08f, Screen.height * 0.18f);

                    // Agrandir les traits proportionnellement
                    foreach (GameObject trait in Traits)
                    {
                        if (trait != null && trait.TryGetComponent(out RectTransform rt) && defaultTraitSizes.ContainsKey(trait))
                        {
                            rt.sizeDelta = defaultTraitSizes[trait] * 1.5f;
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
                        if (trait != null && trait.TryGetComponent(out RectTransform rt) && defaultTraitSizes.ContainsKey(trait))
                        {
                            rt.sizeDelta = defaultTraitSizes[trait];
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
            // Vérifier que le trait existe avant de l'utiliser
            if (trait != null && !trait.activeSelf) 
                return; // Si un trait est encore caché ou inexistant, on ne fait rien
        }

        // Si on arrive ici, tous les traits sont activés
        Debug.Log("Tous les objets sont ramassés ! Chargement de la scène de victoire...");
        SceneManager.LoadScene(sceneVictoire);
    }
}