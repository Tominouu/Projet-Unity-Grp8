using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // La liste des objets à réinitialiser, par exemple, les items à réapparaître
    public string[] itemTags; // Les tags des objets (Item1, Item2, Item3...)
    
    void Start()
    {
        // Assurez-vous que ce script persiste entre les scènes
        DontDestroyOnLoad(gameObject);

        // Lors du changement de scène, réinitialiser les objets
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Quand une scène est chargée, réinitialise les objets
        ResetItems();
    }

    private void ResetItems()
    {
        // Pour chaque tag dans le tableau itemTags
        foreach (var tag in itemTags)
        {
            // Trouve l'objet avec ce tag
            GameObject item = GameObject.FindWithTag(tag);
            
            // Si l'objet existe, réactive-le
            if (item != null)
            {
                item.SetActive(true);
            }
        }

        // Tu peux aussi réinitialiser d'autres choses ici si nécessaire
    }

    // Facultatif: Détruit l'objet de gestion de scène si tu veux qu'il ne persiste pas après un certain moment
    public void DestroyManager()
    {
        Destroy(gameObject);
    }
}
