using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsClickManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioLevel
    {
        public Image[] boxes;
        public float volume = 0f;
    }

    public AudioLevel musicLevel;
    public AudioLevel sfxLevel;
    public Image subtitleBox;
    public bool subtitlesEnabled = false;
    
    public Color selectedColor = Color.black;
    public Color unselectedColor = Color.white;
    
    public Button backButton;
    
    // Référence vers l'AudioManager
    private AudioManager audioManager;

    void Start()
    {
        // Obtenir une référence à l'AudioManager
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trouvé!");
        }
        
        // Charger les préférences sauvegardées si disponibles
        LoadPreferences();
        
        // Ajouter des listeners pour les clics sur les carrés
        SetupBoxListeners();
        
        // Configurer le bouton retour
        if (backButton != null)
        {
            backButton.onClick.AddListener(ReturnToPreviousMenu);
        }
        
        // Mettre à jour l'UI
        UpdateUI();
        
        // Appliquer les valeurs au démarrage
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(musicLevel.volume);
            audioManager.SetSFXVolume(sfxLevel.volume);
        }
    }
    
    void SetupBoxListeners()
    {
        // Ajouter des listeners pour les carrés de musique
        for (int i = 0; i < musicLevel.boxes.Length; i++)
        {
            int index = i; // Capturer l'index pour le closure
            Button boxButton = musicLevel.boxes[i].gameObject.AddComponent<Button>();
            boxButton.onClick.AddListener(() => OnMusicBoxClick(index));
            
            // Rendre le bouton transparent
            ColorBlock colors = boxButton.colors;
            colors.normalColor = new Color(1, 1, 1, 0);
            colors.highlightedColor = new Color(1, 1, 1, 0.1f);
            colors.pressedColor = new Color(1, 1, 1, 0.2f);
            boxButton.colors = colors;
        }
        
        // Ajouter des listeners pour les carrés d'effets sonores
        for (int i = 0; i < sfxLevel.boxes.Length; i++)
        {
            int index = i; // Capturer l'index pour le closure
            Button boxButton = sfxLevel.boxes[i].gameObject.AddComponent<Button>();
            boxButton.onClick.AddListener(() => OnSFXBoxClick(index));
            
            // Rendre le bouton transparent
            ColorBlock colors = boxButton.colors;
            colors.normalColor = new Color(1, 1, 1, 0);
            colors.highlightedColor = new Color(1, 1, 1, 0.1f);
            colors.pressedColor = new Color(1, 1, 1, 0.2f);
            boxButton.colors = colors;
        }
        
        // Ajouter un listener pour le carré de sous-titres
        Button subtitleButton = subtitleBox.gameObject.AddComponent<Button>();
        subtitleButton.onClick.AddListener(ToggleSubtitles);
        
        // Rendre le bouton transparent
        ColorBlock subtitleColors = subtitleButton.colors;
        subtitleColors.normalColor = new Color(1, 1, 1, 0);
        subtitleColors.highlightedColor = new Color(1, 1, 1, 0.1f);
        subtitleColors.pressedColor = new Color(1, 1, 1, 0.2f);
        subtitleButton.colors = subtitleColors;
    }
    
    void OnMusicBoxClick(int index)
    {
        // Quand un carré est cliqué, définir la valeur jusqu'à cet index
        for (int i = 0; i < musicLevel.boxes.Length; i++)
        {
            musicLevel.boxes[i].color = (i <= index) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour le volume de la musique (0 à 1)
        // index 0 = 0%, index 4 = 100%
        musicLevel.volume = (float)(index + 1) / 5;
        
        // Jouer un son pour indiquer le changement (en utilisant le volume actuel des SFX)
        if (audioManager != null && audioManager.sfx_list.sfx_key != null)
        {
            audioManager.PlaySFX(audioManager.sfx_list.sfx_key);
        }
        
        SetMusicVolume(musicLevel.volume);
    }
    
    void OnSFXBoxClick(int index)
    {
        // Quand un carré est cliqué, définir la valeur jusqu'à cet index
        for (int i = 0; i < sfxLevel.boxes.Length; i++)
        {
            sfxLevel.boxes[i].color = (i <= index) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour le volume des effets sonores (0 à 1)
        sfxLevel.volume = (float)(index + 1) / 5;
        
        // Jouer un son pour indiquer le changement (en utilisant le nouveau volume des SFX)
        if (audioManager != null && audioManager.sfx_list.sfx_key != null)
        {
            // Appliquer le volume avant de jouer le son
            SetSFXVolume(sfxLevel.volume);
            audioManager.PlaySFX(audioManager.sfx_list.sfx_key);
        }
    }
    
    void ToggleSubtitles()
    {
        subtitlesEnabled = !subtitlesEnabled;
        subtitleBox.color = subtitlesEnabled ? selectedColor : unselectedColor;
        
        // Jouer un son pour indiquer le changement
        if (audioManager != null && audioManager.sfx_list.sfx_key != null)
        {
            audioManager.PlaySFX(audioManager.sfx_list.sfx_key);
        }
        
        // Appliquer le paramètre des sous-titres
        SetSubtitlesEnabled(subtitlesEnabled);
    }
    
    void UpdateUI()
    {
        // Mettre à jour l'affichage des carrés de musique
        int musicIndex = Mathf.RoundToInt(musicLevel.volume * 5) - 1;
        for (int i = 0; i < musicLevel.boxes.Length; i++)
        {
            musicLevel.boxes[i].color = (i <= musicIndex) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour l'affichage des carrés d'effets sonores
        int sfxIndex = Mathf.RoundToInt(sfxLevel.volume * 5) - 1;
        for (int i = 0; i < sfxLevel.boxes.Length; i++)
        {
            sfxLevel.boxes[i].color = (i <= sfxIndex) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour l'affichage de la case des sous-titres
        subtitleBox.color = subtitlesEnabled ? selectedColor : unselectedColor;
    }
    
    void LoadPreferences()
    {
        // Charger les préférences sauvegardées
        musicLevel.volume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        sfxLevel.volume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        subtitlesEnabled = PlayerPrefs.GetInt("SubtitlesEnabled", 0) == 1;
    }
    
    void SavePreferences()
    {
        // Sauvegarder les préférences
        PlayerPrefs.SetFloat("MusicVolume", musicLevel.volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxLevel.volume);
        PlayerPrefs.SetInt("SubtitlesEnabled", subtitlesEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    void ReturnToPreviousMenu()
    {
        // Jouer un son pour indiquer le clic sur le bouton retour
        if (audioManager != null && audioManager.sfx_list.sfx_key != null)
        {
            audioManager.PlaySFX(audioManager.sfx_list.sfx_key);
        }
        
        // Sauvegarder les préférences avant de quitter
        SavePreferences();
        
        // Retourner au menu précédent
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    // Fonctions pour définir le volume en utilisant l'AudioManager
    void SetMusicVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
            Debug.Log("Music Volume set to: " + volume);
        }
    }
    
    void SetSFXVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(volume);
            Debug.Log("SFX Volume set to: " + volume);
        }
    }
    
    void SetSubtitlesEnabled(bool enabled)
    {
        // Stocker le paramètre pour votre système de sous-titres
        // À implémenter selon votre système
        Debug.Log("Subtitles " + (enabled ? "enabled" : "disabled"));
    }
}