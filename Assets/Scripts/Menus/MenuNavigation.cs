using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons;
    public GameObject selectionArrow; // Référence à l'objet flèche
    private int currentIndex = 0;
    
    void Start()
    {
        // Sélectionner le premier bouton au démarrage
        SelectButton(currentIndex);
    }
    
    void Update()
    {
        // Navigation avec les touches Z/S ou flèches haut/bas
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateUp();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateDown();
        }
        
        // Activer le bouton sélectionné avec la touche Entrée
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }
    
    void NavigateUp()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = menuButtons.Length - 1;
            
        SelectButton(currentIndex);
    }
    
    void NavigateDown()
    {
        currentIndex++;
        if (currentIndex >= menuButtons.Length)
            currentIndex = 0;
            
        SelectButton(currentIndex);
    }
    
    void SelectButton(int index)
    {
        // Désélectionner tous les boutons
        for (int i = 0; i < menuButtons.Length; i++)
        {
            // Réinitialiser l'apparence des boutons
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = Color.white;
            menuButtons[i].colors = colors;
        }
        
        // Sélectionner le bouton actuel
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        
        // Mettre en évidence le bouton sélectionné
        ColorBlock selectedColors = menuButtons[index].colors;
        selectedColors.normalColor = new Color(0.9f, 0.9f, 0.9f); // Légèrement plus clair
        menuButtons[index].colors = selectedColors;
        
        // Positionner la flèche à côté du bouton sélectionné
        if (selectionArrow != null)
        {
            RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();
            RectTransform arrowRect = selectionArrow.GetComponent<RectTransform>();
            
            // Positionner la flèche à gauche du bouton
            // Ajustez ces valeurs selon vos besoins
            float arrowOffset = 100f; // Distance entre la flèche et le bouton
            Vector3 newPosition = buttonRect.position;
            newPosition.x -= (buttonRect.rect.width / 2 + arrowRect.rect.width / 2 + arrowOffset);
            
            selectionArrow.transform.position = newPosition;
            
            // S'assurer que la flèche est visible
            selectionArrow.SetActive(true);
        }
    }
}