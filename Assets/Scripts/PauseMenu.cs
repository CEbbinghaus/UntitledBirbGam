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
	public KeyCode DEBUG_Key;
	public AnimationCurve movementCurve;
	public AnimationCurve fadeCurve;
	public float duration;

	public PauseCurve() { }
	public PauseCurve(KeyCode _key, float _duration)
	{
		DEBUG_Key = _key;
		duration = _duration;
	}
}

public class PauseMenu : MonoBehaviour
{
	float progress;
	PauseState state;
	float cachedXPos;
	[SerializeField]
	new RectTransform transform;
	int screenHeight;

	[SerializeField]
	PauseCurve fallCurve = new PauseCurve(KeyCode.K, 2);
	[SerializeField]
	PauseCurve riseCurve = new PauseCurve(KeyCode.L, 0.3f);

	[SerializeField]
	CanvasGroup pauseFade;

	private void Awake()
	{
		screenHeight = Screen.height;
		transform = GetComponent<RectTransform>();
		cachedXPos = transform.anchoredPosition.x;
		transform.anchoredPosition = new Vector2(cachedXPos, screenHeight);
	}

	private void Update()
	{
		#region Debug stuff
		if (Input.GetKeyDown(fallCurve.DEBUG_Key))
			SetFallState();
		if (Input.GetKeyDown(riseCurve.DEBUG_Key))
			SetRiseState();
		#endregion

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

	public void ChangeState(int _state) => ChangeState((PauseState)_state);

	void ChangeState(PauseState _state)
	{
		state = _state;
		progress = 0;
		pauseFade.blocksRaycasts = ((int)_state - 1) == 1;
	}

	[ContextMenu("Fall")]
	void SetFallState() => ChangeState(PauseState.Falling);

	[ContextMenu("Rise")]
	void SetRiseState() => ChangeState(PauseState.Rising);
}
