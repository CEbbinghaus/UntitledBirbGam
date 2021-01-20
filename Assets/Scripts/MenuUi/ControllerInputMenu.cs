using System.Collections.Generic;
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
    public ScreenOrientation DEBUGOrientation;
    public MenuState menuState = MenuState.Menu;
    public UIPanElement activeSubMenu;
    public float holdDelay = 0.5f;
    public float sensitivity = 0.3f;
    float timer = 0;
    public Transform landscapeButtons;
    public Transform portraitButtons;
    List<ControllerButton> menuButtonsLandscape = new List<ControllerButton>();
    List<ControllerButton> menuButtonsPortrait = new List<ControllerButton>();
    List<ControllerButton> activeMenuButtons = new List<ControllerButton>();
    int currentIndex = -1;
    ScreenOrientation cachedOrientation;

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
        Screen.orientation = DEBUGOrientation;
    }

    private void Start()
    {
        int setIndex = 0;
        foreach (Transform child in landscapeButtons)
        {
            ControllerButton button = child.GetComponent<ControllerButton>();
            if (button)
            {
                button.index = setIndex;
                menuButtonsLandscape.Add(button);
                setIndex++;
            }
        }
        setIndex = 0;
        foreach (Transform child in portraitButtons)
        {
            ControllerButton button = child.GetComponent<ControllerButton>();
            if (button)
            {
                button.index = setIndex;
                menuButtonsPortrait.Add(button);
                setIndex++;
            }
        }
        UpdateOrientation();
    }

    void Update()
    {
        //if (Screen.orientation != cachedOrientation)
        if (DEBUGOrientation != cachedOrientation)
        {
            UpdateOrientation();
        }
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

    void UpdateOrientation()
    {
        //switch (Screen.orientation)
        switch (DEBUGOrientation)
        {
            case ScreenOrientation.Portrait:
                cachedOrientation = ScreenOrientation.Portrait;
                activeMenuButtons = menuButtonsPortrait;
                if (activeSubMenu == UIPan.instance.creditsLandscape)
                    activeSubMenu = UIPan.instance.creditsPortrait;
                if (activeSubMenu == UIPan.instance.optionsLandscape)
                    activeSubMenu = UIPan.instance.optionsPortrait;
                break;
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
                cachedOrientation = ScreenOrientation.Landscape;
                activeMenuButtons = menuButtonsLandscape;
                if (activeSubMenu == UIPan.instance.creditsPortrait)
                    activeSubMenu = UIPan.instance.creditsLandscape;
                if (activeSubMenu == UIPan.instance.optionsPortrait)
                    activeSubMenu = UIPan.instance.optionsLandscape;
                break;
            default:
                goto case ScreenOrientation.LandscapeRight;
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

    public void ExitSubMenu()
    {
        UIPan.instance.ChangeState(activeSubMenu, UIPanState.MovingOffscreen);
        activeSubMenu = null;
        SetSelectedButton(currentIndex);
        menuState = MenuState.Menu;
    }
}
