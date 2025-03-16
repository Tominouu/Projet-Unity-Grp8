using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
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
        
        if (index < 0 || index >= menuButtons.Length || menuButtons[index] == null)
            return;
            
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] != null)
            {
                ColorBlock colors = menuButtons[i].colors;
                colors.normalColor = Color.white;
                menuButtons[i].colors = colors;
            }
        }
        
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
        
        ColorBlock selectedColors = menuButtons[index].colors;
        selectedColors.normalColor = new Color(0.9f, 0.9f, 0.9f);
        menuButtons[index].colors = selectedColors;
        
        if (selectionArrow != null)
        {
            RectTransform buttonRect = menuButtons[index].GetComponent<RectTransform>();
            
            Vector3 newPosition = buttonRect.position;
            newPosition.x -= 230f; 
        
            if (arrowAnim != null)
            {
                
                arrowAnim.UpdateBasePosition(new Vector3(newPosition.x, newPosition.y, newPosition.z));
            }
            else
            {
                
                selectionArrow.transform.position = newPosition;
            }
        }
    }
}