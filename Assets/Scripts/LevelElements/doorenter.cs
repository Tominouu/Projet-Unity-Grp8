using UnityEngine;
using UnityEngine.UI;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(-3f, 0, 0); // Distance de déplacement de la porte
    public float speed = 2f; // Vitesse d'ouverture/fermeture
    public Text hudMessageText; // Référence au composant Text pour afficher le message
    public string doorOpenMessage = "Le Magasin Ferme Dans 5 Minutes"; // Message à afficher
    public float messageDisplayTime = 3f; // Durée d'affichage du message en secondes
    public float doorDelay = 1.5f; // Délai avant que la porte ne se ferme après sortie du joueur

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isOpening = false;
    private bool _isClosing = false;
    private bool _playerInTrigger = false;
    private float _messageTimer = 0f;
    private float _doorCloseTimer = 0f;
    private bool _doorFullyOpened = false;

    void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + openOffset;

        // S'assurer que le texte est invisible au démarrage
        if (hudMessageText != null)
        {
            hudMessageText.enabled = false;
        }
    }

    void Update()
    {
        // Gestion de l'ouverture de la porte
        if (_isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, _openPosition, speed * Time.deltaTime);

            // Vérifier si la porte est presque complètement ouverte
            if (Vector3.Distance(transform.position, _openPosition) < 0.01f)
            {
                transform.position = _openPosition;
                _isOpening = false;
                _doorFullyOpened = true;
            }
        }

        // Gestion de la fermeture de la porte avec délai
        if (!_playerInTrigger && _doorFullyOpened)
        {
            if (_doorCloseTimer <= 0)
            {
                _isClosing = true;
                _doorFullyOpened = false;
            }
            else
            {
                _doorCloseTimer -= Time.deltaTime;
            }
        }

        // Mouvement de fermeture
        if (_isClosing)
        {
            transform.position = Vector3.Lerp(transform.position, _closedPosition, speed * Time.deltaTime);

            // Vérifier si la porte est presque complètement fermée
            if (Vector3.Distance(transform.position, _closedPosition) < 0.01f)
            {
                transform.position = _closedPosition;
                _isClosing = false;
            }
        }

        // Gestion du timer pour le message HUD
        if (_messageTimer > 0)
        {
            _messageTimer -= Time.deltaTime;

            if (_messageTimer <= 0 && hudMessageText != null)
            {
                hudMessageText.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assurez-vous que le joueur a bien le tag "Player"
        {
            _playerInTrigger = true;
            _isOpening = true;
            _isClosing = false;
            _doorCloseTimer = doorDelay;

            // Afficher le message sur le HUD
            if (hudMessageText != null)
            {
                hudMessageText.text = doorOpenMessage;
                hudMessageText.enabled = true;
                _messageTimer = messageDisplayTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
            _doorCloseTimer = doorDelay; // Attendre un peu avant de fermer la porte
        }
    }
}
