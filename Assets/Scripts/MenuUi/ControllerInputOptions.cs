using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInputOptions : MonoBehaviour
{
    public List<OptionSlider> sliders;
    int currentIndex = -1;
    float timer;
    public float holdDelayVertical = 0.5f;
    public float holdDelayHorizontal = 0.5f;
    public float sensitivityX = 0.5f;
    public float sensitivityY = 0.7f;

    void Update()
    {
        if (ControllerInputMenu.instance.activeSubMenu == UIPan.instance.optionsPortrait)
        {

            // -1 index is if mouse is currently being used
            if (Input.GetMouseButtonDown(0))
            {
                SetSelectedSlider(null);
            }

            // Joystick is at rest, reset the timer
            if (Input.GetAxis("Vertical") <= 0.5 && Input.GetAxis("Vertical") >= -0.5 && Input.GetAxis("Horizontal") <= 0.5 && Input.GetAxis("Horizontal") >= -0.5)
            {
                timer = 0;
            }

            // Changing selected button if the joystick was previously at rest
            if (timer == 0)
            {
                // Move Up
                if (Input.GetAxis("Vertical") > sensitivityX)
                {
                    SetSelectedSlider(currentIndex == -1 ? 0 : (currentIndex + sliders.Count - 1) % sliders.Count);
                    return;
                }

                // Move down
                if (Input.GetAxis("Vertical") < -sensitivityX)
                {
                    SetSelectedSlider(currentIndex == -1 ? 0 : (currentIndex + sliders.Count + 1) % sliders.Count);
                    return;
                }
            }
            if (timer == 0)
            {
                if (currentIndex >= 0)
                {
                    // Move right
                    if (Input.GetAxis("Horizontal") > sensitivityY)
                    {
                        Slider slider = sliders[currentIndex].slider;
                        slider.value = Mathf.Clamp(slider.value + 1, slider.minValue, slider.maxValue);
                        timer = holdDelayHorizontal;
                        return;
                    }

                    // Move left
                    if (Input.GetAxis("Horizontal") < -sensitivityY)
                    {
                        Slider slider = sliders[currentIndex].slider;
                        slider.value = Mathf.Clamp(slider.value - 1, slider.minValue, slider.maxValue);
                        timer = holdDelayHorizontal;
                        return;
                    }
                }
            }

            if (timer > 0)
            {
                timer = Mathf.Max(0, timer -= Time.deltaTime);
            }
            if (timer > 0)
            {
                timer = Mathf.Max(0, timer -= Time.deltaTime);
            }
        }
    }

    void SetSelectedSlider(int? index)
    {
        if (index == null)
        {
            sliders.ForEach(s => s.background.enabled = false);
            currentIndex = -1;
            return;
        }

        if (currentIndex >= 0)
        {
            sliders[currentIndex].background.enabled = false;
        }
        currentIndex = (int)index;
        sliders[currentIndex].background.enabled = true;
        timer = holdDelayVertical;
    }

    private void OnDisable()
    {
        SetSelectedSlider(null);
    }
}
