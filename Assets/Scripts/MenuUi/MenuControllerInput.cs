using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState
{
    Null,
    Menu,
    SubMenu
}

public class MenuControllerInput : MonoBehaviour
{
    public static MenuControllerInput instance;

    public MenuState menuState = MenuState.Menu;
    [HideInInspector]
    public GameObject activeSubMenu;
    public float holdDelay = 0.5f;
    public float sensitivity = 0.3f;
    float timer = 0;
    List<MenuButton> menuButtons = new List<MenuButton>();
    int currentIndex = -1;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        menuButtons = FindObjectsOfType<MenuButton>().OrderBy(mb => mb.index).ToList();
    }

    void Update()
    {
        switch (menuState)
        {
            case MenuState.Menu:
				#region Menu
				// -1 index is if mouse is currently being used
				if (Input.GetMouseButtonDown(0))
                {
                    CancelControllerInput();
                }

                // Pressing the button
                if (Input.GetButtonDown("Submit"))
                {
                    menuButtons[currentIndex].Press();
                }

                // Joystick is at rest, reset the timer
                if (Input.GetAxis("Vertical") <= 0.5 && Input.GetAxis("Vertical") >= -0.5)
                {
                    timer = 0;
                }

                // Changing selected button if the joystick was previously at rest
                if (timer == 0)
                {
                    if (Input.GetAxis("Vertical") > sensitivity)
                    {
                        if (currentIndex == -1)
                        {
                            SetInitialButton();
                            return;
                        }
                        Button currentButton = menuButtons[currentIndex].button;
                        currentButton.targetGraphic.color = currentButton.colors.normalColor;
                        currentIndex = (currentIndex + menuButtons.Count - 1) % menuButtons.Count;
                        Button newButton = menuButtons[currentIndex].button;
                        newButton.targetGraphic.color = newButton.colors.highlightedColor;
                        timer = holdDelay;
                        return;
                    }

                    if (Input.GetAxis("Vertical") < -sensitivity)
                    {
                        if (currentIndex == -1)
                        {
                            SetInitialButton();
                            return;
                        }
                        Button currentButton = menuButtons[currentIndex].button;
                        currentButton.targetGraphic.color = currentButton.colors.normalColor;
                        currentIndex = (currentIndex + menuButtons.Count + 1) % menuButtons.Count;
                        Button newButton = menuButtons[currentIndex].button;
                        newButton.targetGraphic.color = newButton.colors.highlightedColor;
                        timer = holdDelay;
                        return;
                    }
                }

                if (timer > 0)
                {
                    timer = Mathf.Max(0, timer -= Time.deltaTime);
                }
                break;
			#endregion
			case MenuState.SubMenu:
                if (Input.GetButtonDown("Cancel"))
                {
                    activeSubMenu.SetActive(false);
                    activeSubMenu = null;
                    menuState = MenuState.Menu;
                }
                break;
        }
    }

    void SetInitialButton()
    {
        Button button = menuButtons[0].button;
        button.targetGraphic.color = button.colors.highlightedColor;
        currentIndex = 0;
        timer = holdDelay;
    }

    public void CancelControllerInput()
    {
        menuButtons.ForEach(b => b.button.targetGraphic.color = b.button.colors.normalColor);
        currentIndex = -1;
    }
}
