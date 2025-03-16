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

    void Start()
    {
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
    }
    
    void SetupBoxListeners()
    {
        // Ajouter des listeners pour les carrés de musique
        for (int i = 0; i < musicLevel.boxes.Length; i++)
        {
            int index = i; // Capturer l'index pour le closure
            Button boxButton = musicLevel.boxes[i].gameObject.AddComponent<Button>();
            boxButton.onClick.AddListener(() => OnMusicBoxClick(index));
            
            // Rendre le bouton transparant
            //ColorBlock colors = boxButton.colors;
            //colors.normalColor = new Color(1, 1, 1, 0);
            //colors.highlightedColor = new Color(1, 1, 1, 0.1f);
            //colors.pressedColor = new Color(1, 1, 1, 0.2f);
            //boxButton.colors = colors;
        }
        
        // Ajouter des listeners pour les carrés d'effets sonores
        for (int i = 0; i < sfxLevel.boxes.Length; i++)
        {
            int index = i; // Capturer l'index pour le closure
            Button boxButton = sfxLevel.boxes[i].gameObject.AddComponent<Button>();
            boxButton.onClick.AddListener(() => OnSFXBoxClick(index));
            
            // Rendre le bouton transparant
            //ColorBlock colors = boxButton.colors;
            //colors.normalColor = new Color(1, 1, 1, 0);
            //colors.highlightedColor = new Color(1, 1, 1, 0.1f);
            //colors.pressedColor = new Color(1, 1, 1, 0.2f);
            //boxButton.colors = colors;
        }
        
        // Ajouter un listener pour le carré de sous-titres
        Button subtitleButton = subtitleBox.gameObject.AddComponent<Button>();
        subtitleButton.onClick.AddListener(ToggleSubtitles);
        
        // Rendre le bouton transparant
        //ColorBlock subtitleColors = subtitleButton.colors;
        //subtitleColors.normalColor = new Color(1, 1, 1, 0);
        //subtitleColors.highlightedColor = new Color(1, 1, 1, 0.1f);
        //subtitleColors.pressedColor = new Color(1, 1, 1, 0.2f);
        //subtitleButton.colors = subtitleColors;
    }
    
    void OnMusicBoxClick(int index)
    {
        // Quand un carré est cliqué, définir la valeur jusqu'à cet index
        for (int i = 0; i < musicLevel.boxes.Length; i++)
        {
            musicLevel.boxes[i].color = (i <= index) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour le volume de la musique
        musicLevel.volume = (index + 1) * 0.25f;
        SetMusicVolume(musicLevel.volume);
    }
    
    void OnSFXBoxClick(int index)
    {
        // Quand un carré est cliqué, définir la valeur jusqu'à cet index
        for (int i = 0; i < sfxLevel.boxes.Length; i++)
        {
            sfxLevel.boxes[i].color = (i <= index) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour le volume des effets sonores
        sfxLevel.volume = (index + 1) * 0.25f;
        SetSFXVolume(sfxLevel.volume);
    }
    
    void ToggleSubtitles()
    {
        subtitlesEnabled = !subtitlesEnabled;
        subtitleBox.color = subtitlesEnabled ? selectedColor : unselectedColor;
        
        // Appliquer le paramètre des sous-titres
        // Vous devrez implémenter cette fonction selon votre système
        SetSubtitlesEnabled(subtitlesEnabled);
    }
    
    void UpdateUI()
    {
        // Mettre à jour l'affichage des carrés de musique
        int musicIndex = Mathf.FloorToInt(musicLevel.volume / 0.25f) - 1;
        for (int i = 0; i < musicLevel.boxes.Length; i++)
        {
            musicLevel.boxes[i].color = (i <= musicIndex) ? selectedColor : unselectedColor;
        }
        
        // Mettre à jour l'affichage des carrés d'effets sonores
        int sfxIndex = Mathf.FloorToInt(sfxLevel.volume / 0.25f) - 1;
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
        musicLevel.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxLevel.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
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
        // Sauvegarder les préférences avant de quitter
        SavePreferences();
        
        // Retourner au menu précédent
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }
    
    // Fonctions pour définir le volume dans votre système audio
    void SetMusicVolume(float volume)
    {
        // Implémenter selon votre système audio
        // Par exemple, si vous avez un AudioManager:
        // AudioManager.Instance.SetMusicVolume(volume);
        Debug.Log("Music Volume set to: " + volume);
    }
    
    void SetSFXVolume(float volume)
    {
        // Implémenter selon votre système audio
        // AudioManager.Instance.SetSFXVolume(volume);
        Debug.Log("SFX Volume set to: " + volume);
    }
    
    void SetSubtitlesEnabled(bool enabled)
    {
        // Implémenter selon votre système de sous-titres
        Debug.Log("Subtitles " + (enabled ? "enabled" : "disabled"));
    }
}