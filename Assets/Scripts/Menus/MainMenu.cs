using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel_options;
	
	void Start(){
		//UnshowOptions();
	}
	
	public void PlayGame(){
		SceneManager.LoadScene("Level1");
	}

	public void Settings(){
		SceneManager.LoadScene("Settings");
	}

	public void Menu(){
		SceneManager.LoadScene("Menu");
	}

	public void Musique(){
		SceneManager.LoadScene("Musique");
	}

	public void credits(){
		SceneManager.LoadScene("Crédits");
	}

	public void choix(){
		SceneManager.LoadScene("Choix");
	}
	
	//public void ShowOptions(){
		//panel_options.SetActive(true);
	//}
	
	//public void UnshowOptions(){
		//panel_options.SetActive(false);
	//}
	
	public void QuitGame(){
		Application.Quit();
	}
}
