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
    private Animator animator; // Référence à l'Animator

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentSpeed = normalSpeed;

        timerScript = GameObject.FindObjectOfType<Timer>();
        if (timerScript == null)
        {
            Debug.LogWarning("Script Timer non trouvé dans la scène!");
        }

        // Récupérer l'Animator attaché à l'ennemi
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Aucun Animator trouvé sur cet objet !");
        }
    }

    void Update()
    {
        if (timerScript != null && timerScript.timeRemaining <= 5)
        {
            currentSpeed = sprintSpeed;
            if (animator != null)
            {
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            currentSpeed = normalSpeed;
            if (animator != null)
            {
                animator.SetBool("isRunning", false);
            }
        }

        if (!HudManager.pause)
        {
            Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, currentSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && timerScript.timeRemaining <= 5)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
