using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingManager : MonoBehaviour
{
    // Singleton pour un accès facile
    public static KeyBindingManager Instance { get; private set; }
    
    // Enum pour les actions possibles
    public enum GameAction
    {
        Haut,
        Bas,
        Gauche,
        Droite,
        Ouvrir,
        Prendre
    }
    
    // Références UI
    [Header("Références UI")]
    public Button hautButton;
    public Button basButton;
    public Button gaucheButton;
    public Button droiteButton;
    public Button ouvrirButton;
    public Button prendreButton;
    public Button quitterButton;
    
    public TextMeshProUGUI hautText;
    public TextMeshProUGUI basText;
    public TextMeshProUGUI gaucheText;
    public TextMeshProUGUI droiteText;
    public TextMeshProUGUI ouvrirText;
    public TextMeshProUGUI prendreText;
    
    // Canvas du menu des commandes
    public GameObject commandesMenuCanvas;
    
    // Dictionnaire stockant les touches
    private Dictionary<GameAction, KeyCode> keyBindings = new Dictionary<GameAction, KeyCode>();
    
    // Variable indiquant l'action en cours de remappage
    private GameAction? currentlyRebinding = null;
    
    // Couleurs pour le feedback visuel
    private Color defaultColor;
    private Color rebindingColor = new Color(1f, 0.5f, 0.5f, 1f);
    
    private void Awake()
{
    // Mise en place du singleton
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
    
    // Détecter automatiquement la disposition du clavier
    bool isAzertyKeyboard = IsAzertyKeyboard();
    
    // Initialisation des touches par défaut selon la disposition
    if (isAzertyKeyboard)
    {
        // Configuration AZERTY française
        keyBindings.Add(GameAction.Haut, KeyCode.Z);
        keyBindings.Add(GameAction.Bas, KeyCode.S);
        keyBindings.Add(GameAction.Gauche, KeyCode.Q);
        keyBindings.Add(GameAction.Droite, KeyCode.D);
    }
    else
    {
        // Configuration QWERTY par défaut
        keyBindings.Add(GameAction.Haut, KeyCode.W);
        keyBindings.Add(GameAction.Bas, KeyCode.S);
        keyBindings.Add(GameAction.Gauche, KeyCode.A);
        keyBindings.Add(GameAction.Droite, KeyCode.D);
    }
    
    // Ces touches sont généralement les mêmes quelle que soit la disposition
    keyBindings.Add(GameAction.Ouvrir, KeyCode.F);
    keyBindings.Add(GameAction.Prendre, KeyCode.E);
    
    // Chargement des touches sauvegardées
    LoadKeyBindings();
}

    // Méthode pour tenter de détecter si le clavier est AZERTY
    private bool IsAzertyKeyboard()
    {
        // Vérifier la langue du système
        string systemLanguage = Application.systemLanguage.ToString();
        return systemLanguage == "French" || systemLanguage == "Belgian" || systemLanguage == "Swiss";
    }
    
    private void Start()
    {
        // Mémoriser la couleur par défaut des boutons
        if (hautButton != null)
            defaultColor = hautButton.GetComponent<Image>().color;
        
        // Configuration des listeners de boutons
        SetupButtonListeners();
        
        // Mise à jour de l'affichage des touches
        UpdateAllKeyTexts();
        
        // Désactiver le menu au démarrage
        if (commandesMenuCanvas != null)
            commandesMenuCanvas.SetActive(false);
    }
    
    private void SetupButtonListeners()
    {
        if (hautButton != null)
            hautButton.onClick.AddListener(() => StartRebinding(GameAction.Haut));
        if (basButton != null)
            basButton.onClick.AddListener(() => StartRebinding(GameAction.Bas));
        if (gaucheButton != null)
            gaucheButton.onClick.AddListener(() => StartRebinding(GameAction.Gauche));
        if (droiteButton != null)
            droiteButton.onClick.AddListener(() => StartRebinding(GameAction.Droite));
        if (ouvrirButton != null)
            ouvrirButton.onClick.AddListener(() => StartRebinding(GameAction.Ouvrir));
        if (prendreButton != null)
            prendreButton.onClick.AddListener(() => StartRebinding(GameAction.Prendre));
        if (quitterButton != null)
            quitterButton.onClick.AddListener(() => ToggleCommandesMenu(false));
    }
    
    private void Update()
    {
        // Si on est en mode réassignation de touche
        if (currentlyRebinding.HasValue)
        {
            // Vérifier chaque touche possible
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Vérifier si cette touche est déjà utilisée
                    GameAction? conflictAction = CheckForBindingConflict(keyCode);
                    
                    if (conflictAction.HasValue && conflictAction.Value != currentlyRebinding.Value)
                    {
                        // Échange des touches en cas de conflit
                        KeyCode oldKey = keyBindings[currentlyRebinding.Value];
                        keyBindings[conflictAction.Value] = oldKey;
                        UpdateKeyText(conflictAction.Value);
                    }
                    
                    // Assigner la nouvelle touche
                    keyBindings[currentlyRebinding.Value] = keyCode;
                    
                    // Mettre à jour l'affichage
                    UpdateKeyText(currentlyRebinding.Value);
                    
                    // Sortir du mode réassignation
                    StopRebinding();
                    
                    // Sauvegarder les changements
                    SaveKeyBindings();
                    
                    break;
                }
            }
            
            // Si l'utilisateur clique ailleurs, annuler la réassignation
            if (Input.GetMouseButtonDown(0))
            {
                StopRebinding();
            }
        }
    }
    
    private GameAction? CheckForBindingConflict(KeyCode key)
    {
        foreach (var binding in keyBindings)
        {
            if (binding.Value == key)
            {
                return binding.Key;
            }
        }
        return null;
    }
    
    private void StartRebinding(GameAction action)
    {
        // Si on était déjà en train de réassigner une touche, on annule
        if (currentlyRebinding.HasValue)
        {
            StopRebinding();
        }
        
        // Démarrer la réassignation pour cette action
        currentlyRebinding = action;
        
        // Changer la couleur du bouton pour indiquer qu'il est en cours de réassignation
        Button button = GetButtonForAction(action);
        if (button != null)
            button.GetComponent<Image>().color = rebindingColor;
        
        // Changer le texte pour indiquer d'appuyer sur une touche
        TextMeshProUGUI text = GetTextForAction(action);
        if (text != null)
            text.text = "...";
    }
    
    private void StopRebinding()
    {
        // Si on était en train de réassigner une touche
        if (currentlyRebinding.HasValue)
        {
            // Restaurer la couleur par défaut
            Button button = GetButtonForAction(currentlyRebinding.Value);
            if (button != null)
                button.GetComponent<Image>().color = defaultColor;
            
            // Mettre à jour le texte
            UpdateKeyText(currentlyRebinding.Value);
            
            // Sortir du mode réassignation
            currentlyRebinding = null;
        }
    }
    
    private void UpdateAllKeyTexts()
    {
        foreach (GameAction action in System.Enum.GetValues(typeof(GameAction)))
        {
            if (keyBindings.ContainsKey(action))
            {
                UpdateKeyText(action);
            }
        }
    }
    
    private void UpdateKeyText(GameAction action)
    {
        // Obtenir le texte correspondant à l'action
        TextMeshProUGUI text = GetTextForAction(action);
        if (text != null && keyBindings.ContainsKey(action))
        {
            // Mettre à jour le texte avec la touche actuelle
            text.text = keyBindings[action].ToString();
        }
    }
    
    // Obtenir le bouton correspondant à une action
    private Button GetButtonForAction(GameAction action)
    {
        switch (action)
        {
            case GameAction.Haut: return hautButton;
            case GameAction.Bas: return basButton;
            case GameAction.Gauche: return gaucheButton;
            case GameAction.Droite: return droiteButton;
            case GameAction.Ouvrir: return ouvrirButton;
            case GameAction.Prendre: return prendreButton;
            default: return null;
        }
    }
    
    // Obtenir le texte correspondant à une action
    private TextMeshProUGUI GetTextForAction(GameAction action)
    {
        switch (action)
        {
            case GameAction.Haut: return hautText;
            case GameAction.Bas: return basText;
            case GameAction.Gauche: return gaucheText;
            case GameAction.Droite: return droiteText;
            case GameAction.Ouvrir: return ouvrirText;
            case GameAction.Prendre: return prendreText;
            default: return null;
        }
    }
    
    // Fonction pour sauvegarder les configurations de touches
    public void SaveKeyBindings()
    {
        foreach (var binding in keyBindings)
        {
            PlayerPrefs.SetInt("KeyBinding_" + binding.Key.ToString(), (int)binding.Value);
        }
        PlayerPrefs.Save();
        Debug.Log("Touches sauvegardées");
    }
    
    // Fonction pour charger les configurations de touches
    public void LoadKeyBindings()
    {
        foreach (GameAction action in System.Enum.GetValues(typeof(GameAction)))
        {
            if (PlayerPrefs.HasKey("KeyBinding_" + action.ToString()))
            {
                keyBindings[action] = (KeyCode)PlayerPrefs.GetInt("KeyBinding_" + action.ToString());
            }
        }
        UpdateAllKeyTexts();
    }
    
    // Fonction pour rétablir les touches par défaut
    public void ResetToDefaults()
{
    bool isAzertyKeyboard = IsAzertyKeyboard();
    
    if (isAzertyKeyboard)
    {
        keyBindings[GameAction.Haut] = KeyCode.Z;
        keyBindings[GameAction.Bas] = KeyCode.S;
        keyBindings[GameAction.Gauche] = KeyCode.Q;
        keyBindings[GameAction.Droite] = KeyCode.D;
    }
    else
    {
        keyBindings[GameAction.Haut] = KeyCode.W;
        keyBindings[GameAction.Bas] = KeyCode.S;
        keyBindings[GameAction.Gauche] = KeyCode.A;
        keyBindings[GameAction.Droite] = KeyCode.D;
    }
    
    keyBindings[GameAction.Ouvrir] = KeyCode.F;
    keyBindings[GameAction.Prendre] = KeyCode.E;
    
    UpdateAllKeyTexts();
    SaveKeyBindings();
}
    
    // Vérifier si une action est actuellement pressée
    public bool IsActionPressed(GameAction action)
    {
        if (keyBindings.ContainsKey(action))
            return Input.GetKey(keyBindings[action]);
        return false;
    }
    
    // Vérifier si une action vient d'être pressée ce frame
    public bool IsActionDown(GameAction action)
    {
        if (keyBindings.ContainsKey(action))
            return Input.GetKeyDown(keyBindings[action]);
        return false;
    }
    
    // Vérifier si une action vient d'être relâchée ce frame
    public bool IsActionUp(GameAction action)
    {
        if (keyBindings.ContainsKey(action))
            return Input.GetKeyUp(keyBindings[action]);
        return false;
    }
    
    // Obtenir la touche associée à une action
    public KeyCode GetKeyForAction(GameAction action)
    {
        if (keyBindings.ContainsKey(action))
            return keyBindings[action];
        return KeyCode.None;
    }
    
    // Afficher/Masquer le menu des commandes
    public void ToggleCommandesMenu(bool show)
    {
        if (commandesMenuCanvas != null)
        {
            commandesMenuCanvas.SetActive(show);
            
            // Si on affiche le menu, mettre à jour les textes
            if (show)
            {
                UpdateAllKeyTexts();
            }
        }
    }

}