using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManager : Singleton<OrientationManager>
{
	ScreenOrientation cachedOrientation;

	// Events
	public static event OnChangeUIOrientation onChangeUIOrientation;
	public delegate void OnChangeUIOrientation(ScreenOrientation orientation);

	private void Awake()
	{
		RegisterInstance(this);
		cachedOrientation = Screen.orientation;
	}

	void Update()
	{
		if (Screen.orientation != cachedOrientation)
		{
			cachedOrientation = Screen.orientation;
			Debug.Log($"Rotating to {cachedOrientation}");
			onChangeUIOrientation?.Invoke(cachedOrientation);
		}
	}
}
