using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveForward : MonoBehaviour
{
    private Transform player;
    public float normalSpeed = 0.1f;
    public float sprintSpeed = 0.25f;
    private float currentSpeed;

    private Timer timerScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentSpeed = normalSpeed;

        timerScript = GameObject.FindObjectOfType<Timer>();
        if (timerScript == null)
        {
            Debug.LogWarning("Script Timer non trouvé dans la scène!");
        }
    }

    void Update()
    {
        if (timerScript != null && timerScript.timeRemaining <= 0)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        // Suppression de la distance pour que l'ennemi suive toujours
        if (!HudManager.pause)
        {
            Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, currentSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && timerScript != null && timerScript.timeRemaining <= 0 ){
            SceneManager.LoadScene("GameOver");
        }
    }
}
