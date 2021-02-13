using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ControllerMenuInputGeneric : MonoBehaviour
{
    public MenuState menuState;

    protected int currentIndex = -1;
    protected List<ControllerButton> activeMenuButtons = new List<ControllerButton>();

    public float holdDelay = 0.5f;
    public float sensitivity = 0.3f;
    protected float timer = 0;

    [NonSerialized] public UIPanElement activeSubMenu;

    protected virtual void Update()
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
                        activeMenuButtons[currentIndex].Press();
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
                        SetSelectedButton(currentIndex == -1 ? 0 : (currentIndex + activeMenuButtons.Count - 1) % activeMenuButtons.Count);
                        return;
                    }

                    // Move down
                    if (Input.GetAxis("Vertical") < -sensitivity)
                    {
                        SetSelectedButton(currentIndex == -1 ? 0 : (currentIndex + activeMenuButtons.Count + 1) % activeMenuButtons.Count);
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
            activeMenuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");

        currentIndex = index;
        // If the player is using a mouse, don't set a selected button
        if (currentIndex == -1) return;

        activeMenuButtons[currentIndex].buttonText.fontMaterial.EnableKeyword("GLOW_ON");

        activeMenuButtons[currentIndex].button.Select();
        timer = holdDelay;
    }

    public void SetSelectedButton(ControllerButton button)
    {
        // No button previously selected, nothing to reset.
        if (currentIndex != -1)
            activeMenuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");

        currentIndex = button.index;
        activeMenuButtons[currentIndex].buttonText.fontMaterial.EnableKeyword("GLOW_ON");
        activeMenuButtons[currentIndex].button.Select();
        timer = holdDelay;
    }

    public void CancelControllerInput(ControllerButton button)
    {
        // Already not using controller, return
        if (currentIndex == -1) return;

        if (button == null || currentIndex == button.index)
        {
            activeMenuButtons[currentIndex].buttonText.fontMaterial.DisableKeyword("GLOW_ON");
            EventSystem.current.SetSelectedGameObject(null);
            currentIndex = -1;
        }
    }

    public abstract void ExitSubMenu();
}
