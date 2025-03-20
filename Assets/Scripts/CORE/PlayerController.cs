using UnityEngine;

public class PlayerController : MonoBehaviour {
    //Vitesse en marchant et en courant
    [SerializeField] private float walk, run;

    //Sensibilité de la souris
    private float sensitivity = 500;
    public float speed;
    private float normalWalk, normalRun; // Normal speeds without slowdown
    private float slowFactor = 0.5f; // Slowdown factor for puddles

    private bool isMoving = false;
    private bool isRunning = false;
    private bool isInPuddle = false; // To track if player is in puddle

    private CharacterController cc;

    private float X, Y;

    //Pour les bruits de pas
    [SerializeField] private AudioClip[] sfx_steps;
    private int num_step = 0;
    private float step_timer = 0.0f;
    private float max_step_timer = 0.5f;
    private AudioSource audio_steps;

    // Référence au gestionnaire de touches
    private KeyBindingManager keyManager;

    private void Start()
    {
        normalWalk = walk; // Store original walk speed
        normalRun = run;   // Store original run speed
        speed = walk;
        cc = GetComponent<CharacterController>();
        audio_steps = GetComponent<AudioSource>();
        cc.enabled = true;

        // Récupérer la référence au KeyBindingManager
        keyManager = KeyBindingManager.Instance;
        if (keyManager == null)
        {
            Debug.LogWarning("KeyBindingManager non trouvé! Les contrôles par défaut seront utilisés.");
        }

        //Rend le curseur invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(!HudManager.pause){
            //Camera limitation variables
            const float MIN_Y = -60.0f;
            const float MAX_Y = 70.0f;

            Y -= Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);

            if (Y < MIN_Y)
                Y = MIN_Y;
            else if (Y > MAX_Y)
                Y = MAX_Y;

            X += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);

            transform.localRotation = Quaternion.Euler(Y, X, 0.0f);

            // Utiliser les touches personnalisées si KeyBindingManager est disponible
            float horizontal = 0f;
            float vertical = 0f;

            if (keyManager != null)
            {
                // Déplacement horizontal (gauche/droite)
                if (keyManager.IsActionPressed(KeyBindingManager.GameAction.Droite))
                    horizontal += 1f;
                if (keyManager.IsActionPressed(KeyBindingManager.GameAction.Gauche))
                    horizontal -= 1f;

                // Déplacement vertical (avant/arrière)
                if (keyManager.IsActionPressed(KeyBindingManager.GameAction.Haut))
                    vertical += 1f;
                if (keyManager.IsActionPressed(KeyBindingManager.GameAction.Bas))
                    vertical -= 1f;
            }
            else
            {
                // Utiliser les contrôles par défaut si aucun KeyBindingManager n'est trouvé
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }

            Vector3 forward = transform.forward * vertical;
            Vector3 right = transform.right * horizontal;

            cc.SimpleMove((forward + right) * speed);
            isMoving = (horizontal != 0 || vertical != 0);

            // Vérifier la touche pour courir (utiliser la touche personnalisée si disponible)
            bool runKeyPressed = Input.GetKey(KeyCode.LeftShift); // Par défaut

            //Si on appuie sur Shift Gauche, on court
            if (runKeyPressed)
            {
                speed = isInPuddle ? (normalRun * slowFactor) : normalRun;
                isRunning = true;
            }
            else
            {
                isRunning = false;
                speed = isInPuddle ? (normalWalk * slowFactor) : normalWalk;
            }

            // Gérer les actions d'ouverture et de prise
            if (keyManager != null)
            {
                if (keyManager.IsActionDown(KeyBindingManager.GameAction.Ouvrir))
                {
                    // Action pour ouvrir (portes, coffres, etc.)
                    HandleOpenAction();
                }

                if (keyManager.IsActionDown(KeyBindingManager.GameAction.Prendre))
                {
                    // Action pour prendre des objets
                    HandleTakeAction();
                }
            }

            // Gestion des bruits de pas
            if(isMoving){ //Si le joueur se déplace, on joue des bruits de pas
                if(step_timer <= 0){
                    audio_steps.clip = sfx_steps[num_step];
                    audio_steps.Play();

                    step_timer = max_step_timer;
                    if(isRunning){ //S'il court, on divise par 2 le temps avant d'entendre un nouveau pas
                        step_timer /= 2;
                    }
                    num_step = (num_step+1)%sfx_steps.Length;
                } else {
                    step_timer -= Time.deltaTime;
                }
            } else {
                step_timer = 0.1f;
            }
        }

        // Gestion pour ouvrir le menu des commandes (par exemple avec Escape)
        if (Input.GetKeyDown(KeyCode.Escape) && keyManager != null)
        {
            ToggleCommandesMenu();
        }
    }

    // Use OnControllerColliderHit for CharacterController collisions
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.CompareTag("flaque")) {
            isInPuddle = true;
            // Update speed immediately
            if (isRunning) {
                speed = normalRun * slowFactor;
            } else {
                speed = normalWalk * slowFactor;
            }
        }
    }

    // Add method to detect when player exits puddle
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("flaque")) {
            isInPuddle = false;
            // Reset speed to normal
            if (isRunning) {
                speed = normalRun;
            } else {
                speed = normalWalk;
            }
        }
    }

    // Nouvelle méthode pour gérer l'action d'ouvrir
    private void HandleOpenAction()
    {
        // Détecter les objets à proximité avec lesquels on peut interagir
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Interactive"))
            {
                IInteractable interactable = hitCollider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                    break; // Interagir avec un seul objet à la fois
                }
            }
        }
    }

    // Nouvelle méthode pour gérer l'action de prendre
    private void HandleTakeAction()
    {
        // Détecter les objets à proximité qu'on peut ramasser
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Pickable"))
            {
                // Logique pour ramasser l'objet
                Debug.Log("Objet ramassé : " + hitCollider.gameObject.name);
                Destroy(hitCollider.gameObject); // Ou mettez votre logique d'inventaire ici
                break; // Ramasser un seul objet à la fois
            }
        }
    }

    // Méthode pour afficher/masquer le menu des commandes
    public void ToggleCommandesMenu()
    {
        if (keyManager != null && keyManager.commandesMenuCanvas != null)
        {
            bool isMenuActive = keyManager.commandesMenuCanvas.activeSelf;
            keyManager.ToggleCommandesMenu(!isMenuActive);

            // Mettre le jeu en pause pendant que le menu est ouvert
            HudManager.pause = !isMenuActive; // Utilisez votre système de pause existant
        }
    }
}

// Interface pour les objets interactifs
public interface IInteractable
{
    void Interact();
}
