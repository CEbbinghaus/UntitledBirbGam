using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
	enum State
	{
		None,
		Rising, 
		Falling
	}

	float progress;
	State state;
	float cachedXPos;
	[SerializeField]
	new RectTransform transform;
	int screenHeight;

	[SerializeField]
	AnimationCurve fallCurve;
	[SerializeField]
	float fallDuration;

	[SerializeField]
	AnimationCurve riseCurve;
	[SerializeField]
	float riseDuration;

	[SerializeField]
	Gradient g;

	private void Awake()
	{
		screenHeight = Screen.height;
		transform = GetComponent<RectTransform>();
		cachedXPos = transform.anchoredPosition.x;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			state = State.Falling;
		}
		switch (state)
		{
			case State.None:
				break;
			case State.Rising:
				Rise();
				break;
			case State.Falling:
				Fall();
				break;
		}
	}

	void Rise()
	{
		progress = Mathf.Clamp(progress + (Time.deltaTime / riseDuration), 0, 1);
		transform.anchoredPosition = new Vector3(cachedXPos, fallCurve.Evaluate(progress) * screenHeight);
		CheckProgess();
	}

	void Fall()
	{
		progress = Mathf.Clamp(progress + (Time.deltaTime / fallDuration), 0, 1);
		transform.anchoredPosition = new Vector3(cachedXPos, fallCurve.Evaluate(progress) * screenHeight);
		CheckProgess();
	}

	void CheckProgess()
	{
		if (progress == 1)
		{
			progress = 0;
			state = State.None;
		}
	}
}
