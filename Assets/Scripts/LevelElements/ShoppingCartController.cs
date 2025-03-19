/*using UnityEngine;

public class ShoppingCartController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject cartPrefab;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float maxGrabDistance = 2.5f;
    [SerializeField] private float holdDistance = 1.5f;
    [SerializeField] private LayerMask cartLayer;

    [Header("Paramètres")]
    [SerializeField] private KeyCode grabKey = KeyCode.R;
    [SerializeField] private float cartRotationSmoothing = 5f;
    [SerializeField] private Vector3 spawnScale = Vector3.one; // Échelle par défaut pour les nouveaux caddies

    // Variables privées
    private GameObject currentCart;
    private bool isHoldingCart = false;
    private Rigidbody cartRigidbody;
    private Vector3 originalScale; // Pour stocker l'échelle originale du caddie

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (isHoldingCart)
            {
                ReleaseCart();
            }
            else
            {
                TryGrabCart();
            }
        }

        if (isHoldingCart)
        {
            MoveCartWithPlayer();
        }
    }

    private void TryGrabCart()
    {
        // Chercher un caddie à proximité
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, maxGrabDistance, cartLayer))
        {
            if (hit.collider.CompareTag("ShoppingCart"))
            {
                // Prendre le caddie existant
                currentCart = hit.collider.gameObject;
                cartRigidbody = currentCart.GetComponent<Rigidbody>();

                // Mémoriser l'échelle originale
                originalScale = currentCart.transform.localScale;

                // Activer la cinématique pour le déplacement contrôlé
                if (cartRigidbody != null)
                {
                    cartRigidbody.isKinematic = true;
                }

                isHoldingCart = true;
            }
        }
        else
        {
            // Créer un nouveau caddie si aucun n'est trouvé
            SpawnNewCart();
        }
    }

    private void SpawnNewCart()
    {
        // Calculer la position devant le joueur
        Vector3 spawnPosition = playerCamera.position + playerCamera.forward * holdDistance;

        // Créer le caddie
        currentCart = Instantiate(cartPrefab, spawnPosition, Quaternion.identity);
        currentCart.tag = "ShoppingCart";

        // Appliquer l'échelle configurée
        currentCart.transform.localScale = spawnScale;
        originalScale = spawnScale;

        // Configurer le rigidbody
        cartRigidbody = currentCart.GetComponent<Rigidbody>();
        if (cartRigidbody == null)
        {
            cartRigidbody = currentCart.AddComponent<Rigidbody>();
        }
        cartRigidbody.isKinematic = true;

        isHoldingCart = true;
    }

    private void MoveCartWithPlayer()
    {
        if (currentCart != null)
        {
            // Position cible devant le joueur
            Vector3 targetPosition = playerCamera.position + playerCamera.forward * holdDistance;

            // Déplacer le caddie vers la position cible
            currentCart.transform.position = targetPosition;

            // S'assurer que l'échelle reste la même que l'originale
            currentCart.transform.localScale = originalScale;

            // Rotation du caddie pour qu'il suive la direction du joueur mais garde son orientation verticale
            Quaternion targetRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
            currentCart.transform.rotation = Quaternion.Slerp(
                currentCart.transform.rotation,
                targetRotation,
                Time.deltaTime * cartRotationSmoothing
            );
        }
    }

    private void ReleaseCart()
    {
        if (currentCart != null && cartRigidbody != null)
        {
            // S'assurer que l'échelle reste la même lors du relâchement
            currentCart.transform.localScale = originalScale;

            // Désactiver la cinématique pour les collisions physiques
            cartRigidbody.isKinematic = false;

            // Ajouter une petite impulsion dans la direction du regard
            cartRigidbody.AddForce(playerCamera.forward * 2f, ForceMode.Impulse);
        }

        isHoldingCart = false;
        currentCart = null;
        cartRigidbody = null;
    }
}*/
