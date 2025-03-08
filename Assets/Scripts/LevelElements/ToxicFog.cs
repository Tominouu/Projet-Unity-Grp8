using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicFog : MonoBehaviour
{
	private bool playerContact = false;
	private HudManager hud;
	private float cooldown = 0.1f;
	private float timerCooldown = 0;
	
	void Start(){
		hud = HudManager.instance;
	}
	
	void Update(){
		if(playerContact){
			//On enlève 1 PV au joueur toutes les 0.1s
			if(timerCooldown <= 0){
				hud.subPV(1);
				timerCooldown = cooldown;
			} else {
				timerCooldown -= Time.deltaTime;
			}
			
		}
	}
	
    //Si on touche son collider
	void OnTriggerEnter(Collider col){
    	if (col.gameObject.tag == "Player" && !playerContact){
        	playerContact = true;
			AudioManager am = AudioManager.instance;
			am.PlaySFX(am.sfx_list.sfx_hit);
    	}
	}
	
	//Si on sort du collider
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player"){
			playerContact = false;
			timerCooldown = 0;
		}
	}
}
