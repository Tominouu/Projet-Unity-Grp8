using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsNavigation : MonoBehaviour
{
    public Button[] menuButtons;
    public GameObject selectionArrow;
    private int currentIndex = 0;
    private ArrowAnimation arrowAnim;
    
    void Start()
    {
        if (selectionArrow != null)
            arrowAnim = selectionArrow.GetComponent<ArrowAnimation>();
            
        SelectButton(currentIndex);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateUp();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateDown();
        }
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
    // Vérifier si les index sont valides
    if (index < 0 || index >= menuButtons.Length || menuButtons[index] == null)
        return;
        
    // Désélectionner tous les boutons
    for (int i = 0; i < menuButtons.Length; i++)
    {
        if (menuButtons[i] != null)
        {
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = Color.white;
            menuButtons[i].colors = colors;
        }
    }
    
    // Sélectionner le bouton actuel
    EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
    
    // Mettre en évidence le bouton sélectionné
    ColorBlock selectedColors = menuButtons[index].colors;
    selectedColors.normalColor = new Color(0.9f, 0.9f, 0.9f);
    menuButtons[index].colors = selectedColors;
    
    // Positionner la flèche à côté du bouton sélectionné
    if (selectionArrow != null)
    {
        RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();
        RectTransform arrowRect = selectionArrow.GetComponent<RectTransform>();
        
        // Calculer la position précise pour la flèche
        // Aligner le centre vertical de la flèche avec le centre vertical du bouton
        float arrowOffset = 20f; // Distance entre la flèche et le bouton, ajustez selon vos besoins
        
        Vector3 newPosition = buttonRect.position;
        newPosition.x = buttonRect.position.x - (buttonRect.rect.width / 2) - (arrowRect.rect.width / 2) - arrowOffset;
        
        // Utiliser le même Y que le bouton pour un alignement vertical parfait
        
        // Mettre à jour la position de base de l'animation
        if (arrowAnim != null)
        {
            arrowAnim.UpdateBasePosition(newPosition);
        }
        else
        {
            // Positionner directement la flèche
            selectionArrow.transform.position = newPosition;
        }
    }
}
}