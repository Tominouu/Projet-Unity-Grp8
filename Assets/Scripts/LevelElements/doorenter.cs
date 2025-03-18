using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(-3f, 0, 0); // Distance de déplacement de la porte
    public float speed = 2f; // Vitesse d'ouverture

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isOpening = false;

    void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + openOffset;
    }

    void Update()
    {
        if (_isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, _openPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assurez-vous que le joueur a bien le tag "Player"
        {
            _isOpening = true;
        }
    }
}
