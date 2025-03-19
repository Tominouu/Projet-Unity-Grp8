using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFXPlayer : MonoBehaviour
{
    [Tooltip("Temps minimum entre chaque effet sonore (en secondes)")]
    [SerializeField] private float minTimeBetweenSFX = 60f; // 1 minute par défaut
    
    [Tooltip("Temps maximum entre chaque effet sonore (en secondes)")]
    [SerializeField] private float maxTimeBetweenSFX = 80f; // Ajoute un peu de variabilité
    
    [Tooltip("Volume des effets sonores")]
    [SerializeField] private float sfxVolume = 0.7f;
    
    [Tooltip("Liste des clips audio à jouer aléatoirement")]
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();
    
    private AudioManager audioManager;
    private float nextSFXTime;
    
    void Start()
    {
        // Récupérer l'instance de l'AudioManager
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trouvé. Assurez-vous qu'il existe dans la scène.");
            enabled = false;
            return;
        }
        
        // Vérifier que nous avons des clips audio
        if (sfxClips.Count == 0)
        {
            Debug.LogWarning("Aucun clip audio n'a été assigné à RandomSFXPlayer.");
            enabled = false;
            return;
        }
        
        // Définir le moment du premier effet sonore
        SetNextSFXTime();
    }
    
    void Update()
    {
        // Vérifier si c'est le moment de jouer un effet sonore
        if (Time.time >= nextSFXTime)
        {
            PlayRandomSFX();
            SetNextSFXTime();
        }
    }
    
    private void PlayRandomSFX()
    {
        // Vérifier que nous avons des clips audio
        if (sfxClips.Count == 0)
            return;
        
        // Sélectionner un clip audio aléatoire
        AudioClip randomClip = sfxClips[Random.Range(0, sfxClips.Count)];
        
        // Jouer le clip audio
        audioManager.PlaySFX(randomClip, sfxVolume);
        
        Debug.Log("Effet sonore aléatoire joué: " + randomClip.name);
    }
    
    private void SetNextSFXTime()
    {
        // Déterminer quand le prochain effet sonore sera joué
        float randomTime = Random.Range(minTimeBetweenSFX, maxTimeBetweenSFX);
        nextSFXTime = Time.time + randomTime;
    }
    
    // Méthode publique pour ajouter des clips audio dynamiquement
    public void AddSFXClip(AudioClip clip)
    {
        if (clip != null && !sfxClips.Contains(clip))
        {
            sfxClips.Add(clip);
        }
    }
    
    // Méthode publique pour définir le temps entre les effets sonores
    public void SetTimeBetweenSFX(float minTime, float maxTime)
    {
        minTimeBetweenSFX = Mathf.Max(1f, minTime); // Au moins 1 seconde
        maxTimeBetweenSFX = Mathf.Max(minTimeBetweenSFX, maxTime);
        
        // Recalculer le prochain temps d'effet sonore
        SetNextSFXTime();
    }
}