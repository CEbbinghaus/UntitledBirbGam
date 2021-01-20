using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum UIPanState
{
	None,
	MovingOnscreen,
	MovingOffscreen
}

[Serializable]
public class UIPanElement
{
	public RectTransform transform;
	public AnimationCurve movementCurveXOnscreen;
	public AnimationCurve movementCurveYOnscreen;
	public AnimationCurve movementCurveXOffscreen;
	public AnimationCurve movementCurveYOffscreen;
	public AnimationCurve fadeCurve;
	public float duration = 0.3f;
	[NonSerialized]	public float progress;
	[NonSerialized]	public Vector2 cachedLocation;
	[NonSerialized]	public UIPanState state;
}
public class UIPan : MonoBehaviour
{
	public static UIPan instance;
	Vector2Int screenSize;
	public GameObject landscapeUI;
	public GameObject portraitUI;
	[Space(10)]
	public UIPanElement optionsLandscape;
	public UIPanElement optionsPortrait;
	public UIPanElement creditsLandscape;
	public UIPanElement creditsPortrait;
	[NonSerialized] public UIPanElement activeOptions;
	[NonSerialized] public UIPanElement activeCredits;
	[SerializeField]
	CanvasGroup fade = new CanvasGroup();
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
	}

	// Start is called before the first frame update
	void Start()
    {
		screenSize = new Vector2Int(Screen.width, Screen.height);
		int largerDimension = screenSize.x > screenSize.y ? screenSize.x : screenSize.y;
		optionsLandscape.cachedLocation = optionsLandscape.transform.anchoredPosition;
		optionsPortrait.cachedLocation = optionsPortrait.transform.anchoredPosition;
		creditsLandscape.cachedLocation = creditsLandscape.transform.anchoredPosition;
		creditsPortrait.cachedLocation = creditsPortrait.transform.anchoredPosition;

		// Move them offscreen
		optionsLandscape.transform.anchoredPosition = new Vector2(largerDimension, optionsLandscape.cachedLocation.y);
		optionsPortrait.transform.anchoredPosition = new Vector2(optionsPortrait.cachedLocation.x, largerDimension);
		creditsLandscape.transform.anchoredPosition = new Vector2(largerDimension, creditsLandscape.cachedLocation.y);
		creditsPortrait.transform.anchoredPosition = new Vector2(creditsPortrait.cachedLocation.x, largerDimension);

		UpdateOrientation();
	}

    // Update is called once per frame
    void Update()
	{
		//if (Screen.orientation != cachedOrientation)
		if (ControllerInputMenu.instance.DEBUGOrientation != cachedOrientation)
		{
			UpdateOrientation();
		}
		AnimateElement(activeCredits);
		AnimateElement(activeOptions);
	}

	void UpdateOrientation()
	{
		//switch (Screen.orientation)
		switch (ControllerInputMenu.instance.DEBUGOrientation)
		{
			case ScreenOrientation.Portrait:
				// Set new orientation
				cachedOrientation = ScreenOrientation.Portrait;
				// Toggle active UI
				portraitUI.SetActive(true);
				landscapeUI.SetActive(false);
				activeOptions = optionsPortrait;
				activeCredits = creditsPortrait;
				// Sync animation progress/state, position will be synced immediately after this
				optionsPortrait.state = optionsLandscape.state;
				creditsPortrait.state = creditsLandscape.state;
				optionsPortrait.progress = optionsLandscape.progress;
				creditsPortrait.progress = creditsLandscape.progress;
				break;
			case ScreenOrientation.LandscapeLeft:
			case ScreenOrientation.LandscapeRight:
				// Set new orientation
				cachedOrientation = ScreenOrientation.Landscape;
				// Toggle active UI
				landscapeUI.SetActive(true);
				portraitUI.SetActive(false);
				activeOptions = optionsLandscape;
				activeCredits = creditsLandscape;
				// Sync animation progress/state, position will be synced immediately after this
				optionsLandscape.state = optionsPortrait.state;
				creditsLandscape.state = creditsPortrait.state;
				optionsLandscape.progress = optionsPortrait.progress;
				creditsLandscape.progress = creditsPortrait.progress;
				break;
			default:
				goto case ScreenOrientation.LandscapeRight;
		}

		// Force the menus into the correct locations
		UIPanElement submenu = ControllerInputMenu.instance.activeSubMenu;
		if (activeCredits != submenu)
			activeCredits.transform.anchoredPosition = new Vector3(activeCredits.movementCurveXOffscreen.Evaluate(1) * screenSize.x, activeCredits.movementCurveYOffscreen.Evaluate(1) * screenSize.y);
		if (activeOptions != submenu)
			activeOptions.transform.anchoredPosition = new Vector3(activeOptions.movementCurveXOffscreen.Evaluate(1) * screenSize.x, activeOptions.movementCurveYOffscreen.Evaluate(1) * screenSize.y);
		if (submenu.transform && submenu.progress == 0)
			submenu.transform.anchoredPosition = submenu.cachedLocation;
	}

	void AnimateElement(UIPanElement element)
	{
		// If it's not meant to be moving, just quit here
		if (element.state == UIPanState.None) return;

		// Calculate how far into the curve we are
		element.progress = Mathf.Clamp(element.progress + (Time.unscaledDeltaTime / element.duration), 0, 1);

		// Set the position and fade
		switch (element.state)
		{
			case UIPanState.MovingOnscreen:
				element.transform.anchoredPosition =
					new Vector3(element.movementCurveXOnscreen.Evaluate(element.progress) * screenSize.x, element.movementCurveYOnscreen.Evaluate(element.progress) * screenSize.y);
				fade.alpha = element.fadeCurve.Evaluate(element.progress);
				break;
			case UIPanState.MovingOffscreen:
				element.transform.anchoredPosition =
					new Vector3(element.movementCurveXOffscreen.Evaluate(element.progress) * screenSize.x, element.movementCurveYOffscreen.Evaluate(element.progress) * screenSize.y);
				fade.alpha = element.fadeCurve.Evaluate(1 - element.progress);
				break;
		}

		// Stop if at completion
		if (element.progress == 1)
		{
			element.progress = 0;
			element.state = UIPanState.None;
		}

	}

	public void ChangeState(UIPanElement element, UIPanState _state)
	{
		element.state = _state;
		element.progress = 0;
	}
}
