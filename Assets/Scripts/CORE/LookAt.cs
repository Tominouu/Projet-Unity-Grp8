using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
	private Transform player;
	private Vector3 lastPlayerPosition;
	private float rotationSpeed = 5f; // Ajustable
		
	void Start(){
		player = GameObject.FindWithTag("Player").transform;
		lastPlayerPosition = getPlayerPosition();
	}
	
	void Update(){
		Vector3 targetPosition = getPlayerPosition();
		Vector3 direction = targetPosition - transform.position;
		if (direction != Vector3.zero && targetPosition != lastPlayerPosition) { 
			lastPlayerPosition = targetPosition;
			//transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
			
			//rotation fluide
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * rotationSpeed);
		}
	}
	
	private Vector3 getPlayerPosition(){
		return new Vector3(player.position.x, player.position.y, player.position.z);
	}
    
}
