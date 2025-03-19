using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SFXList sfx_list = new SFXList();
    
    [System.Serializable]
    public class SFXList{
        [SerializeField] public AudioClip ambiance_aération;
        [SerializeField] public AudioClip jingle_annonce;
        [SerializeField] public AudioClip jingle_fermeture;
        [SerializeField] public AudioClip voix_excellente;
        [SerializeField] public AudioClip voix_léo;
        [SerializeField] public AudioClip voix_promotion;
        [SerializeField] public AudioClip voix_réduction;
        [SerializeField] public AudioClip voix_responsable;
        [SerializeField] public AudioClip voix_sac;
        [SerializeField] public AudioClip musique;
    }

    public MusicList music_list = new MusicList();

    [System.Serializable]
    public class MusicList{
        [SerializeField] public AudioClip musique;
    }

    public static AudioManager instance = null;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject); // Ceci permet à l'AudioManager de persister entre les scènes
    }
    else
    {
        Destroy(gameObject); // Détruire les copies supplémentaires
    }
}

    void Start()
    {
        LoadAudioSettings();
    }

    public void LoadAudioSettings()
    {
        // Charger les préférences audio sauvegardées
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        
        // Appliquer les volumes
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        
        Debug.Log("Audio settings loaded: Music=" + musicVolume + ", SFX=" + sfxVolume);
    }
    public void PlaySFX(AudioClip sfx, float volume = 0.7f){
        if (sfx != null)
        {
            sfxSource.volume = volume;
            sfxSource.PlayOneShot(sfx);
        }
        else
        {
            Debug.LogWarning("SFX non trouvé");
        }
    }

    public void PlayMusic(AudioClip music, float volume = 0.5f, bool loop = true)
    {
        if (music != null)
        {
            if (musicSource.clip == music && musicSource.isPlaying)
            {
                //Debug.Log("same music");
            }
            else
            {
                musicSource.volume = volume;
                musicSource.clip = music;
                musicSource.loop = loop;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Musique non trouvée: " + name);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Ajout des nouvelles méthodes pour définir les volumes
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
