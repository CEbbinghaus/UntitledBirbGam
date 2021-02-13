using System;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    Null,
    Menu,
    SubMenu
}

public class ControllerInputMenu : ControllerMenuInputGeneric
{
    public static ControllerInputMenu instance;
    public Transform landscapeButtons;
    public Transform portraitButtons;
    List<ControllerButton> menuButtonsLandscape = new List<ControllerButton>();
    List<ControllerButton> menuButtonsPortrait = new List<ControllerButton>();

    ControllerInputMenu()
    {
        menuState = MenuState.Menu;
    }

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

        OrientationManager.onChangeUIOrientation += UpdateOrientation;
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
        UpdateOrientation(Screen.orientation);
    }

    protected override void Update()
    {
        base.Update();
    }

    void UpdateOrientation(ScreenOrientation screenOrientation)
    {
        switch (screenOrientation)
        {
            case ScreenOrientation.Portrait:
                activeMenuButtons = menuButtonsPortrait;
                if (activeSubMenu == UIPan.instance.creditsLandscape)
                    activeSubMenu = UIPan.instance.creditsPortrait;
                if (activeSubMenu == UIPan.instance.optionsLandscape)
                    activeSubMenu = UIPan.instance.optionsPortrait;
                break;
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
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

    public override void ExitSubMenu()
    {
        UIPan.instance.ChangeState(activeSubMenu, UIPanState.MovingOffscreen);
        activeSubMenu = null;
        SetSelectedButton(currentIndex);
        menuState = MenuState.Menu;
    }
}
