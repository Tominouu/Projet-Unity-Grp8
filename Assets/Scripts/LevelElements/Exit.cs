using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool active = true;
    //Si on touche son collider
	void OnTriggerEnter(Collider col){
    	if (col.gameObject.tag == "Player" && active){
        	HudManager hud = HudManager.instance; //On récupère le hud
			hud.showTimedMessage("Vous avez gagné !");
			active = false; //Evite de revenir dans le script une fois l'objet supprimé
			AudioManager am = AudioManager.instance;
			am.PlaySFX(am.sfx_list.sfx_end);
			am.StopMusic();
    	}
	}
}
