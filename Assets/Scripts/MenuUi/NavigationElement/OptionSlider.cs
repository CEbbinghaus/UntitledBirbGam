using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSlider : NavigationElement
{
	public Image background;
	private Slider slider;

	private void Start()
	{
		slider = GetComponentInChildren<Slider>();
	}

	public override void SelectElement() => background.enabled = true;

	public override void DeselectElement() => background.enabled = false;

	public void IncreaseValue(float change) => SetValue(slider.value + change);

	public void DecreaseValue(float change) => SetValue(slider.value - change);

	private void SetValue(float value) => slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
}
