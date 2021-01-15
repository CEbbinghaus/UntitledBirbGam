using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
        int setIndex = 0;
        foreach (Transform child in transform)
        {
            MenuButton button = child.GetComponent<MenuButton>();
            if (button)
            {
                button.index = setIndex;
                menuButtons.Add(button);
                setIndex++;
            }
        }
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
                    CancelControllerInput(null);
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
                    // Move Up
                    if (Input.GetAxis("Vertical") > sensitivity)
                    {
                        SetHoverButton(currentIndex == -1 ? 0 : (currentIndex + menuButtons.Count - 1) % menuButtons.Count);
                        return;
                    }

                    // Move down
                    if (Input.GetAxis("Vertical") < -sensitivity)
                    {
                        SetHoverButton(currentIndex == -1 ? 0 : (currentIndex + menuButtons.Count + 1) % menuButtons.Count);
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

    public void SetHoverButton(int index)
    {
        currentIndex = index;
        menuButtons[currentIndex].button.Select();
        timer = holdDelay;
    }

    public void SetHoverButton(MenuButton button)
    {
        currentIndex = button.index;
        menuButtons[currentIndex].button.Select();
        timer = holdDelay;
    }

    public void CancelControllerInput(MenuButton button)
    {
        if (button == null | currentIndex == button.index)
        {
            EventSystem.current.SetSelectedGameObject(null);
            currentIndex = -1;
        }
    }
}
