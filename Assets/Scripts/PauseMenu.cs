using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum PauseState
{
	None,
	Rising,
	Falling
}

[Serializable]
public class PauseCurve
{
	public AnimationCurve movementCurve;
	public AnimationCurve fadeCurve;
	public float duration;

	public PauseCurve() {}
	public PauseCurve(float _duration)
	{
		duration = _duration;
	}
}

public class PauseMenu : MonoBehaviour
{
	public static PauseMenu currentInstance;

	float progress;
	PauseState state;
	bool paused;

	float cachedXPos;
	[SerializeField]
	new RectTransform transform;
	int screenHeight;

	[SerializeField]
	PauseCurve fallCurve = new PauseCurve(2);
	[SerializeField]
	PauseCurve riseCurve = new PauseCurve(0.3f);

	[SerializeField]
	CanvasGroup pauseFade;

	private void Awake()
	{
		currentInstance = this;
		screenHeight = Screen.height;
		transform = GetComponent<RectTransform>();
		cachedXPos = transform.anchoredPosition.x;
		transform.anchoredPosition = new Vector2(cachedXPos, screenHeight);
	}

	private void Update()
	{
		switch (state)
		{
			case PauseState.None:
				break;
			case PauseState.Rising:
				SetPosition(riseCurve);
				break;
			case PauseState.Falling:
				SetPosition(fallCurve);
				break;
		}
	}

	void SetPosition(PauseCurve pauseCurve)
	{
		// Calculate how far into the curve we are
		progress = Mathf.Clamp(progress + (Time.unscaledDeltaTime / pauseCurve.duration), 0, 1);
		// Set the position
		transform.anchoredPosition = new Vector3(cachedXPos, pauseCurve.movementCurve.Evaluate(progress) * screenHeight);
		pauseFade.alpha = pauseCurve.fadeCurve.Evaluate(progress);
		Time.timeScale = pauseCurve.fadeCurve.Evaluate(1 - progress);
		// Stop if at completion
		if (progress == 1)
		{
			progress = 0;
			state = PauseState.None;
		}
	}

	void ChangeState(PauseState _state)
	{
		state = _state;
		progress = 0;
		pauseFade.blocksRaycasts = ((int)_state - 1) == 1;
	}

	public void SetPaused(bool pauseState)
	{
		paused = pauseState;
		ChangeState(pauseState ? PauseState.Falling : PauseState.Rising);
	}

	public void TogglePause()
	{
		SetPaused(!paused);
	}
}
