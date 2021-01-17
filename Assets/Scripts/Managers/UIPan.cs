﻿using System.Collections;
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
	public float duration;
	public float progress;
	public Vector2 cachedLocation;
	public UIPanState state;
}
public class UIPan : MonoBehaviour
{
	public static UIPan instance;
	Vector2 screenSize;
	public UIPanElement options;
	public UIPanElement credits;
	[SerializeField]
	CanvasGroup fade = new CanvasGroup();

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
		credits.cachedLocation = credits.transform.anchoredPosition;
		options.cachedLocation = options.transform.anchoredPosition;

		// Move them offscreen
		options.transform.anchoredPosition = new Vector2(screenSize.x, options.cachedLocation.y);
		credits.transform.anchoredPosition = new Vector2(screenSize.x, credits.cachedLocation.y);
    }

    // Update is called once per frame
    void Update()
	{
		AnimateElement(options);
		AnimateElement(credits);
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