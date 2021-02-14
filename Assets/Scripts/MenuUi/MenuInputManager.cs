using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInputManager : MonoBehaviour
{
    public static MenuInputManager instance;

    public Menu defaultMenu;

    [NonSerialized]
    Stack<Menu> activeMenuHierarchy = new Stack<Menu>();
    [NonSerialized]
    Stack<int> activeIndexHierarchy = new Stack<int>();
    [NonSerialized]
    int activeIndex;


    public float timer;
    public float holdDelayVertical = 0.5f;
    public float holdDelayHorizontal = 0.5f;
    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.7f;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else this.enabled = false;


        if (defaultMenu)
        {
            OpenMenu(defaultMenu);
        }
    }

    private void Update()
    {
        if (activeMenuHierarchy.Count > 0)
        {
            // -1 index is if mouse is currently being used
            if (Input.GetMouseButtonDown(0))
            {
                CancelControllerInput(null);
            }

            // Pressing the button
            if (Input.GetButtonDown("Submit"))
            {
                if (activeMenuHierarchy.Peek().menuElements[activeIndex] is ControllerButton button)
                {
                    button.Press();
                }
            }

            // Backing out of the menu
            if (Input.GetButtonUp("Cancel") && activeMenuHierarchy.Count > 1)
            {
                CloseMenu();
            }

            if (activeIndex == -1) return;

            // Joystick is at rest, reset the timer
            if (Input.GetAxis("Vertical") <= 0.5 && Input.GetAxis("Vertical") >= -0.5)
            {
                timer = 0;
            }

            // Changing selected button if the joystick was previously at rest
            if (timer == 0)
            {
                // Move Up
                if (Input.GetAxis("Vertical") > sensitivityX)
                {
                    SetSelectedElement(activeIndex == -1 ? 0 : (activeIndex + activeMenuHierarchy.Peek().menuElements.Count - 1) % activeMenuHierarchy.Peek().menuElements.Count);
                    return;
                }

                // Move down
                if (Input.GetAxis("Vertical") < -sensitivityX)
                {
                    SetSelectedElement(activeIndex == -1 ? 0 : (activeIndex + activeMenuHierarchy.Peek().menuElements.Count + 1) % activeMenuHierarchy.Peek().menuElements.Count);
                    return;
                }


                // Moving slider
                if (activeMenuHierarchy.Peek().menuElements[activeIndex] is OptionSlider slider)
                {
                    // Move left
                    if (Input.GetAxis("Horizontal") < -sensitivityY)
                    {
                        slider.DecreaseValue(1);
                        timer = holdDelayHorizontal;
                        return;
                    }

                    // Move right
                    if (Input.GetAxis("Horizontal") > sensitivityY)
                    {
                        slider.IncreaseValue(1);
                        timer = holdDelayHorizontal;
                        return;
                    }
                }
            }

            if (timer > 0)
            {
                timer = Mathf.Max(0, timer -= Time.deltaTime);
            }
        }
    }

    public void OpenMenu(Menu m)
    {
        activeMenuHierarchy.Push(m);
        activeIndexHierarchy.Push(activeIndex);
        activeIndex = -1;
        m.panningData.state = UIPanState.MovingOnscreen;
    }

    public void CloseMenu()
    {
        Menu closedMenu = activeMenuHierarchy.Pop();
        if (activeIndexHierarchy.Count > 0)
        {
            activeIndex = activeIndexHierarchy.Pop();
        }
        closedMenu.panningData.state = UIPanState.MovingOffscreen;
    }

    public void SetSelectedElement(int index)
    {
        // No button previously selected, nothing to reset.
        if (activeIndex != -1)
            activeMenuHierarchy.Peek().menuElements[activeIndex].DeselectElement();

        activeIndex = index;
        activeMenuHierarchy.Peek().menuElements[index].SelectElement();
        timer = holdDelayVertical;
    }

    public void CancelControllerInput(NavigationElement navElement)
    {
        // Already not using controller, return
        if (activeIndex == -1) return;

        activeMenuHierarchy.Peek().menuElements[activeIndex].DeselectElement();
        EventSystem.current.SetSelectedGameObject(null);
        activeIndex = -1;
        timer = 0;
    }

    [ContextMenu("Current menu")] void CurrentMenu() => Debug.Log(activeMenuHierarchy.Peek().gameObject.name);
}
