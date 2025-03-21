using UnityEngine;

public class Curseur : MonoBehaviour
{
    [Header("Paramètres du Curseur")]
    [Tooltip("Texture du curseur personnalisé")]
    public Texture2D cursorTexture;
    
    [Tooltip("Point d'ancrage du curseur (0,0 = coin supérieur gauche)")]
    public Vector2 hotSpot = Vector2.zero;
    
    [Tooltip("Mode du curseur")]
    public CursorMode cursorMode = CursorMode.Auto;
    
    [Header("Options avancées")]
    [Tooltip("Conserver ce GameObject entre les scènes")]
    public bool dontDestroyOnLoad = true;
    
    [Tooltip("Réappliquer le curseur quand la fenêtre gagne le focus")]
    public bool reapplyOnFocus = true;

    private void Awake()
    {
        // Conserver ce GameObject lors des changements de scène si demandé
        if (dontDestroyOnLoad)
        {
            // Vérifier s'il existe déjà une instance de Curseur
            Curseur[] curseurs = FindObjectsOfType<Curseur>();
            if (curseurs.Length > 1)
            {
                // Si une instance existe déjà, détruire celle-ci
                Destroy(gameObject);
                return;
            }
            
            // Conserver cette instance entre les scènes
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Appliquer le curseur personnalisé au démarrage
        AppliquerCurseur();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // Réappliquer le curseur quand l'application récupère le focus
        if (hasFocus && reapplyOnFocus)
        {
            AppliquerCurseur();
        }
    }

    private void AppliquerCurseur()
    {
        // Vérifier que la texture du curseur est bien assignée
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        else
        {
            Debug.LogWarning("Curseur personnalisé : Aucune texture de curseur n'est assignée!");
        }
    }

    // Méthode publique pour changer de curseur en cours de jeu si nécessaire
    public void ChangerCurseur(Texture2D nouvelleTex, Vector2 nouveauHotSpot)
    {
        cursorTexture = nouvelleTex;
        hotSpot = nouveauHotSpot;
        AppliquerCurseur();
    }

    // Pour revenir au curseur par défaut si nécessaire
    public void ResetCurseur()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}