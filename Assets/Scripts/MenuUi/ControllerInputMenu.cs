﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MenuState 
{ 
    Null,
    Menu,
    SubMenu
}

public class ControllerInputMenu : MonoBehaviour
{
    public static ControllerInputMenu instance;

    public MenuState menuState = MenuState.Menu;
    [HideInInspector]
    public UIPanElement activeSubMenu;
    public float holdDelay = 0.5f;
    public float sensitivity = 0.3f;
    float timer = 0;
    List<ControllerButton> menuButtons = new List<ControllerButton>();
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
            ControllerButton button = child.GetComponent<ControllerButton>();
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
                    if (currentIndex >= 0)
                    {
                        menuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");
                        menuButtons[currentIndex].Press();
                    }
                    else
                    {
                        SetSelectedButton(0);
                    }
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
                        SetSelectedButton(currentIndex == -1 ? 0 : (currentIndex + menuButtons.Count - 1) % menuButtons.Count);
                        return;
                    }

                    // Move down
                    if (Input.GetAxis("Vertical") < -sensitivity)
                    {
                        SetSelectedButton(currentIndex == -1 ? 0 : (currentIndex + menuButtons.Count + 1) % menuButtons.Count);
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
                if (Input.GetButtonUp("Cancel"))
                {
                    ExitSubMenu();
                }
                break;
        }
    }

    public void SetSelectedButton(int index)
    {
        // No button previously selected, nothing to reset.
        if (currentIndex != -1)
            menuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");

        currentIndex = index;
        // If the player is using a mouse, don't set a selected button
        if (currentIndex == -1) return;

        menuButtons[currentIndex].buttonText.fontMaterial.EnableKeyword("GLOW_ON");

        menuButtons[currentIndex].button.Select();
        timer = holdDelay;
    }

    public void SetSelectedButton(ControllerButton button)
    {
        // No button previously selected, nothing to reset.
        if (currentIndex != -1)
            menuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");

        currentIndex = button.index;
        menuButtons[currentIndex].buttonText.fontMaterial.EnableKeyword("GLOW_ON");
        menuButtons[currentIndex].button.Select();
        timer = holdDelay;
    }

    public void CancelControllerInput(ControllerButton button)
    {
        // Already not using controller, return
        if (currentIndex == -1) return;

        if (button == null || currentIndex == button.index)
        {
            menuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");
            EventSystem.current.SetSelectedGameObject(null);
            currentIndex = -1;
        }
    }

    public void ExitSubMenu()
    {
        UIPan.instance.ChangeState(activeSubMenu, UIPanState.MovingOffscreen);
        activeSubMenu = null;
        SetSelectedButton(currentIndex);
        menuState = MenuState.Menu;
    }
}
