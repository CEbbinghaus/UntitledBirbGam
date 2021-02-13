using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButtonPauseOptions : ControllerButton
{
	private void Start()
	{
		optionsElement.cachedLocation = optionsElement.transform.anchoredPosition;
		optionsElement.transform.anchoredPosition = new Vector2(0, Screen.height);
	}

	[SerializeField]
	public UIPanElement optionsElement;
	public override void Press()
	{
		base.Press();
		ControllerInputPause.instance.menuState = MenuState.SubMenu;
		optionsElement.state = UIPanState.MovingOnscreen;
	}

	private void Update()
	{
		// If it's not meant to be moving, just quit here
		if (optionsElement.state == UIPanState.None) return;

		// Calculate how far into the curve we are
		optionsElement.progress = Mathf.Clamp(optionsElement.progress + (Time.unscaledDeltaTime / optionsElement.duration), 0, 1);

		// Set the position and fade
		switch (optionsElement.state)
		{
			case UIPanState.MovingOnscreen:
				optionsElement.transform.anchoredPosition =
					new Vector3(optionsElement.movementCurveXOnscreen.Evaluate(optionsElement.progress) * Screen.width, optionsElement.movementCurveYOnscreen.Evaluate(optionsElement.progress) * Screen.height);
				break;
			case UIPanState.MovingOffscreen:
				optionsElement.transform.anchoredPosition =
					new Vector3(optionsElement.movementCurveXOffscreen.Evaluate(optionsElement.progress) * Screen.width, optionsElement.movementCurveYOffscreen.Evaluate(optionsElement.progress) * Screen.height);
				break;
		}

		// Stop if at completion
		if (optionsElement.progress == 1)
		{
			optionsElement.progress = 0;
			optionsElement.state = UIPanState.None;
		}
	}
}
