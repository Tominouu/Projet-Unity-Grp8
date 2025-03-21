using UnityEngine;
using UnityEngine.UI;

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
        // Configuration du singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Vérifier si le toggle existe
        if (accessibilityToggle == null)
        {
            Debug.LogError("Toggle non assigné au AccessibilityManager!");
            return;
        }

        // Charger l'état précédent
        AccessibilityModeEnabled = PlayerPrefs.GetInt("AccessibilityMode", 0) == 1;

        // Ne pas déclencher l'événement pendant l'initialisation
        accessibilityToggle.SetIsOnWithoutNotify(AccessibilityModeEnabled);

        // Configurer l'écouteur d'événement pour le toggle
        accessibilityToggle.onValueChanged.RemoveAllListeners();
        accessibilityToggle.onValueChanged.AddListener(OnToggleChanged);

        // Notifier tous les objets avec des outlines de l'état actuel
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
}
