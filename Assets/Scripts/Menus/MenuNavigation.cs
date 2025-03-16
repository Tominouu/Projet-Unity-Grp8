using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons;
    public GameObject selectionArrow; 
    private int currentIndex = 0;
    
    void Start()
    {
        
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
        for (int i = 0; i < menuButtons.Length; i++)
        {
            
            ColorBlock colors = menuButtons[i].colors;
            colors.normalColor = Color.white;
            menuButtons[i].colors = colors;
        }
        
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        
        ColorBlock selectedColors = menuButtons[index].colors;
        selectedColors.normalColor = new Color(0.9f, 0.9f, 0.9f); 
        menuButtons[index].colors = selectedColors;
        
        if (selectionArrow != null)
        {
            RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();
            RectTransform arrowRect = selectionArrow.GetComponent<RectTransform>();
            //à changer si l'arrow est trop loin ou trop proche
            float arrowOffset = 100f; 
            Vector3 newPosition = buttonRect.position;
            newPosition.x -= (buttonRect.rect.width / 2 + arrowRect.rect.width / 2 + arrowOffset);
            
            selectionArrow.transform.position = newPosition;
            
            selectionArrow.SetActive(true);
        }
    }
}