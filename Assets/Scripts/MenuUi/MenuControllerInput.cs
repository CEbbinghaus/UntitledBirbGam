using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerInput : MonoBehaviour
{
    public bool inMenu = true;
    public float holdDelay = 0.5f;
    float timer = 0;
    List<MenuButton> menuButtons = new List<MenuButton>();
    int currentIndex;

    private void Start()
    {
        menuButtons = FindObjectsOfType<MenuButton>().OrderBy(mb => mb.index).ToList();
    }

    void Update()
    {
        if (inMenu)
        {
            // Pressing the button
            if (Input.GetButtonDown("Submit"))
            {
                menuButtons[currentIndex].Press();
                inMenu = false;
            }

            // Joystick is at rest, reset the timer
            if (Input.GetAxis("Vertical") <= 0.5 && Input.GetAxis("Vertical") >= -0.5)
            {
                timer = 0;
            }

            // Changing selected button if the joystick was previously at rest
            if (timer == 0)
            {
                if (Input.GetAxis("Vertical") > 0.5)
                {
                    Button currentButton = menuButtons[currentIndex].button;
                    currentButton.targetGraphic.color = currentButton.colors.normalColor;
                    currentIndex = (currentIndex + menuButtons.Count - 1) % menuButtons.Count;
                    Button newButton = menuButtons[currentIndex].button;
                    newButton.targetGraphic.color = newButton.colors.selectedColor;
                    timer = holdDelay;
                }
                else if (Input.GetAxis("Vertical") < -0.5)
                {
                    Button currentButton = menuButtons[currentIndex].button;
                    currentButton.targetGraphic.color = currentButton.colors.normalColor;
                    currentIndex = (currentIndex + menuButtons.Count + 1) % menuButtons.Count;
                    Button newButton = menuButtons[currentIndex].button;
                    newButton.targetGraphic.color = newButton.colors.selectedColor;
                    timer = holdDelay;
                }
            }

            if (timer > 0)
            {
                timer = Mathf.Max(0, timer -= Time.deltaTime);
            }
        }
    }
}
