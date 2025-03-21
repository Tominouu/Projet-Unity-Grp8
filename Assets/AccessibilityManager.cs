using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccessibilityManager : MonoBehaviour
{
    // Singleton pour accès facile
    public static AccessibilityManager Instance { get; private set; }

    // Référence au toggle UI
    public Toggle accessibilityToggle;

    // État actuel du mode accessibilité
    public bool AccessibilityModeEnabled { get; private set; }

    void Awake()
    {
        // Configuration du singleton avec persistance entre scènes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // S'abonner à l'événement de changement de scène
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Charger l'état depuis PlayerPrefs dès le Awake
        AccessibilityModeEnabled = PlayerPrefs.GetInt("AccessibilityMode", 0) == 1;
    }

    void Start()
    {
        InitializeToggle();
    }

    // Initialise le toggle et configure les événements
    private void InitializeToggle()
    {
        // Vérifier si le toggle existe
        if (accessibilityToggle == null)
        {
            // Chercher le toggle dans la scène si pas assigné
            accessibilityToggle = FindObjectOfType<Toggle>();

            if (accessibilityToggle == null)
            {
                Debug.LogError("Toggle non assigné au AccessibilityManager!");
                return;
            }
        }

        // Ne pas déclencher l'événement pendant l'initialisation
        accessibilityToggle.SetIsOnWithoutNotify(AccessibilityModeEnabled);

        // Configurer l'écouteur d'événement pour le toggle
        accessibilityToggle.onValueChanged.RemoveAllListeners();
        accessibilityToggle.onValueChanged.AddListener(OnToggleChanged);

        // Notifier tous les objets avec des outlines de l'état actuel
        NotifyOutlineObjects();
    }

    // Appelé lors du chargement d'une nouvelle scène
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attendre une frame pour que tous les objets soient initialisés
        Invoke("ApplyAccessibilitySettings", 0.1f);
    }

    // Applique les paramètres d'accessibilité après changement de scène
    private void ApplyAccessibilitySettings()
    {
        // Réinitialiser le toggle si nécessaire
        if (accessibilityToggle == null)
        {
            accessibilityToggle = FindObjectOfType<Toggle>();
            if (accessibilityToggle != null)
            {
                InitializeToggle();
            }
        }

        // Appliquer les paramètres aux outlines de la nouvelle scène
        NotifyOutlineObjects();
    }

    // Appelé lorsque le toggle change d'état
    public void OnToggleChanged(bool isOn)
    {
        Debug.Log("Mode accessibilité: " + (isOn ? "activé" : "désactivé"));

        // Mettre à jour l'état
        AccessibilityModeEnabled = isOn;

        // Sauvegarder l'état
        PlayerPrefs.SetInt("AccessibilityMode", isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Notifier tous les objets avec des outlines
        NotifyOutlineObjects();
    }

    // Notifie tous les objets qui ont un composant Outline
    private void NotifyOutlineObjects()
    {
        // Trouver tous les composants Outline dans la scène
        Outline[] outlines = FindObjectsOfType<Outline>();

        foreach (Outline outline in outlines)
        {
            // Activer ou désactiver l'outline selon l'état actuel
            outline.enabled = AccessibilityModeEnabled;
        }
    }

    // Assurer la sauvegarde lors de la fermeture de l'application
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
