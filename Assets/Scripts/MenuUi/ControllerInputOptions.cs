using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInputOptions : MonoBehaviour
{
    public List<OptionSlider> sliders;
    int currentIndex = -1;
    float timerVertical;
    float timerHorizontal;
    public float holdDelayVertical = 0.5f;
    public float holdDelayHorizontal = 0.5f;
    public float sensitivity = 0.5f;

    void Update()
    {
        // -1 index is if mouse is currently being used
        if (Input.GetMouseButtonDown(0))
        {
            SetSelectedSlider(null);
        }

        // Joystick is at rest, reset the timer
        if (Input.GetAxis("Vertical") <= 0.5 && Input.GetAxis("Vertical") >= -0.5)
        {
            timerVertical = 0;
        }
        if (Input.GetAxis("Horizontal") <= 0.5 && Input.GetAxis("Horizontal") >= -0.5)
        {
            timerHorizontal = 0;
        }

        // Changing selected button if the joystick was previously at rest
        if (timerVertical == 0)
        {
            // Move Up
            if (Input.GetAxis("Vertical") > sensitivity)
            {
                SetSelectedSlider(currentIndex == -1 ? 0 : (currentIndex + sliders.Count - 1) % sliders.Count);
                return;
            }

            // Move down
            if (Input.GetAxis("Vertical") < -sensitivity)
            {
                SetSelectedSlider(currentIndex == -1 ? 0 : (currentIndex + sliders.Count + 1) % sliders.Count);
                return;
            }
        }
        if (timerHorizontal == 0)
        {
            if (currentIndex >= 0)
            {
                // Move Up
                if (Input.GetAxis("Horizontal") > sensitivity)
                {
                    Slider slider = sliders[currentIndex].slider;
                    slider.value = Mathf.Clamp(slider.value + 1, slider.minValue, slider.maxValue);
                    timerHorizontal = holdDelayHorizontal;
                    return;
                }

                // Move down
                if (Input.GetAxis("Horizontal") < -sensitivity)
                {
                    Slider slider = sliders[currentIndex].slider;
                    slider.value = Mathf.Clamp(slider.value - 1, slider.minValue, slider.maxValue);
                    timerHorizontal = holdDelayHorizontal;
                    return;
                }
            }
        }

        if (timerVertical > 0)
        {
            timerVertical = Mathf.Max(0, timerVertical -= Time.deltaTime);
        }
        if (timerHorizontal > 0)
        {
            timerHorizontal = Mathf.Max(0, timerHorizontal -= Time.deltaTime);
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
        timerVertical = holdDelayVertical;
    }

    private void OnDisable()
    {
        print("Resetting");
        SetSelectedSlider(null);
    }
}
