using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicKey : MonoBehaviour
{
	private bool active = true;
	//Si on touche son collider
	void OnTriggerEnter(Collider col){
    	if (col.gameObject.tag == "Player" && active){
        	HudManager hud = HudManager.instance; //On récupère le hud
			
			//Si le joueur n'a pas déjà un item
			if(!hud.hasItem()){
				hud.addItem(Item.ClassicKey);
				hud.showTimedMessage("Clé obtenue");
				Destroy(this.gameObject);
				active = false; //Evite de revenir dans le script une fois l'objet supprimé
				AudioManager am = AudioManager.instance;
				am.PlaySFX(am.sfx_list.sfx_key);
			} else{
				hud.showMessage("Votre inventaire est déjà rempli.");
			}

    	}
	}
	
	//Si on sort du collider
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player" && active){
			HudManager hud = HudManager.instance;
			hud.eraseMessage();
		}
	}
}
