using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public SFXList sfx_list = new SFXList();
	[System.Serializable]
	public class SFXList{
    	[SerializeField] public AudioClip sfx_key;
		[SerializeField] public AudioClip sfx_lock;
		[SerializeField] public AudioClip sfx_heal;
		[SerializeField] public AudioClip sfx_end;
		[SerializeField] public AudioClip sfx_hit;
	}

	public MusicList music_list = new MusicList();
	[System.Serializable]
	public class MusicList{
    	[SerializeField] public AudioClip music1;
	}

	public static AudioManager instance = null;
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;
	
	void Awake(){
		if (instance == null){
			instance = this;
		}
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
}

