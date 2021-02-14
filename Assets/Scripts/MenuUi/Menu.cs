using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public Vector2 defaultOffset;

	public List<NavigationElement> menuElements;

	[Space(20)]
	private bool animateElement;
	public UIPanElement panningData;

	private Vector2 screenSize;

	private void Start()
	{
		UpdateScreenSize(ScreenOrientation.AutoRotation);
		OrientationManager.onChangeUIOrientation += UpdateScreenSize;

		defaultOffset = new Vector2(defaultOffset.x * screenSize.x, defaultOffset.y * screenSize.y);
		panningData.transform.anchoredPosition += defaultOffset;

		panningData.doFade = panningData.fade;
		
		animateElement = panningData.transform;
	}

	private void Update()
	{
		if (animateElement)
		{
			// If it's not meant to be moving, just quit here
			if (panningData.state == UIPanState.None) return;

			// Calculate how far into the curve we are
			panningData.progress = Mathf.Clamp(panningData.progress + (Time.unscaledDeltaTime / panningData.duration), 0, 1);


			// Set the position and fade
			switch (panningData.state)
			{
				case UIPanState.MovingOnscreen:
					panningData.transform.anchoredPosition =
						new Vector3(panningData.movementCurveXOnscreen.Evaluate(panningData.progress) * screenSize.x, panningData.movementCurveYOnscreen.Evaluate(panningData.progress) * screenSize.y);
					if (panningData.doFade)
						panningData.fade.alpha = panningData.fadeCurve.Evaluate(1 - panningData.progress);
					if (panningData.alterTimescale)
						Time.timeScale = panningData.fadeCurve.Evaluate(panningData.progress);
					break;
				case UIPanState.MovingOffscreen:
					panningData.transform.anchoredPosition =
						new Vector3(panningData.movementCurveXOffscreen.Evaluate(panningData.progress) * screenSize.x, panningData.movementCurveYOffscreen.Evaluate(panningData.progress) * screenSize.y);
					if (panningData.doFade)
						panningData.fade.alpha = panningData.fadeCurve.Evaluate(panningData.progress);
					if (panningData.alterTimescale)
						Time.timeScale = panningData.fadeCurve.Evaluate(1 - panningData.progress);
					break;
			}

			// Stop if at completion
			if (panningData.progress == 1)
			{
				panningData.progress = 0;
				panningData.state = UIPanState.None;
			}
		}
	}

	public void ChangeState(UIPanState state)
	{
		if (animateElement)
			panningData.state = state;
	}

	private void UpdateScreenSize(ScreenOrientation orientation) => screenSize = new Vector2(Screen.width, Screen.height);
}
